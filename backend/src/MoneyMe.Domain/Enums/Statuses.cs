
using System.Text.Json.Serialization;

namespace MoneyMe.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Statuses
{
    Transaction_Success
}

