// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetOne.cs">
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
        /// Creates a get one query against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new get one query.</returns>
        public static GetOneQueryAsync<TModel> GetOneAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            GetOneQueryAsync<TModel> queryAsync = async (where, collectionName, cancellationToken) =>
            {
                if (where == null)
                {
                    throw new ArgumentNullException(nameof(where));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);

                var results = await collection.Find(where).ToListAsync(cancellationToken);
                return results.SingleOrDefault();
            };

            return queryAsync;
        }

        /// <summary>
        /// Creates a get one query against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new get one query.</returns>
        public static GetOneQueryAsync<TModel, TMetadata> GetOneAsync<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var getOneQueryAsync = GetOneAsync<TDatabase, StorageModel<TModel, TMetadata>>(readModelDatabase);

            GetOneQueryAsync<TModel, TMetadata> queryAsync = async (where, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var result = await getOneQueryAsync(where, modelTypeName, cancellationToken);

                return result == null ? default(TModel) : result.Model;
            };

            return queryAsync;
        }
    }
}
