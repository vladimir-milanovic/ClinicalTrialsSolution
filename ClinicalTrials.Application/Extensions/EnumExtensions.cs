using System.Reflection;
using System.Runtime.Serialization;

namespace ClinicalTrials.Application.Extensions;

public static class EnumExtensions
{
    public static string ToEnumMemberAttributeValue(this Enum value)
    {
        var enumType = value.GetType();
        var enumMemberAttribute = enumType
            .GetTypeInfo()
            .DeclaredMembers
            .Single(x => x.Name == value.ToString())
            .GetCustomAttribute<EnumMemberAttribute>(false);

        if (enumMemberAttribute == null)
        {
            throw new NotSupportedException($"Enum: '{enumType.FullName}', value: {value} does not have attribute: '{nameof(EnumMemberAttribute)}'.");
        }

        return enumMemberAttribute?.Value ?? string.Empty;
    }
}
