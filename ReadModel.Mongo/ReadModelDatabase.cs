// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadModelDatabase.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System.Reflection;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Conventions;
    using MongoDB.Driver;
    using Spritely.Cqrs;

    /// <summary>
    /// Class representing a read model database. Commands and queries accessing the read model
    /// require this.
    /// </summary>
    public class ReadModelDatabase<T> : IDatabase where T : ReadModelDatabase<T>
    {
        static ReadModelDatabase()
        {
            var pack = new ConventionPack
            {
                new NamedIdMemberConvention(
                    new[] { "Id", "id", "_id" },
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic),
                new IgnoreExtraElementsConvention(true),
                new CamelCaseEnumConvention(),
                new CamelCaseElementNameConvention()
            };

            ConventionRegistry.Register(
                "Spritely.ReadModel.Mongo Conventions",
                pack,
                t => t.FullName.StartsWith("Spritely.ReadModel.Mongo"));
        }

        /// <summary>
        /// Gets or sets the connection settings.
        /// </summary>
        /// <value>The connection settings.</value>
        public MongoConnectionSettings ConnectionSettings { get; set; }

        /// <summary>
        /// Creates a database connection.
        /// </summary>
        /// <returns>A new database connection.</returns>
        public virtual IMongoDatabase CreateConnection()
        {
            var client = new MongoClient(new MongoClientSettings
            {
                GuidRepresentation = GuidRepresentation.Standard,
            });

            var database = client.GetDatabase(this.ConnectionSettings.Database);

            return database;
        }
    }
}
