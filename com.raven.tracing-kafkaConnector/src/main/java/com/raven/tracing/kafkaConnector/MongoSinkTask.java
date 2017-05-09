package com.raven.tracing.kafkaConnector;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.apache.kafka.clients.consumer.OffsetAndMetadata;
import org.apache.kafka.common.TopicPartition;
import org.apache.kafka.connect.sink.SinkRecord;
import org.apache.kafka.connect.sink.SinkTask;
import org.bson.BSONObject;
import org.bson.Document;

import java.io.*;
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
    Class messageClass;
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
        try {
            messageClass = Class.forName( map.get("messageClass"));
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }

        repository = new MongodbRepository(this.mongoBrokers, this.messageDb);
        dataBuffer = new ArrayList<>(Integer.parseInt(batchFlushSize) * 2);
        logger = Logger.getLogger("MongoSinkTask");
    }

    @Override
    public void put(Collection<SinkRecord> collection) {
        for (SinkRecord record : collection) {
            try {
                byte[] valueBs = (byte[]) record.value();
                Document document = convertJsonByteArray(valueBs);
                synchronized (dataBuffer) {
                    dataBuffer.add(document);
                }
            }catch (Exception ex){
                StringWriter writer = new StringWriter();
                ex.printStackTrace(new PrintWriter(writer));
                logger.warning(writer.toString());
            }
        }
    }

    Document convertBsonByteArray(byte[] value){
        BSONObject bsonObject = org.bson.BSON.decode(value);
        Document document = new Document(bsonObject.toMap());
        return document;
    }

    Document convertJsonByteArray(byte[] value) throws IOException {
        String valueString = new String(value, Charset.forName("utf-8"));
//        logger.warning(valueString);
        Object message =  JsonConvert.fromJson(valueString,messageClass);
        if(message instanceof Mapable){
            Document document = new Document(((Mapable) message).ToMap());
            return document;
        }
        throw new InvalidClassException(messageClass.getCanonicalName(),"messageClass should implements com.raven.tracing.kafkaConnector.Mapable");
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
