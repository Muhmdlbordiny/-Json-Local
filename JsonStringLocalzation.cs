using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace json_based_localization
{
    public class JsonStringLocalzation : IStringLocalizer
    {
        private readonly JsonSerializer _serializer=new();
        private readonly IDistributedCache _cache; //inject Idistributedcache

        public JsonStringLocalzation(IDistributedCache cache)
        {
            _cache = cache;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var actualvalue = this[name];
                return !actualvalue.ResourceNotFound
                    ? new LocalizedString(name, string.Format(actualvalue, arguments))
                    : actualvalue;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var filepath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
            using FileStream stream = new(filepath, FileMode.Open, FileAccess.Read,FileShare.Read);
            using StreamReader streamReader = new(stream);
            using JsonTextReader reader = new(streamReader);
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                    continue;
                var Key = reader.Value as string;
                reader.Read();
                var value = _serializer.Deserialize<string>(reader);
                yield return new LocalizedString(Key, value);
            }

        }
        private string GetvalueFromJson(string propertyName ,string filepath) // known file json &PropertyName =>Key
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filepath))
                return string.Empty;
            using FileStream stream = new (filepath,FileMode.Open,FileAccess.Read);
            using StreamReader streamReader = new(stream);
            using JsonTextReader reader = new(streamReader);
            while (reader.Read())
            {
                if(reader.TokenType == JsonToken.PropertyName && reader.Value as string == propertyName)
                {
                    reader.Read();
                    return _serializer.Deserialize<string>(reader); 
                }
            }

            return string.Empty;
        }
        private string GetString(string Key)
        {
            // resources /ar-Eg.json
            // resources /En-US.json
            var filepath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
            var fullfilepath = Path.GetFullPath(filepath);


            if(File.Exists(fullfilepath))
            {
                var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{Key}";
                var cachevalue = _cache.GetString(cacheKey);


                if (!string.IsNullOrEmpty(cachevalue))
                    return cachevalue;


                var result = GetvalueFromJson(Key,fullfilepath);

                if (!string.IsNullOrEmpty(result))
                    _cache.SetString(cacheKey, result);
               
                return result;

            }
            return string.Empty;
        }
    }
}
