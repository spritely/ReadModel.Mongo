// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoConnectionSettings.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    /// <summary>
    /// Configuration object for initializing Mongo database connections.
    /// </summary>
    public class MongoConnectionSettings
    {
        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        public string Database { get; set; }
    }
}
