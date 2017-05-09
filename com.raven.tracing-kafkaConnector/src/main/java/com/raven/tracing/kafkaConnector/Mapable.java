package com.raven.tracing.kafkaConnector;

import java.util.Map;

/**
 * Created by klice on 2017/5/2.
 */
public interface Mapable {
    Map<String,Object> ToMap();
}
