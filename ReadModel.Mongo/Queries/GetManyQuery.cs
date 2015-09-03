// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetManyQuery.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Linq;
    using MongoDB.Driver;

    public static partial class Create
    {
        public static GetManyQueryAsync<TModel> GetManyQueryAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            GetManyQueryAsync<TModel> getManyQueryAsync = async (where, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);

                var search = collection.Aggregate().Match(where);
                var results = await search.ToListAsync(cancellationToken);

                return results;
            };

            return getManyQueryAsync;
        }

        public static GetManyQueryAsync<TModel, TMetadata> GetManyQueryAsync<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var getManyModelsQueryAsync = GetManyQueryAsync<TDatabase, StorageModel<TModel, TMetadata>>(readModelDatabase);

            GetManyQueryAsync<TModel, TMetadata> getManyQueryAsync = async (where, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var results = await getManyModelsQueryAsync(where, modelTypeName, cancellationToken);

                return results.Select(sm => sm.Model);
            };

            return getManyQueryAsync;
        }
    }
}
