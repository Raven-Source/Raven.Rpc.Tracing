package com.raven.tracing.kafkaConnector;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.bson.json.JsonReader;

//import javax.json.Json;
//import javax.json.JsonObject;
import java.io.IOException;
import java.io.StringReader;
import java.text.DateFormat;
import java.text.FieldPosition;
import java.text.ParsePosition;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;

/**
 * Created by klice on 2017/4/19.
 */
public class JsonConvert {
    final static ObjectMapper mapper = new ObjectMapper();
//    static {
//        DateFormat format = new SimpleDateFormatGroup("yyyy-MM-dd'T'HH:mm:ss.SSSSSSXXX","yyyy-MM-dd'T'HH:mm:ss.SSSSSSSXXX","yyyy-MM-dd'T'HH:mm:ss.SSSSSXXX");
//        mapper.setDateFormat(format);
//    }

//    public static void main(String[] args){
//        String s = "{\"d1\":\"2017-05-08T12:36:42.8882281+08:00\",\"s1\":\"aaaa\",\"ex\":{\"ed1\":Date(1494235868000)}}";
//        try {
//            StringReader stringReader = new StringReader(s);
//            javax.json.JsonReader reader = Json.createReader(stringReader);
//
//            JsonObject object = reader.readObject();
//            fromJson(s,TestObject.class);
//        } catch (IOException e) {
//            e.printStackTrace();
//        }
//    }

//    static class TestObject{
//        Date d1;
//        String s1;
//
//        public HashMap<String, Object> getEx() {
//            return ex;
//        }
//
//        public void setEx(HashMap<String, Object> ex) {
//            this.ex = ex;
//        }
//
//        HashMap<String,Object> ex;
//
//        public Date getD1() {
//            return d1;
//        }
//
//        public void setD1(Date d1) {
//            this.d1 = d1;
//        }
//
//        public String getS1() {
//            return s1;
//        }
//
//        public void setS1(String s1) {
//            this.s1 = s1;
//        }
//    }

    static class SimpleDateFormatGroup extends DateFormat {

        ArrayList<SimpleDateFormat> simpleDateFormats;

        private SimpleDateFormatGroup(){

        }

        public SimpleDateFormatGroup(String...formats){
            simpleDateFormats = new ArrayList<>(formats.length);
            for(String format : formats){
                simpleDateFormats.add(new SimpleDateFormat(format));
            }
        }

        @Override
        public StringBuffer format(Date date, StringBuffer toAppendTo, FieldPosition fieldPosition) {
            SimpleDateFormat simpleDateFormat = simpleDateFormats.get(0);
            return simpleDateFormat.format(date,toAppendTo,fieldPosition);
        }

        @Override
        public Date parse(String source, ParsePosition pos) {
            int index = pos.getIndex();
            int errorIndex = pos.getErrorIndex();
            for(SimpleDateFormat simpleDateformat : simpleDateFormats){
                try {
                    pos.setIndex(index);
                    pos.setErrorIndex(errorIndex);
                    Date date = simpleDateformat.parse(source, pos);
                    if(date != null)
                        return date;
                }catch (Exception e){
                    e.printStackTrace();
                }
            }
            return new Date(0);
        }

        @Override
        public Object clone() {
            SimpleDateFormatGroup cloneObj = new SimpleDateFormatGroup();
            cloneObj.simpleDateFormats = new ArrayList<>(simpleDateFormats.size());
            for(SimpleDateFormat format : simpleDateFormats){
                cloneObj.simpleDateFormats.add((SimpleDateFormat) format.clone());
            }
            return cloneObj;
        }
    }

    public static <T> T fromJson(String content, Class<T> valueType) throws IOException {
        return mapper.readValue(content, valueType);
    }
}
