// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOneCommand.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;

    public static partial class Create
    {
        public static AddOneCommand<TModel, TMetadata> AddOneCommand<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            AddOneCommand<TModel, TMetadata> addOneCommand = (model, metadata, modelType) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(modelType) ? typeof(TModel).Name : modelType;

                var storageModel = new StorageModel<TModel, TMetadata>
                {
                    Model = model,
                    Metadata = metadata
                };

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<StorageModel<TModel, TMetadata>>(modelTypeName);
                var task = collection.InsertOneAsync(storageModel);

                task.Wait();
            };

            return addOneCommand;
        }
    }
}
