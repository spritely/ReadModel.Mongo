// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateOne.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using MongoDB.Driver;

    public static partial class Commands
    {
        /// <summary>
        /// Creates an update one command against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new update one command.</returns>
        public static UpdateOneCommandAsync<TModel> UpdateOneAsync<TModel>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            UpdateOneCommandAsync<TModel> commandAsync = async (model, collectionName, cancellationToken) =>
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var updateOptions = new UpdateOptions
                {
                    IsUpsert = false
                };

                var id = IdReader.ReadValue(model);
                var filter = Builders<TModel>.Filter.Eq("_id", id);
                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);

                var result = await collection.ReplaceOneAsync(filter, model, updateOptions, cancellationToken);

                if (result.ModifiedCount == 0)
                {
                    throw new DatabaseException(
                        $"Unable to find {nameof(model)} of type {typeof(TModel).Name} with id '{id}' in data store to update.");
                }
            };

            return commandAsync;
        }

        /// <summary>
        /// Creates an update one command against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new update one command.</returns>
        public static UpdateOneCommandAsync<TModel, TMetadata> UpdateOneAsync<TModel, TMetadata>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            UpdateOneCommandAsync<TModel, TMetadata> commandAsync = async (model, metadata, collectionName, cancellationToken) =>
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var id = IdReader.ReadValue(model);
                var storageModel = new StorageModel<TModel, TMetadata>
                {
                    Id = id,
                    Model = model,
                    Metadata = metadata
                };

                var updateOptions = new UpdateOptions
                {
                    IsUpsert = false
                };

                var filter = Builders<StorageModel<TModel, TMetadata>>.Filter.Eq("model._id", id);
                var client = database.CreateClient();
                var collection = client.GetCollection<StorageModel<TModel, TMetadata>>(modelTypeName);

                var result = await collection.ReplaceOneAsync(filter, storageModel, updateOptions, cancellationToken);

                if (result.ModifiedCount == 0)
                {
                    throw new DatabaseException(
                        $"Unable to find {nameof(model)} of type {typeof(TModel).Name} with id '{id}' in data store to update.");
                }
            };

            return commandAsync;
        }
    }
}
