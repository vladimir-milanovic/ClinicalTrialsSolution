using System.Reflection;
using System.Text;

namespace ClinicalTrials.Api.Extensions;

public static class AssemblyExtensions
{
    public static string GetJsonSchemaContent(this Assembly assembly)
    {
        var resourceStream = assembly.GetManifestResourceStream("ClinicalTrials.Api.Resources.schema.json");
        
        if (resourceStream is null)
            return string.Empty;

        using var reader = new StreamReader(resourceStream, Encoding.UTF8);
        
        return reader.ReadToEnd();
    }
}
