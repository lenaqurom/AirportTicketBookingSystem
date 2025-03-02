using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using AirportTicketBookingSystem.Models;

public class ClassTypeConverter : JsonConverter<ClassType>
{
    public override ClassType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var classTypeString = reader.GetString();

        if (Enum.TryParse<ClassType>(classTypeString, true, out var classType))
        {
            return classType;
        }

        throw new JsonException($"Unable to convert \"{classTypeString}\" to ClassType.");
    }

    public override void Write(Utf8JsonWriter writer, ClassType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
