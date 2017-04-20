package com.raven.tracing.kafkaConnector;

import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.IOException;
import java.text.DateFormat;
import java.text.FieldPosition;
import java.text.ParsePosition;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

/**
 * Created by klice on 2017/4/19.
 */
public class JsonConvert {
    final static ObjectMapper mapper = new ObjectMapper();
    static {
        DateFormat format = new SimpleDateFormatGroup("yyyy-MM-dd'T'HH:mm:ss.SSSSX","yyyy-MM-dd'T'HH:mm:ss.SSSSSX","yyyy-MM-dd'T'HH:mm:ss.SSSSSSX","yyyy-MM-dd'T'HH:mm:ss.SSSSSSSX","yyyy-MM-dd'T'HH:mm:ss.SSSSSSSSX");
        mapper.setDateFormat(format);
    }

    static class SimpleDateFormatGroup extends DateFormat {

        ArrayList<SimpleDateFormat> simpleDateFormats;

        private SimpleDateFormatGroup(){

        }

        public SimpleDateFormatGroup(String...formats){
            simpleDateFormats = new ArrayList<SimpleDateFormat>(formats.length);
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
            for(SimpleDateFormat simpleDateformat : simpleDateFormats){
                try {
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
            cloneObj.simpleDateFormats = new ArrayList<SimpleDateFormat>(simpleDateFormats.size());
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
