namespace Spritely.ReadModel.Mongo
{
    using System;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;

    /// <summary>
    /// A convention that allows you to set the Enum serialization representation
    /// </summary>
    public class CamelCaseEnumConvention : ConventionBase, IMemberMapConvention
    {
        /// <summary>
        /// Applies a modification to the member map.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        public void Apply(BsonMemberMap memberMap)
        {
            if (memberMap.MemberType.IsEnum)
            {
                var serializer = memberMap.GetSerializer();
                var camelCaseValueSerializer = new CamelCaseValueSerializer(serializer);
                memberMap.SetSerializer(camelCaseValueSerializer);
            }
        }

        private class CamelCaseValueSerializer : IBsonSerializer
        {
            private readonly IBsonSerializer baseSerializer;

            public CamelCaseValueSerializer(IBsonSerializer baseSerializer)
            {
                this.baseSerializer = baseSerializer;
            }

            public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
            {
                var bsonType = context.Reader.CurrentBsonType;
                switch (bsonType)
                {
                    case BsonType.Null:
                        context.Reader.ReadNull();
                        return null;

                    case BsonType.String:
                        var value = context.Reader.ReadString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = string.Concat(char.ToUpperInvariant(value[0]), value.Substring(1));
                            var enumValue = Enum.Parse(this.ValueType, value);
                            return enumValue;
                        }
                        return value;

                    default:
                        return this.baseSerializer.Deserialize(context, args);
                }
            }

            public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
            {
                if (value == null)
                {
                    context.Writer.WriteNull();
                }
                else
                {
                    var stringValue = value.ToString();

                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        context.Writer.WriteNull();
                    }
                    else
                    {
                        stringValue = string.Concat(char.ToLowerInvariant(stringValue[0]), stringValue.Substring(1));
                        context.Writer.WriteString(stringValue);
                    }
                }
            }

            public Type ValueType => this.baseSerializer.ValueType;
        }
    }
}