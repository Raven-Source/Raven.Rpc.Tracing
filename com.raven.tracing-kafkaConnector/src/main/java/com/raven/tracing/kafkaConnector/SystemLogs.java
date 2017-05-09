package com.raven.tracing.kafkaConnector;

import java.util.Date;
import java.util.HashMap;
import java.util.Map;

/**
 * Created by klice on 2017/5/8.
 */
public class SystemLogs implements Mapable {
    public String TraceId;
    public double TimeLength;
    public boolean IsException;
    public Integer Level;
    public String SystemID;
    public String SystemName;
    public String Environment;
    public String InvokeID;
    public String ServerHost;
    public String Code;
    public String Content;
    public String StackTrace;
    public Date CreateTime;
    public String SearchKey;

    public String getTraceId() {
        return TraceId;
    }

    public void setTraceId(String traceId) {
        TraceId = traceId;
    }

    public double getTimeLength() {
        return TimeLength;
    }

    public void setTimeLength(double timeLength) {
        TimeLength = timeLength;
    }

    public boolean isException() {
        return IsException;
    }

    public void setException(boolean exception) {
        IsException = exception;
    }

    public Integer getLevel() {
        return Level;
    }

    public void setLevel(Integer level) {
        Level = level;
    }

    public String getSystemID() {
        return SystemID;
    }

    public void setSystemID(String systemID) {
        SystemID = systemID;
    }

    public String getSystemName() {
        return SystemName;
    }

    public void setSystemName(String systemName) {
        SystemName = systemName;
    }

    public String getEnvironment() {
        return Environment;
    }

    public void setEnvironment(String environment) {
        Environment = environment;
    }

    public String getInvokeID() {
        return InvokeID;
    }

    public void setInvokeID(String invokeID) {
        InvokeID = invokeID;
    }

    public String getServerHost() {
        return ServerHost;
    }

    public void setServerHost(String serverHost) {
        ServerHost = serverHost;
    }

    public String getCode() {
        return Code;
    }

    public void setCode(String code) {
        Code = code;
    }

    public String getContent() {
        return Content;
    }

    public void setContent(String content) {
        Content = content;
    }

    public String getStackTrace() {
        return StackTrace;
    }

    public void setStackTrace(String stackTrace) {
        StackTrace = stackTrace;
    }

    public Date getCreateTime() {
        return CreateTime;
    }

    public void setCreateTime(Date createTime) {
        CreateTime = createTime;
    }

    public String getSearchKey() {
        return SearchKey;
    }

    public void setSearchKey(String searchKey) {
        SearchKey = searchKey;
    }

    @Override
    public Map<String, Object> ToMap() {
        Map<String,Object> map = new HashMap<>(32);
        map.put("TraceId",TraceId);
        map.put("TimeLength",TimeLength);
        map.put("IsException",IsException);
        map.put("Level",Level);
        map.put("SystemID",SystemID);
        map.put("SystemName",SystemName);
        map.put("Environment",Environment);
        map.put("InvokeID",InvokeID);
        map.put("ServerHost",ServerHost);
        map.put("Code",Code);
        map.put("Content",Content);
        map.put("StackTrace",StackTrace);
        map.put("CreateTime",CreateTime);
        map.put("SearchKey",SearchKey);
        return null;
    }
}
