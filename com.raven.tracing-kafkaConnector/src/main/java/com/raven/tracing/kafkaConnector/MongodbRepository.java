package com.raven.tracing.kafkaConnector;

import com.mongodb.async.client.MongoClient;
import com.mongodb.async.client.MongoClients;
import com.mongodb.async.client.MongoCollection;
import com.mongodb.client.model.InsertManyOptions;
import org.bson.Document;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReadWriteLock;
import java.util.concurrent.locks.ReentrantReadWriteLock;

/**
 * Created by klice on 2017/4/19.
 */
public class MongodbRepository {

    static final ReadWriteLock lock = new ReentrantReadWriteLock();
    static final Lock writeLock = lock.writeLock();
    static HashMap<String, MongoClient> clientHashMap = new HashMap<>();

    static MongoClient GetClient(String broker) {
        if (clientHashMap.containsKey(broker))
            return clientHashMap.get(broker);

        writeLock.lock();
        try {
            if (clientHashMap.containsKey(broker))
                return clientHashMap.get(broker);

            MongoClient client = MongoClients.create(broker);
            clientHashMap.put(broker, client);
            return client;
        } finally {
            writeLock.unlock();
        }
    }


    String db;
    String broker;

    public MongodbRepository(String broker, String db) {
        this.broker = broker;
        this.db = db;
    }

    public void insertMany(String collectionName, ArrayList<Document> documents) {
        MongoClient client = GetClient(this.broker);
        MongoCollection<Document> collection = client.getDatabase(this.db).getCollection(collectionName);
        InsertManyOptions options = new InsertManyOptions();
        options.bypassDocumentValidation(true).ordered(false);
        collection.insertMany(documents, options, (aVoid, throwable) -> {
            if (throwable != null)
                throwable.printStackTrace();
        });
    }
}
