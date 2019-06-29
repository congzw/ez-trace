using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace EzTrace.Common
{
    public interface IJsonFileHelper
    {
        T Load<T>(string filePath);
        void Save<T>(T config, string filePath);

        T Load<T>();
        void Save<T>(T config);
    }

    public class JsonFileHelper : IJsonFileHelper
    {
        #region for di extensions

        private static IJsonFileHelper _resolve()
        {
            var helper = new JsonFileHelper();
            return helper;
        }

        public static Func<IJsonFileHelper> Resolve = _resolve;

        #endregion

        public T Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default(T);
            }
            var json = File.ReadAllText(filePath);
            var config = JsonConvert.DeserializeObject<T>(json);
            return config;
        }

        public void Save<T>(T config, string filePath)
        {
            var json = JsonConvert.SerializeObject(config);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }

        public T Load<T>()
        {
            var fileName = $"{typeof(T).Name}.json";
            var filePath = AppDomain.CurrentDomain.Combine(fileName);
            return Load<T>(filePath);
        }

        public void Save<T>(T config)
        {
            var fileName = $"{typeof(T).Name}.json";
            var filePath = AppDomain.CurrentDomain.Combine(fileName);
            Save(config, filePath);
        }

    }
}
