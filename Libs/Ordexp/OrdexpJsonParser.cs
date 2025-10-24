using System.Collections.Generic;
using System.IO;
using Godot;
using Newtonsoft.Json;

namespace DeadDog.Ordexp;

public static class OrdexpJsonParser
{
    #region 数据
        public static T Parse<T>(string param, bool isFile = false) where T : class 
            => isFile ? ParseFile<T>(param) : ParseString<T>(param);

        public static T ParseString<T>(string jsonString) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (JsonException ex)
            {
                GD.PushError($"JSON解析错误：{ex.Message}");
                return null;
            }
        }

        public static T ParseFile<T>(string filePath) where T : class
        {
            var jsonString = File.ReadAllText(filePath.GetAbsolutePath());
            return ParseString<T>(jsonString);
        }

        public static dynamic ParseDynamic(string param, bool isFile = false) => 
            isFile ? ParseStringDynamic(param) : ParseFileDynamic(param);

        public static dynamic ParseStringDynamic(string jsonString) => 
            JsonConvert.DeserializeObject<dynamic>(jsonString);

        public static dynamic ParseFileDynamic(string filePath)
        {
            var jsonString = File.ReadAllText(filePath.GetAbsolutePath());
            return ParseStringDynamic(jsonString);
        }

        public static Dictionary<string, object> ParseMap(string param, bool isFile = false) => 
            isFile ? ParseStringMap(param) : ParseFileMap(param);

        public static Dictionary<string, object> ParseStringMap(string jsonString) => 
            JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

        public static Dictionary<string, object> ParseFileMap(string filePath)
        {
            var jsonString = File.ReadAllText(filePath.GetAbsolutePath());
            return ParseStringMap(jsonString);
        }
        #endregion
}