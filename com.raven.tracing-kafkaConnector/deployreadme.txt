1、搭建kafka集群
参考官网说明

2、创建存储offset、config、status的topic（以下localhost:2181为kafka集群使用的zookeeper服务）
bin/kafka-topics.sh --zookeeper localhost:2181 --create --topic tracingconnectoffsets --partitions 10 --replication-factor 2
bin/kafka-topics.sh --zookeeper localhost:2181 --create --topic tracingconnectconfigs --partitions 1 --replication-factor 5
bin/kafka-topics.sh --zookeeper localhost:2181 --create --topic tracingconnectstatus --partitions 10 --replication-factor 2

3、创建存储数据的topic
bin/kafka-topics.sh --zookeeper localhost:2181 --create --topic raven_trace_logs --partitions 10 --replication-factor 1
bin/kafka-topics.sh --zookeeper localhost:2181 --create --topic raven_sys_logs --partitions 10 --replication-factor 1

4、拷贝jar到kafka的libs目录下
com.raven.tracing.kafkaConnector-1.0-SNAPSHOT.jar
mongodb-driver-async-3.4.2.jar
mongodb-driver-core-3.4.2.jar
mongo-java-driver-3.4.2.jar
bson-3.4.2.jar

5、修改connect服务配置文件config/connect-distributed.properties
#kafka集群地址
bootstrap.servers=localhost:9092,localhost:9093,localhost:9094,localhost:9095,localhost:9096
#Connect集群id，注意不要和ConsumerGroupId冲突
group.id=tracing-connect-cluster
value.converter=com.raven.tracing.kafkaConnector.RawConverter
key.converter.schemas.enable=false
value.converter.schemas.enable=false
#写库时间间隔
offset.flush.interval.ms=1000
#存储offset的topic，此topic应该是有多个分区（partition）并且备份（replicated）
offset.storage.topic=tracingconnectoffsets
#存储config的topic，此topic应该是单个分区（partition）并且多份备份（replicated）
config.storage.topic=tracingconnectconfigs
#存储status的topic，此topic应该是有多个分区（partition）并且备份（replicated）
status.storage.topic=tracingconnectstatus
#管理接口端口，若一台服务器部署多个进程，需要配置成不同端口
rest.port=8083

6、启动connect服务（可以在多台服务器启动，单台也可启动多个进程）
bin/connector-distributed.sh config/connect-distributed.properties

7、通过管理接口创建connector（此处localhost:8083为刚才启动的connect服务地址）
post http://localhost:8083/connectors （注意需要设置头，Content-Type: Application/json）
{
"name":"tracing-tracelog-sink-distributed",
"config":{
	"connector.class":"com.raven.tracing.kafkaConnector.MongoSinkConnector",
	"tasks.max":10,
	"topics":"raven_trace_logs",
	"mongoBrokers":"mongodb://10.161.173.80:27001,10.160.39.59:28002,10.29.186.4:28002/?slaveOk=true",
	"messageDb":"RavenLogs",
	"messageCollection":"TraceLogs",
	"batchFlushSize":10000
	}
}
post http://localhost:8083/connectors
{
"name":"tracing-syslog-sink-distributed",
"config":{
	"connector.class":"com.raven.tracing.kafkaConnector.MongoSinkConnector",
	"tasks.max":10,
	"topics":"raven_sys_logs",
	"mongoBrokers":"mongodb://10.161.173.80:27001,10.160.39.59:28002,10.29.186.4:28002/?slaveOk=true",
	"messageDb":"RavenLogs",
	"messageCollection":"SystemLogs",
	"batchFlushSize":10000
	}
}

8、检查connector是否创建成功
get http://localhost:8083/connectors
get http://localhost:8083/connectors/tracing-tracelog-sink-distributed
get http://localhost:8083/connectors/tracing-syslog-sink-distributed

9、若需要部署其他connect服务器，只需要重复执行第4、5、6步骤就可以了