using System;
using Functions.Interfaces;
using Newtonsoft.Json;

namespace Functions.Services
{
    public class JsonConverterWrapper : IJsonConverterWrapper
    {
        public T DeserializeObject<T>(string value)
        {
            if(string.IsNullOrEmpty(value))
                throw new InvalidOperationException("value is null or empty");
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}