﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOne.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;

    public static partial class Commands
    {
        public static AddOneCommandAsync<TModel> AddOneAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            AddOneCommandAsync<TModel> commandAsync = async (model, collectionName, cancellationToken) =>
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);
                await collection.InsertOneAsync(model, cancellationToken);
            };

            return commandAsync;
        }

        public static AddOneCommandAsync<TModel, TMetadata> AddOneAsync<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var addOneCommandAsync = AddOneAsync<TDatabase, StorageModel<TModel, TMetadata>>(readModelDatabase);

            AddOneCommandAsync<TModel, TMetadata> commandAsync = async (model, metadata, collectionName, cancellationToken) =>
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var storageModel = new StorageModel<TModel, TMetadata>
                {
                    Model = model,
                    Metadata = metadata
                };

                await addOneCommandAsync(storageModel, modelTypeName, cancellationToken);
            };

            return commandAsync;
        }
    }
}