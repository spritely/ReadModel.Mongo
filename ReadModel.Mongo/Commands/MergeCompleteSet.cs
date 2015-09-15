// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeCompleteSet.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Driver;

    public static partial class Commands
    {
        /// <summary>
        /// Creates a merge complete set command against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new merge complete set command.</returns>
        public static MergeCompleteSetCommandAsync<TId, TModel> MergeCompleteSetAsync<TDatabase, TId, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            // remove from set b where not in set a
            var addOrUpdateOneCommandAsync = AddOrUpdateOneAsync<TDatabase, TModel>(readModelDatabase);

            MergeCompleteSetCommandAsync<TId, TModel> commandAsync = async (models, collectionName, cancellationToken) =>
            {
                if (models == null)
                {
                    throw new ArgumentNullException(nameof(models));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                // Remove all existing models with ids not in the list of ids given
                var equalFilter = Builders<TModel>.Filter.In("_id", models.Select(kvp => IdReader.ReadValue(kvp.Value)));
                var notEqualFilter = Builders<TModel>.Filter.Not(equalFilter);

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);
                var removeTask = collection.DeleteManyAsync(notEqualFilter, cancellationToken);

                // Add or update the rest
                var addOrUpdateTasks = models.Values.Select(model => addOrUpdateOneCommandAsync(model, modelTypeName, cancellationToken));

                var allTasks = new List<Task> { removeTask };
                allTasks.AddRange(addOrUpdateTasks);

                await Task.WhenAll(allTasks);
            };

            return commandAsync;
        }

        /// <summary>
        /// Creates a merge complete set command against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new merge complete set command.</returns>
        public static MergeCompleteSetCommandAsync<TId, TModel, TMetadata> MergeCompleteSetAsync<TDatabase, TId, TModel, TMetadata>(
            TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var addOrUpdateOneCommandAsync = AddOrUpdateOneAsync<TDatabase, TModel, TMetadata>(readModelDatabase);

            MergeCompleteSetCommandAsync<TId, TModel, TMetadata> commandAsync = async (models, collectionName, cancellationToken) =>
            {
                if (models == null)
                {
                    throw new ArgumentNullException(nameof(models));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                // Remove all existing models with ids not in the list of ids given
                var equalFilter = Builders<StorageModel<TModel, TMetadata>>.Filter.In(
                    "_id",
                    models.Select(kvp => IdReader.ReadValue(kvp.Value.Model)));
                var notEqualFilter = Builders<StorageModel<TModel, TMetadata>>.Filter.Not(equalFilter);

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<StorageModel<TModel, TMetadata>>(modelTypeName);
                var removeTask = collection.DeleteManyAsync(notEqualFilter, cancellationToken);

                // Add or update the rest
                var addOrUpdateTasks =
                    models.Values.Select(sm => addOrUpdateOneCommandAsync(sm.Model, sm.Metadata, modelTypeName, cancellationToken));

                var allTasks = new List<Task> { removeTask };
                allTasks.AddRange(addOrUpdateTasks);

                await Task.WhenAll(allTasks);
            };

            return commandAsync;
        }
    }
}
