// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateOne.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using MongoDB.Driver;
    using Spritely.Cqrs;

    public static partial class Commands
    {
        public static UpdateOneCommandAsync<TModel> UpdateOneAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            UpdateOneCommandAsync<TModel> commandAsync = async (model, collectionName, cancellationToken) =>
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var idReader = new IdReader<TModel>();

                var updateOptions = new UpdateOptions
                {
                    IsUpsert = false
                };

                var filter = Builders<TModel>.Filter.Eq("_id", idReader.Read(model));
                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);

                var result = await collection.ReplaceOneAsync(filter, model, updateOptions, cancellationToken);

                if (result.ModifiedCount == 0)
                {
                    throw new DatabaseException(
                        $"Unable to find {nameof(model)} of type {typeof(TModel).Name} with id '{idReader.Read(model)}' in data store to update.");
                }
            };

            return commandAsync;
        }

        public static UpdateOneCommandAsync<TModel, TMetadata> UpdateOneAsync<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            UpdateOneCommandAsync<TModel, TMetadata> commandAsync = async (model, metadata, collectionName, cancellationToken) =>
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var idReader = new IdReader<TModel>();

                var storageModel = new StorageModel<TModel, TMetadata>
                {
                    Id = idReader.Read(model),
                    Model = model,
                    Metadata = metadata
                };

                var updateOptions = new UpdateOptions
                {
                    IsUpsert = false
                };

                var filter = Builders<StorageModel<TModel, TMetadata>>.Filter.Eq("model._id", idReader.Read(model));
                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<StorageModel<TModel, TMetadata>>(modelTypeName);

                var result = await collection.ReplaceOneAsync(filter, storageModel, updateOptions, cancellationToken);

                if (result.ModifiedCount == 0)
                {
                    throw new DatabaseException(
                        $"Unable to find {nameof(model)} of type {typeof(TModel).Name} with id '{idReader.Read(model)}' in data store to update.");
                }
            };

            return commandAsync;
        }
    }
}
