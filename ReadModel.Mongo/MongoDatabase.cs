// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoDatabase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
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
    public class MongoDatabase : IGetMongoQueriesInterface, IGetMongoCommandsInterface
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
        public MongoDatabase(MongoConnectionSettings connectionSettings = null)
        {
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
            if (ConnectionSettings == null)
            {
                throw new InvalidOperationException("ConnectionSettings must be initialized.");
            }

            var client = ConnectionSettings.CreateClient();
            var database = client.GetDatabase(ConnectionSettings.Database);

            return database;
        }

        /// <inheritdoc />
        public IQueries<TModel> GetQueriesInterface<TModel>()
        {
            var queries = new Queries<TModel>(this);
            return queries;
        }

        /// <inheritdoc />
        public IMongoQueries<TModel> GetMongoQueriesInterface<TModel>()
        {
            var queries = new Queries<TModel>(this);
            return queries;
        }

        /// <inheritdoc />
        public IQueries<TModel, TMetadata> GetQueriesInterface<TModel, TMetadata>()
        {
            var queries = new Queries<TModel, TMetadata>(this);
            return queries;
        }

        /// <inheritdoc />
        public ICommands<TId, TModel> GetCommandsInterface<TId, TModel>()
        {
            var commands = new Commands<TId, TModel>(this);
            return commands;
        }

        /// <inheritdoc />
        public IMongoCommands<TId, TModel> GetMongoCommandsInterface<TId, TModel>()
        {
            var commands = new Commands<TId, TModel>(this);
            return commands;
        }

        /// <inheritdoc />
        public ICommands<TId, TModel, TMetadata> GetCommandsInterface<TId, TModel, TMetadata>()
        {
            var commands = new Commands<TId, TModel, TMetadata>(this);
            return commands;
        }
    }
}
