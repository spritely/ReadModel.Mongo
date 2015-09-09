// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoConnectionSettings.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Spritely.Cqrs;

    /// <summary>
    /// Configuration object for initializing Mongo database connections.
    /// </summary>
    public class MongoConnectionSettings
    {
        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        /// <value>The credentials.</value>
        public Credentials Credentials { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the server port.
        /// </summary>
        /// <value>The server port.</value>
        public int? Port { get; set; }

        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        /// <value>The server name.</value>
        public string Server { get; set; }

        /// <summary>
        /// Creates a mongo client from the settings contained in this instance.
        /// </summary>
        /// <returns>A new mongo client.</returns>
        public MongoClient CreateClient()
        {
            var clientSettings = new MongoClientSettings
            {
                GuidRepresentation = GuidRepresentation.Standard
            };

            if (!string.IsNullOrWhiteSpace(Database) &&
                !string.IsNullOrWhiteSpace(Credentials?.User))
            {
                var credential = MongoCredential.CreateMongoCRCredential(
                    Database,
                    Credentials.User,
                    Credentials.Password);

                clientSettings.Credentials = new[] { credential };
            }

            var server = (string.IsNullOrWhiteSpace(Server)) ? "localhost" : Server;
            var port = Port ?? 27017;
            clientSettings.Server = new MongoServerAddress(server, port);

            var client = new MongoClient(clientSettings);

            return client;
        }
    }
}
