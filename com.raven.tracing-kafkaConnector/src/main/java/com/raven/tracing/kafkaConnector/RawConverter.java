package com.raven.tracing.kafkaConnector;

import org.apache.kafka.connect.data.Schema;
import org.apache.kafka.connect.data.SchemaAndValue;
import org.apache.kafka.connect.storage.Converter;

import java.util.Map;

/**
 * Created by klice on 2017/4/20.
 */
public class RawConverter implements Converter {
    @Override
    public void configure(Map<String, ?> map, boolean b) {

    }

    @Override
    public byte[] fromConnectData(String s, Schema schema, Object o) {
        return o.toString().getBytes();
    }

    @Override
    public SchemaAndValue toConnectData(String s, byte[] bytes) {
        return new SchemaAndValue(null,bytes);
    }
}
