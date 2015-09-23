// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetMany.cs">
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
        /// Creates a get many query against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new get many query.</returns>
        public static GetManyQueryAsync<TModel> GetManyAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            GetManyQueryAsync<TModel> queryAsync = async (where, collectionName, cancellationToken) =>
            {
                if (where == null)
                {
                    throw new ArgumentNullException((nameof(where)));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);

                return await collection.Find(where).ToListAsync(cancellationToken);
            };

            return queryAsync;
        }

        /// <summary>
        /// Creates a get many query against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new get many query.</returns>
        public static GetManyQueryAsync<TModel, TMetadata> GetManyAsync<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var getManyQueryAsync = GetManyAsync<TDatabase, StorageModel<TModel, TMetadata>>(readModelDatabase);

            GetManyQueryAsync<TModel, TMetadata> queryAsync = async (where, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var results = await getManyQueryAsync(where, modelTypeName, cancellationToken);

                return results.Select(sm => sm.Model);
            };

            return queryAsync;
        }
    }
}
