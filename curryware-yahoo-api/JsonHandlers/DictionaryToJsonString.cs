using Newtonsoft.Json;

namespace curryware_yahoo_api.JsonHandlers;

public class DictionaryJsonHandler
{
    public string DictionaryToJsonString<T> (List<T> item)
    {
        var jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None
        };
        
        return JsonConvert.SerializeObject (item, jsonSettings);
    }
}