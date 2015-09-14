// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveOne.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using MongoDB.Driver;

    public static partial class Commands
    {
        /// <summary>
        /// Creates a remove one command against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new remove one command.</returns>
        public static RemoveOneCommandAsync<TModel> RemoveOneAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            RemoveOneCommandAsync<TModel> commandAsync = async (model, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var filter = Builders<TModel>.Filter.Eq("_id", IdReader.ReadValue(model));

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);

                await collection.DeleteOneAsync(filter, cancellationToken);
            };

            return commandAsync;
        }
    }
}
