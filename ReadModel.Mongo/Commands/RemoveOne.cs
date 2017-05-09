// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveOne.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
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
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new remove one command.</returns>
        public static RemoveOneCommandAsync<TModel> RemoveOneAsync<TModel>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            RemoveOneCommandAsync<TModel> commandAsync = async (model, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var filter = Builders<TModel>.Filter.Eq("_id", IdReader.ReadValue(model));

                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);

                await collection.DeleteOneAsync(filter, cancellationToken);
            };

            return commandAsync;
        }
    }
}
