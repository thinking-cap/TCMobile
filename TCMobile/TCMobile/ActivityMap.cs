using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TCMobile
{
    public class Activity
    {

       
        public string id { get; set; }
       
        public string mapID { get; set; }
       
        public string objectiveID { get; set; }
       
        public string Required { get; set; }
       
        public string CourseID { get; set; }
       
        public string Name { get; set; }
       
        public string Type { get; set; }
       
        public string AuthenticationType { get; set; }
       
        public string Score { get; set; }
       
        public string Status { get; set; }
       
        public string PercentCompleted { get; set; }
       
        public string IsActive { get; set; }
    }

    public class Activities
    {
        [JsonConverter(typeof(SingleValueArrayConverter<Activity>))]
        public List <Activity> Activity { get; set; }
    }

    public class SingleValueArrayConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object retVal = new Object();
            if (reader.TokenType == JsonToken.StartObject)
            {
                T instance = (T)serializer.Deserialize(reader, typeof(T));
                retVal = new List<T>() { instance };
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                retVal = serializer.Deserialize(reader, objectType);
            }
            return retVal;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }





    public class Objective
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Required { get; set; }
        public string PercentCompleted { get; set; }
        public string Score { get; set; }
        public Activities Activities { get; set; }
        public string Duration { get; set; }
    }

    public class Types
    {
        public string Type { get; set; }
    }

    public class StudentActivityMap
    {
        public List<Objective> Objective { get; set; }
        public Types Types { get; set; }
    }

    public class Map
    {
        public StudentActivityMap StudentActivityMap { get; set; }
    }
}
