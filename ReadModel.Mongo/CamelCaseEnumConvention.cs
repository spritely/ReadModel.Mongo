// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CamelCaseEnumConvention.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;

    /// <summary>
    /// A convention that allows you to set the Enum serialization representation
    /// </summary>
    internal class CamelCaseEnumConvention : ConventionBase, IMemberMapConvention
    {
        /// <summary>
        /// Applies a modification to the member map.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
            Justification = "This is only exposed to trusted callers.")]
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

            public Type ValueType => baseSerializer.ValueType;

            [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
                Justification = "This is only exposed to trusted callers.")]
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
                            var enumValue = Enum.Parse(ValueType, value);
                            return enumValue;
                        }
                        return value;

                    default:
                        return baseSerializer.Deserialize(context, args);
                }
            }

            [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0",
                Justification = "This is only exposed to trusted callers.")]
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
        }
    }
}
