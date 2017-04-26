package com.raven.tracing.kafkaConnector;

import org.apache.kafka.clients.consumer.OffsetAndMetadata;
import org.apache.kafka.common.TopicPartition;
import org.apache.kafka.connect.sink.SinkRecord;
import org.apache.kafka.connect.sink.SinkTask;
import org.bson.Document;

import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Map;
import java.util.logging.Logger;

/**
 * Created by klice on 2017/4/18.
 */
public class MongoSinkTask extends SinkTask {

    String mongoBrokers;
    String messageDb;
    String messageCollection;
    String batchFlushSize;
    Logger logger;

    MongodbRepository repository;
    ArrayList<Document> dataBuffer;

    public String version() {
        return "1.0";
    }

    @Override
    public void start(Map<String, String> map) {
        mongoBrokers = map.get("mongoBrokers");
        messageDb = map.get("messageDb");
        messageCollection = map.get("messageCollection");
        batchFlushSize = map.get("batchFlushSize");

        repository = new MongodbRepository(this.mongoBrokers, this.messageDb);
        dataBuffer = new ArrayList<>(Integer.parseInt(batchFlushSize) * 2);
        logger = Logger.getLogger("MongoSinkTask");
    }

    @Override
    public void put(Collection<SinkRecord> collection) {
        for (SinkRecord record : collection) {
            try {
                byte[] valueBs = (byte[]) record.value();
                String valueJson = new String(valueBs, Charset.forName("utf-8"));
                Document document = Document.parse(valueJson);
                synchronized (dataBuffer) {
                    dataBuffer.add(document);
                }
            }catch (Exception ex){
                logger.warning(ex.toString());
            }
        }
    }

    @Override
    public void flush(Map<TopicPartition, OffsetAndMetadata> map) {
        if (dataBuffer.isEmpty())
            return;
        //copy data buffer
        ArrayList<Document> copy;
        synchronized (dataBuffer) {
            if (dataBuffer.isEmpty())
                return;
            copy = new ArrayList<>(dataBuffer);
            dataBuffer.clear();
        }
        int size = copy.size();
        logger.info(String.format("start insert %d messages", size));
        repository.insertMany(this.messageCollection, copy);
        logger.info(String.format("complete insert %d messages", size));
    }

    @Override
    public void stop() {

    }
}
