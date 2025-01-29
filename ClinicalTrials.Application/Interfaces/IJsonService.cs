namespace ClinicalTrials.Application.Interfaces;

public interface IJsonService
{
    T? Deserialize<T>(string json);
    string Serialize<T>(T value);
    bool IsValid(string json, string schema);
}
