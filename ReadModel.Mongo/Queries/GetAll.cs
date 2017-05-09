// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAll.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Linq;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public static partial class Queries
    {
        /// <summary>
        /// Creates a get all query against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new get all query.</returns>
        public static GetAllQueryAsync<TModel> GetAllAsync<TModel>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            GetAllQueryAsync<TModel> queryAsync = async (collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);

                var filter = new BsonDocument();
                return await collection.Find(filter).ToListAsync(cancellationToken);
            };

            return queryAsync;
        }

        /// <summary>
        /// Creates a get all query against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new get all query.</returns>
        public static GetAllQueryAsync<TModel, TMetadata> GetAllAsync<TModel, TMetadata>(MongoDatabase database)
        {
            var getAllQueryAsync = GetAllAsync<StorageModel<TModel, TMetadata>>(database);

            GetAllQueryAsync<TModel, TMetadata> queryAsync = async (collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var results = await getAllQueryAsync(modelTypeName, cancellationToken);

                return results.Select(sm => sm.Model);
            };

            return queryAsync;
        }
    }
}
