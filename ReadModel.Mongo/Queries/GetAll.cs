// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAll.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Linq;
    using MongoDB.Driver;

    public static partial class Queries
    {
        /// <summary>
        /// Creates a get all query against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new get all query.</returns>
        public static GetAllQueryAsync<TModel> GetAllAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            GetAllQueryAsync<TModel> queryAsync = async (collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);

                var search = collection.Aggregate();
                var results = await search.ToListAsync(cancellationToken);

                return results;
            };

            return queryAsync;
        }

        /// <summary>
        /// Creates a get all query against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new get all query.</returns>
        public static GetAllQueryAsync<TModel, TMetadata> GetAllAsync<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var getAllQueryAsync = GetAllAsync<TDatabase, StorageModel<TModel, TMetadata>>(readModelDatabase);

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
