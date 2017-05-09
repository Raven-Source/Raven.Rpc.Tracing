package com.raven.tracing.kafkaConnector;

import org.apache.kafka.common.config.ConfigDef;
import org.apache.kafka.connect.connector.Task;
import org.apache.kafka.connect.sink.SinkConnector;

import java.util.*;

/**
 * Created by klice on 2017/4/18.
 */
public class MongoSinkConnector extends SinkConnector {

    String mongoBrokers;
    String topics;
    String messageCollection;
    String batchFlushSize;
    String messageDb;
    String messageClass;

    @Override
    public String version() {
        return "1.0";
    }

    @Override
    public void start(Map<String, String> map) {
        mongoBrokers = map.get("mongoBrokers");
        topics = map.get("topics");
        messageCollection = map.get("messageCollection");
        messageDb = map.get("messageDb");
        batchFlushSize = map.get("batchFlushSize");
        messageClass = map.get("messageClass");
    }

    @Override
    public Class<? extends Task> taskClass() {
        return MongoSinkTask.class;
    }

    @Override
    public List<Map<String, String>> taskConfigs(int i) {
        List<Map<String,String>> taskConfigs = new ArrayList<Map<String, String>>(i);
        for(int j = 0; j<i;j++){
            HashMap<String,String> configMap = new HashMap<String, String>();
            configMap.put("mongoBrokers",this.mongoBrokers);
            configMap.put("topics",this.topics);
            configMap.put("messageCollection",this.messageCollection);
            configMap.put("batchFlushSize",this.batchFlushSize);
            configMap.put("messageDb",this.messageDb);
            configMap.put("messageClass",this.messageClass);
            taskConfigs.add(j,configMap);
        }
        return taskConfigs;
    }

    @Override
    public void stop() {

    }

    @Override
    public ConfigDef config() {
        return null;
    }
}
