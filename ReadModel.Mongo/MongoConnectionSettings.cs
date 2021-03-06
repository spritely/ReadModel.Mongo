﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoConnectionSettings.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;

    using MongoDB.Bson;
    using MongoDB.Driver;

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
        /// Gets or sets a value indicating whether writes are acknowledged.
        /// </summary>
        /// <value>
        ///   <c>true</c> if writes are acknowledged; otherwise, <c>false</c>.
        /// </value>
        public bool AcknowledgeWrites { get; set; }

        /// <summary>
        /// Gets or sets the connection timeout in seconds.
        /// </summary>
        /// <value>The connection timeout in seconds.</value>
        public int? ConnectionTimeoutInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the default command timeout in seconds.
        /// </summary>
        /// <value>The default command timeout in seconds.</value>
        public int? DefaultCommandTimeoutInSeconds { get; set; }

        /// <summary>
        /// Creates a mongo client from the settings contained in this instance.
        /// </summary>
        /// <returns>A new mongo client.</returns>
        public MongoClient CreateClient()
        {
            var clientSettings = new MongoClientSettings
            {
                GuidRepresentation = GuidRepresentation.Standard,
            };

            if (ConnectionTimeoutInSeconds != null)
            {
                clientSettings.ConnectTimeout = TimeSpan.FromSeconds(ConnectionTimeoutInSeconds.Value);
            }

            if (DefaultCommandTimeoutInSeconds != null)
            {
                clientSettings.SocketTimeout = TimeSpan.FromSeconds(DefaultCommandTimeoutInSeconds.Value);
            }

            if (!string.IsNullOrWhiteSpace(Database) &&
                !string.IsNullOrWhiteSpace(Credentials?.User))
            {
                var credential = MongoCredential.CreateCredential(
                    Database,
                    Credentials.User,
                    Credentials.Password);

                clientSettings.Credentials = new[] { credential };

                clientSettings.WriteConcern = new WriteConcern(journal: AcknowledgeWrites);
            }

            var server = (string.IsNullOrWhiteSpace(Server)) ? "localhost" : Server;
            var port = Port ?? 27017;
            clientSettings.Server = new MongoServerAddress(server, port);

            var client = new MongoClient(clientSettings);

            return client;
        }
    }
}
