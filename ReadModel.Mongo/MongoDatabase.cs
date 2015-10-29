// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoDatabase.cs">
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

    /// <summary>
    /// Class representing a read model database. Commands and queries accessing the read model
    /// require this.
    /// </summary>
    public class MongoDatabase
    {
        private MongoConnectionSettings connectionSettings;

        /// <summary>
        /// Initializes the <see cref="MongoDatabase" /> class.
        /// </summary>
        static MongoDatabase()
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
        /// Initializes a new instance of the <see cref="MongoDatabase" /> class.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <exception cref="System.ArgumentNullException">If connectionSettings is null.</exception>
        public MongoDatabase(MongoConnectionSettings connectionSettings)
        {
            if (connectionSettings == null)
            {
                throw new ArgumentNullException(nameof(connectionSettings));
            }

            this.connectionSettings = connectionSettings;
        }

        /// <summary>
        /// Gets or sets the connection settings.
        /// </summary>
        /// <value>The connection settings.</value>
        public MongoConnectionSettings ConnectionSettings
        {
            get { return connectionSettings; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                connectionSettings = value;
            }
        }

        /// <summary>
        /// Creates a database connection.
        /// </summary>
        /// <returns>A new database connection.</returns>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ConnectionSettings",
            Justification = "Message refers to a class member.")]
        public virtual IMongoDatabase CreateClient()
        {
            var client = ConnectionSettings.CreateClient();
            var database = client.GetDatabase(ConnectionSettings.Database);

            return database;
        }

        /// <summary>
        /// Gets an interface that only allows querying a specific model for this database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        public IQueries<TModel> GetQueriesInterface<TModel>()
        {
            var queries = new Queries<TModel>(this);
            return queries;
        }

        /// <summary>
        /// Gets an interface that only allows querying a specific model for this database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        public IQueries<TModel, TMetadata> GetQueriesInterface<TModel, TMetadata>()
        {
            var queries = new Queries<TModel, TMetadata>(this);
            return queries;
        }

        /// <summary>
        /// Gets an interface that only allows executing commands against a specific model for this database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        public ICommands<TId, TModel> GetCommandsInterface<TId, TModel>()
        {
            var commands = new Commands<TId, TModel>(this);
            return commands;
        }

        /// <summary>
        /// Gets an interface that only allows executing commands against a specific model for this database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        public ICommands<TId, TModel, TMetadata> GetCommandsInterface<TId, TModel, TMetadata>()
        {
            var commands = new Commands<TId, TModel, TMetadata>(this);
            return commands;
        }
    }
}
