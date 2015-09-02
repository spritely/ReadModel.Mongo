// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddManyCommand.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Driver;

    public static partial class Create
    {
        public static GetManyQuery<TModel, TMetadata> GetManyQuery<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            GetManyQuery<TModel, TMetadata> getManyQuery = (where, modelType) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(modelType) ? typeof(TModel).Name : modelType;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<StorageModel<TModel, TMetadata>>(modelTypeName);

                var search = collection.Aggregate().Match(where);
                var task = Task.Run(() => search.ToListAsync());
                task.Wait();

                return task.Result.Select(sm => sm.Model).ToList();
            };

            return getManyQuery;
        }
    }
}
