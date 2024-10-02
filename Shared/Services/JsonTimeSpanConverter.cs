using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Services
{
    // Author   : Yeasin Ahmed
    public class JsonTimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var timeSpan = Convert.ToDateTime(reader.GetString()).ToString("HH:mm");
            return TimeSpan.Parse(timeSpan);
            //throw new NotSupportedException();
        }
        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("c"));
        }
    }
}
