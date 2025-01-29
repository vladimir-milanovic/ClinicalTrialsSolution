using ClinicalTrials.Application.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace ClinicalTrials.Application.Services;

public class JsonService : IJsonService
{
    public T? Deserialize<T>(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception e)
        {
            return default;
        }
    }

    public string Serialize<T>(T value)
    {
        return JsonConvert.SerializeObject(value);
    }

    public bool IsValid(string jsonString, string schema)
    {
        try
        {
            using var stringReader = new StringReader(jsonString);
            using var jsonTextReader = new JsonTextReader(stringReader);
            using var validatingReader = new JSchemaValidatingReader(jsonTextReader);

            validatingReader.Schema = JSchema.Parse(schema);

            var serializer = new JsonSerializer();
            serializer.Deserialize<object>(validatingReader);

            return true;
        }
        catch (JsonReaderException e)
        {
            return false;
        }
        catch (JSchemaValidationException e)
        {
            return false;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
