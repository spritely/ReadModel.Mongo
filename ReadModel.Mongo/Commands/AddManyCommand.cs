// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddManyCommand.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;

    public static partial class Create
    {
        public static AddManyCommand<TModel, TMetadata> AddManyCommand<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            AddManyCommand<TModel, TMetadata> addManyCommand = (storageModels, modelType) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(modelType) ? typeof(TModel).Name : modelType;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<StorageModel<TModel, TMetadata>>(modelTypeName);

                var task = collection.InsertManyAsync(storageModels);

                task.Wait();
            };

            return addManyCommand;
        }
    }
}
