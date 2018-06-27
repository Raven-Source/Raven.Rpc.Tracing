using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raven.Rpc.Tracing.Record.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    public class RabbitMQClient : IDisposable
    {
        private IDataSerializer serializer;

        private ILoger loger;
        private static readonly object objLock = new object();
        private IConnection _connection;
        private ConnectionFactory factory;
        private IConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    try
                    {
                        Monitor.Enter(objLock);
                        if (_connection == null)
                        {
                            _connection = CreateConnection();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        Monitor.Exit(objLock);
                    }

                    //lock (objLock)
                    //{
                    //    if (_connection == null)
                    //    {
                    //        _connection = CreateConnection();
                    //    }
                    //}
                }
                return _connection;
            }
        }       

        private System.Collections.Concurrent.BlockingCollection<StrongBox<QueueMessage>> _queue;
        private int _maxQueueCount;

        private IBasicProperties propertiesEmpty = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="userName"></param>
        /// <param name="port"></param>
        /// <param name="password"></param>
        /// <param name="maxQueueCount"></param>
        /// <param name="serializerType"></param>
        /// <param name="loger"></param>
        /// <param name="writeWorkerTaskNumber"></param>
        private RabbitMQClient(string hostName, string userName, string password, int? port, int maxQueueCount
            , SerializerType serializerType, ILoger loger = null, short writeWorkerTaskNumber = 4)
        {
            factory = new ConnectionFactory();
            factory.HostName = hostName;
            if (!port.HasValue || port.Value < 0)
            {
                factory.Port = 5672;
            }
            else
            {
                factory.Port = port.Value;
            }
            factory.Password = password;
            factory.UserName = userName;

            serializer = SerializerFactory.Create(serializerType);
            _queue = new System.Collections.Concurrent.BlockingCollection<StrongBox<QueueMessage>>();
            
            _maxQueueCount = maxQueueCount > 0 ? maxQueueCount : Options.DefaultMaxQueueCount;
            

            this.loger = loger;

            var scheduler = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, writeWorkerTaskNumber).ExclusiveScheduler;
            TaskFactory taskFactory = new TaskFactory(scheduler);

            for (int i = 0; i < writeWorkerTaskNumber; i++)
            {
                taskFactory.StartNew(QueueToWrite, TaskCreationOptions.LongRunning);
            }
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static RabbitMQClient GetInstance(Options options)
        {
            return new RabbitMQClient(options.HostName, options.UserName, options.Password, options.Port, options.MaxQueueCount
                , options.SerializerType, options.Loger, options.WriteWorkerTaskNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IConnection CreateConnection()
        {
            return factory.CreateConnection();
        }

        /// <summary>
        /// 批量接收
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName">队列名</param>
        /// <param name="exchangeName"></param>
        /// <param name="noAck"></param>
        /// <returns></returns>
        public List<T> ReceiveBatch<T>(string queueName, string exchangeName = null, bool noAck = false)
        {
            List<T> list = new List<T>();
            List<BasicGetResult> resList = BasicDequeueBatch(exchangeName, queueName, noAck: noAck);
            foreach (var res in resList)
            {
                try
                {
                    T obj = serializer.Deserialize<T>(res.Body);
                    list.Add(obj);
                }
                catch (Exception ex)
                {
                    RecordException(ex, res.Body);
                }
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="exchangeName"></param>
        /// <param name="noAck"></param>
        /// <returns></returns>
        public T ReceiveSingle<T>(string queueName, string exchangeName = null, bool noAck = false)
        {
            T model = default(T);
            BasicGetResult res = BasicDequeue(exchangeName, queueName, noAck: noAck);
            if (res != null)
            {
                try
                {
                    model = serializer.Deserialize<T>(res.Body);
                }
                catch (Exception ex)
                {
                    RecordException(ex, res.Body);
                }
            }
            return model;
        }

        /// <summary>
        /// 注册Receive
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueName"></param>
        /// <param name="callback"></param>
        /// <param name="exchangeName"></param>
        /// <param name="exchangeType"></param>
        /// <param name="noAck"></param>
        /// <returns></returns>
        public IModel RegisterReceive<T>(string queueName, Func<T, bool> callback, string exchangeName = "", ExchangeType exchangeType = ExchangeType.Default, bool noAck = false)
        {
            try
            {
                IModel channel = Connection.CreateModel();
                if (exchangeType != ExchangeType.Default)
                {
                    string strExchangeType = ExchangeTypeDataDict.ExchangeTypeDict[exchangeType];
                    channel.ExchangeDeclare(exchangeName, strExchangeType);
                }

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (o, res) =>
                {
                    bool isOk = false;
                    try
                    {
                        var model = serializer.Deserialize<T>(res.Body);
                        if (callback != null)
                        {
                            isOk = callback(model);
                        }

                    }
                    catch (Exception ex)
                    {
                        RecordException(ex, res.Body);
                    }

                    if (!noAck && isOk)
                    {
                        channel.BasicAck(res.DeliveryTag, false);
                    }
                };

                channel.BasicConsume(queueName, noAck, consumer);

                return channel;
            }
            catch (Exception ex)
            {
                if (!Monitor.IsEntered(objLock))
                {
                    if (_connection != null)
                    {
                        try
                        {
                            _connection.Close(100);
                            _connection.Dispose();
                        }
                        catch
                        {
                        }
                    }
                    _connection = null;
                }
                RecordException(ex, null);
            }

            return null;
        }

        /// <summary>
        /// 异步入队
        /// </summary>
        /// <param name="queueName">队列名</param>
        /// <param name="dataObj">入队数据</param>
        /// <param name="persistent">数据是否持久化</param>
        /// <param name="durableQueue">队列是否持久化</param>
        public void Send(string queueName, object dataObj, bool persistent = false, bool durableQueue = false)
        {
            if (_queue.Count < _maxQueueCount)
            {
                var qm = new QueueMessage();
                qm.exchangeName = "";
                qm.queueName = queueName;
                qm.data = dataObj;
                qm.persistent = persistent;
                qm.durableQueue = durableQueue;
                qm.exchangeType = ExchangeType.Default;

                _queue.Add(new StrongBox<QueueMessage>(qm));
            }
        }


        /// <summary>
        /// 异步入队
        /// </summary>
        /// <param name="queueName">队列名</param>
        /// <param name="dataObj">入队数据</param>
        /// <param name="persistent">数据是否持久化</param>
        /// <param name="durableQueue">队列是否持久化</param>
        public void SendSync(string queueName, object dataObj, bool persistent = false, bool durableQueue = false)
        {
            BasicEnqueue("", queueName, dataObj, persistent, durableQueue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="dataObj"></param>
        public void Publish(string exchangeName, object dataObj)
        {
            if (_queue.Count < _maxQueueCount)
            {
                var qm = new QueueMessage();
                qm.exchangeName = exchangeName;
                qm.queueName = "";
                qm.data = dataObj;
                qm.persistent = false;
                qm.durableQueue = false;
                qm.exchangeType = ExchangeType.Fanout;
                
                _queue.Add(new StrongBox<QueueMessage>(qm));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchangeName"></param>
        /// <param name="callback"></param>
        public IModel Subscribe<T>(string exchangeName, Action<T> callback)
        {
            return BasicQueueBind(exchangeName, (o, res) =>
            {
                try
                {
                    var model = serializer.Deserialize<T>(res.Body);
                    if (callback != null)
                    {
                        callback(model);
                    }
                }
                catch (Exception ex)
                {
                    RecordException(ex, res.Body);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="exchangeType"></param>
        /// <param name="noAck"></param>
        /// <returns></returns>
        private List<BasicGetResult> BasicDequeueBatch(string exchangeName, string queueName, ExchangeType exchangeType = ExchangeType.Default, bool noAck = false)
        {
            List<BasicGetResult> resList = new List<BasicGetResult>();
            try
            {
                using (IModel channel = Connection.CreateModel())
                {
                    if (exchangeType != ExchangeType.Default)
                    {
                        string strExchangeType = ExchangeTypeDataDict.ExchangeTypeDict[exchangeType];
                        channel.ExchangeDeclare(exchangeName, strExchangeType);
                    }
                    while (true)
                    {
                        BasicGetResult res = channel.BasicGet(queueName, noAck);
                        if (res != null)
                        {
                            resList.Add(res);
                            if (!noAck)
                            {
                                channel.BasicAck(res.DeliveryTag, false);
                            }
                        }
                        else
                        {
                            break;
                        }

                        if (resList.Count >= 5000)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Monitor.IsEntered(objLock))
                {
                    if (_connection != null)
                    {
                        try
                        {
                            _connection.Close(100);
                            _connection.Dispose();
                        }
                        catch
                        {
                        }
                    }
                    _connection = null;
                }
                RecordException(ex, null);
            }

            return resList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="exchangeType"></param>
        /// <param name="noAck"></param>
        /// <returns></returns>
        private BasicGetResult BasicDequeue(string exchangeName, string queueName, ExchangeType exchangeType = ExchangeType.Default, bool noAck = false)
        {
            BasicGetResult res = null;
            try
            {
                using (IModel channel = Connection.CreateModel())
                {
                    if (exchangeType != ExchangeType.Default)
                    {
                        string strExchangeType = ExchangeTypeDataDict.ExchangeTypeDict[exchangeType];
                        channel.ExchangeDeclare(exchangeName, strExchangeType);
                    }

                    res = channel.BasicGet(queueName, noAck);
                    if (res != null)
                    {
                        if (!noAck)
                        {
                            channel.BasicAck(res.DeliveryTag, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Monitor.IsEntered(objLock))
                {
                    if (_connection != null)
                    {
                        try
                        {
                            _connection.Close(100);
                            _connection.Dispose();
                        }
                        catch
                        {
                        }
                    }
                    _connection = null;
                }
                RecordException(ex, null);
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IModel BasicQueueBind(string exchangeName, EventHandler<BasicDeliverEventArgs> callback)
        {
            try
            {
                IModel channel = Connection.CreateModel();

                string strExchangeType = ExchangeTypeDataDict.ExchangeTypeDict[ExchangeType.Fanout];
                channel.ExchangeDeclare(exchangeName, strExchangeType);

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queueName, exchangeName, "");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += callback;

                channel.BasicConsume(queueName, true, consumer);

                return channel;                

            }
            catch (Exception ex)
            {
                if (!Monitor.IsEntered(objLock))
                {
                    if (_connection != null)
                    {
                        try
                        {
                            _connection.Close(100);
                            _connection.Dispose();
                        }
                        catch
                        {
                        }
                    }
                    _connection = null;
                }
                RecordException(ex, null);
            }

            return null;
        }


        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="dataObj"></param>
        /// <param name="persistent">数据是否持久化</param>
        /// <param name="durableQueue">队列是否持久化</param>
        /// <param name="exchangeType">默认为空，值为fanout是支持订阅发布模型</param>
        private bool BasicEnqueue(string exchangeName, string queueName, object dataObj, bool persistent = false, bool durableQueue = false, ExchangeType exchangeType = ExchangeType.Default)
        {
            try
            {
                using (IModel channel = Connection.CreateModel())
                {
                    if (exchangeType != ExchangeType.Default)
                    {
                        string strExchangeType = ExchangeTypeDataDict.ExchangeTypeDict[exchangeType];
                        channel.ExchangeDeclare(exchangeName, strExchangeType);
                    }
                    //if (exchangeType == ExchangeType.Fanout)
                    //{
                    //    exchangeName = "publish";
                    //}
                    if (durableQueue)
                    {
                        channel.QueueDeclare(queueName, true, false, false, null);
                    }

                    IBasicProperties properties = propertiesEmpty;
                    if (persistent)
                    {
                        properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                    }
                    byte[] data = serializer.Serialize(dataObj);
                    dataObj = null;
                    channel.BasicPublish(exchangeName, queueName, properties, data);


                    return true;
                }
            }
            catch (Exception ex)
            {
                if (!Monitor.IsEntered(objLock))
                {
                    if (_connection != null)
                    {
                        try
                        {
                            _connection.Close(100);
                            _connection.Dispose();
                        }
                        catch
                        {
                        }
                    }
                    _connection = null;
                }
                RecordException(ex, dataObj);
                dataObj = null;

                return false;
                //throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void QueueToWrite()
        {
            while (true)
            {
                StrongBox<QueueMessage> item = _queue.Take();
                QueueMessage qm = item.Value;
                item.Value = null;

                if (!BasicEnqueue(qm.exchangeName, qm.queueName, qm.data, qm.persistent, qm.durableQueue, qm.exchangeType))
                {
                    loger.LogError(new Exception(), qm);
                }

                qm = null;
                item = null;
            }
            
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="dataObj"></param>
        private void RecordException(Exception ex, object dataObj)
        {
            if (loger != null)
            {
                loger.LogError(ex, dataObj);
            }
        }


        #region Dispose

        private bool disposed = false;

        /// <summary>
        /// 必须，以备程序员忘记了显式调用Dispose方法
        /// </summary> 
        ~RabbitMQClient()
        {
            //必须为false
            Dispose(false);
        }
        /// <summary>
        /// 实现IDisposable中的Dispose方法
        /// </summary>
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // 那么这个方法是被客户直接调用的,那么托管的,和非托管的资源都可以释放
                if (disposing)
                {
                    // 释放 托管资源
                    if (Connection != null)
                    {
                        Connection.Close();
                        Connection.Dispose();
                    }

                    if (_queue != null)
                    {
                        _queue.Dispose();
                    }

                    //if (queueWorkThread != null)
                    //{
                    //    try
                    //    {
                    //        queueWorkThread.Abort();
                    //        queueWorkThread = null;
                    //    }
                    //    catch { }
                    //}
                }

                //释放非托管资源

                disposed = true;
            }
        }

        #endregion

    }

    /// <summary>
    /// 队列消息
    /// </summary>
    internal class QueueMessage
    {
        public string exchangeName;
        public string queueName;
        public object data;
        public bool persistent;
        public bool durableQueue;
        public ExchangeType exchangeType;
    }
}
