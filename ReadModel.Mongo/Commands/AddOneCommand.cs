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
        public static AddOneCommandAsync<TModel> AddOneCommandAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            AddOneCommandAsync<TModel> addOneCommandAsync = async (model, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);
                await collection.InsertOneAsync(model, cancellationToken);
            };

            return addOneCommandAsync;
        }

        public static AddOneCommandAsync<TModel, TMetadata> AddOneCommandAsync<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var addOneModelCommandAsync = AddOneCommandAsync<TDatabase, StorageModel<TModel, TMetadata>>(readModelDatabase);

            AddOneCommandAsync<TModel, TMetadata> addOneCommandAsync = async (model, metadata, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var storageModel = new StorageModel<TModel, TMetadata>
                {
                    Model = model,
                    Metadata = metadata
                };

                await addOneModelCommandAsync(storageModel, modelTypeName, cancellationToken);
            };

            return addOneCommandAsync;
        }
    }
}
