using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Config.Base;
using CreativeCoders.Config.Base.Exceptions;
using CreativeCoders.Core;
using CreativeCoders.Core.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace CreativeCoders.Config.Sources.Json
{
    [PublicAPI]
    public class JsonConfigurationSource<T> : IConfigurationSource<T>
        where T : class, new()
    {
        private readonly string _jsonFileName;

        private readonly Func<T> _getDefaultSettingObjectFunc;

        public JsonConfigurationSource(string jsonFileName) : this(jsonFileName, () => new T()) { }

        public JsonConfigurationSource(string jsonFileName, Func<T> getDefaultSettingObjectFunc)
        {
            Ensure.IsNotNullOrWhitespace(jsonFileName, nameof(jsonFileName));
            Ensure.IsNotNull(getDefaultSettingObjectFunc, nameof(getDefaultSettingObjectFunc));

            _jsonFileName = jsonFileName;
            _getDefaultSettingObjectFunc = getDefaultSettingObjectFunc;
        }

        private T LoadSettingFromFile(string jsonFileName)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(FileSys.File.ReadAllText(jsonFileName));
            }
            catch (Exception ex)
            {
                throw new ConfigurationFileSourceException(jsonFileName, this, "", ex);
            }
        }

        public static IEnumerable<JsonConfigurationSource<T>> FromFiles(IEnumerable<string> jsonFileNames)
        {
            Ensure.IsNotNull(jsonFileNames, nameof(jsonFileNames));

            return jsonFileNames.Select(fileName => new JsonConfigurationSource<T>(fileName)).ToArray();
        }

        public static IEnumerable<JsonConfigurationSource<T>> FromFiles(IEnumerable<string> jsonFileNames,
            Func<T> getDefaultSettingFunc)
        {
            Ensure.IsNotNull(jsonFileNames, nameof(jsonFileNames));

            return jsonFileNames.Select(fileName => new JsonConfigurationSource<T>(fileName, getDefaultSettingFunc))
                .ToArray();
        }
        
        public virtual object GetSettingObject()
        {
            return LoadSettingFromFile(_jsonFileName);
        }

        public virtual object GetDefaultSettingObject()
        {
            return _getDefaultSettingObjectFunc();
        }
    }
}