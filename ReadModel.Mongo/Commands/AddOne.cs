// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOne.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
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
        /// Creates an add one command against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new add one command.</returns>
        public static AddOneCommandAsync<TModel> AddOneAsync<TModel>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            AddOneCommandAsync<TModel> commandAsync = async (model, collectionName, cancellationToken) =>
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);

                try
                {
                    await collection.InsertOneAsync(model, cancellationToken);
                }
                catch (MongoException ex)
                {
                    throw new DatabaseException($"Unable to add {nameof(model)} of type {typeof(TModel).Name} to data store.", ex);
                }
            };

            return commandAsync;
        }

        /// <summary>
        /// Creates an add one command against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new add one command.</returns>
        public static AddOneCommandAsync<TModel, TMetadata> AddOneAsync<TModel, TMetadata>(MongoDatabase database)
        {
            var addOneCommandAsync = AddOneAsync<StorageModel<TModel, TMetadata>>(database);

            AddOneCommandAsync<TModel, TMetadata> commandAsync = async (model, metadata, collectionName, cancellationToken) =>
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var storageModel = new StorageModel<TModel, TMetadata>
                {
                    Id = IdReader.ReadValue(model),
                    Model = model,
                    Metadata = metadata
                };

                await addOneCommandAsync(storageModel, modelTypeName, cancellationToken);
            };

            return commandAsync;
        }
    }
}
