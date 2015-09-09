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
    using Spritely.Cqrs;

    public static partial class Queries
    {
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

                var search = collection.Aggregate().Match(where);
                var results = await search.ToListAsync(cancellationToken);

                return results;
            };

            return queryAsync;
        }

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
