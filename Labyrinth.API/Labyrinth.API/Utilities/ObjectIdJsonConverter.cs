﻿using MongoDB.Bson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Labyrinth.API.Utilities
{
    public class ObjectIdJsonConverter : JsonConverter<ObjectId>
    {
        public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            return ObjectId.Parse(stringValue);
        }

        public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
