package com.raven.tracing.kafkaConnector;

import java.util.Date;
import java.util.HashMap;
import java.util.Map;

/**
 * Created by klice on 2017/5/2.
 */
public class TraceLogs implements Mapable {
    public String TraceId;
    public String RpcId;
    public String MachineAddr;
    public Date StartTime;
    public Date EndTime;
    public double TimeLength;
    public boolean IsSuccess;
    public boolean IsException;
    public String Code;
    public String InvokeID;
    public String ServerHost;
    public String SystemID;
    public String SystemName;
    public String Protocol;
    public String SearchKey;
    public HashMap<String,Object> ProtocolHeader;
    public String ContextType;
    public long ResponseSize;
    public String Environment;
    public HashMap<String,Object> Extensions;

    public String getTraceId() {
        return TraceId;
    }

    public void setTraceId(String traceId) {
        TraceId = traceId;
    }

    public String getRpcId() {
        return RpcId;
    }

    public void setRpcId(String rpcId) {
        RpcId = rpcId;
    }

    public String getMachineAddr() {
        return MachineAddr;
    }

    public void setMachineAddr(String machineAddr) {
        MachineAddr = machineAddr;
    }

    public Date getStartTime() {
        return StartTime;
    }

    public void setStartTime(Date startTime) {
        StartTime = startTime;
    }

    public Date getEndTime() {
        return EndTime;
    }

    public void setEndTime(Date endTime) {
        EndTime = endTime;
    }

    public double getTimeLength() {
        return TimeLength;
    }

    public void setTimeLength(double timeLength) {
        TimeLength = timeLength;
    }

    public boolean isSuccess() {
        return IsSuccess;
    }

    public void setSuccess(boolean success) {
        IsSuccess = success;
    }

    public boolean isException() {
        return IsException;
    }

    public void setException(boolean exception) {
        IsException = exception;
    }

    public String getCode() {
        return Code;
    }

    public void setCode(String code) {
        Code = code;
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

    public String getProtocol() {
        return Protocol;
    }

    public void setProtocol(String protocol) {
        Protocol = protocol;
    }

    public String getSearchKey() {
        return SearchKey;
    }

    public void setSearchKey(String searchKey) {
        SearchKey = searchKey;
    }

    public HashMap<String, Object> getProtocolHeader() {
        return ProtocolHeader;
    }

    public void setProtocolHeader(HashMap<String, Object> protocolHeader) {
        ProtocolHeader = protocolHeader;
    }

    public String getContextType() {
        return ContextType;
    }

    public void setContextType(String contextType) {
        ContextType = contextType;
    }

    public long getResponseSize() {
        return ResponseSize;
    }

    public void setResponseSize(long responseSize) {
        ResponseSize = responseSize;
    }

    public String getEnvironment() {
        return Environment;
    }

    public void setEnvironment(String environment) {
        Environment = environment;
    }

    public HashMap<String, Object> getExtensions() {
        return Extensions;
    }

    public void setExtensions(HashMap<String, Object> extensions) {
        Extensions = extensions;
    }

    @Override
    public Map<String, Object> ToMap() {
        HashMap<String,Object> map = new HashMap<>(32);
        map.put("TraceId",TraceId);
        map.put("RpcId",RpcId);
        map.put("MachineAddr",MachineAddr);
        map.put("StartTime",StartTime);
        map.put("EndTime",EndTime);
        map.put("TimeLength",TimeLength);
        map.put("IsSuccess",IsSuccess);
        map.put("IsException",IsException);
        map.put("Code",Code);
        map.put("InvokeID",InvokeID);
        map.put("ServerHost",ServerHost);
        map.put("SystemID",SystemID);
        map.put("SystemName",SystemName);
        map.put("Protocol",Protocol);
        map.put("SearchKey",SearchKey);
        map.put("ProtocolHeader",ProtocolHeader);
        map.put("ContextType",ContextType);
        map.put("ResponseSize",ResponseSize);
        map.put("Environment",Environment);
        map.put("Extensions",Extensions);
        return map;
    }
}
