// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadModelDatabase.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using MongoDB.Bson.Serialization.Conventions;
    using MongoDB.Driver;
    using static System.FormattableString;

    /// <summary>
    /// Class representing a read model database. Commands and queries accessing the read model
    /// require this.
    /// </summary>
    public class ReadModelDatabase<T> : IDatabase where T : ReadModelDatabase<T>
    {
        /// <summary>
        /// Initializes the <see cref="ReadModelDatabase{T}"/> class.
        /// </summary>
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
                t => t.FullName.StartsWith("Spritely.ReadModel", StringComparison.OrdinalIgnoreCase));
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
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ConnectionSettings",
            Justification = "Message refers to a class member.")]
        public virtual IMongoDatabase CreateConnection()
        {
            if (ConnectionSettings == null)
            {
                throw new InvalidOperationException(
                    Invariant($"Cannot create a connection when {nameof(ConnectionSettings)} is null"));
            }

            var client = ConnectionSettings.CreateClient();
            var database = client.GetDatabase(ConnectionSettings.Database);

            return database;
        }
    }
}
