// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddMany.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Linq;
    using MongoDB.Driver;

    public static partial class Commands
    {
        /// <summary>
        /// Creates an add many command against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new add many command./</returns>
        public static AddManyCommandAsync<TModel> AddManyAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            AddManyCommandAsync<TModel> commandAsync = async (models, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);

                var insertManyOptions = new InsertManyOptions
                {
                    IsOrdered = true
                };

                try
                {
                    await collection.InsertManyAsync(models, insertManyOptions, cancellationToken);
                }
                catch (MongoException ex)
                {
                    throw new DatabaseException($"Unable to add {nameof(models)} of type {typeof(TModel).Name} to data store.", ex);
                }
            };

            return commandAsync;
        }

        /// <summary>
        /// Creates an add many command against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new add many command.</returns>
        public static AddManyCommandAsync<TModel, TMetadata> AddManyAsync<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var addManyCommandAsync = AddManyAsync<TDatabase, StorageModel<TModel, TMetadata>>(readModelDatabase);

            AddManyCommandAsync<TModel, TMetadata> commandAsync =
                async (storageModels, collectionName, cancellationToken) =>
                {
                    var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                    var modelsWithIds = storageModels.Select(
                        sm => new StorageModel<TModel, TMetadata>
                        {
                            Id = IdReader.ReadValue(sm.Model),
                            Model = sm.Model,
                            Metadata = sm.Metadata
                        });

                    await addManyCommandAsync(modelsWithIds, modelTypeName, cancellationToken);
                };

            return commandAsync;
        }
    }
}
