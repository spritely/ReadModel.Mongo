// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeCompleteSet.cs">
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
        /// Creates a merge complete set command against the specified database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new merge complete set command.</returns>
        public static MergeCompleteSetCommandAsync<TId, TModel> MergeCompleteSetAsync<TId, TModel>(MongoDatabase database)
        {
            // remove from set b where not in set a
            var addOrUpdateManyCommandAsync = AddOrUpdateManyAsync<TId, TModel>(database);

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

                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);
                await collection.DeleteManyAsync(notEqualFilter, cancellationToken);

                // Add or update the rest
                await addOrUpdateManyCommandAsync(models, modelTypeName, cancellationToken);
            };

            return commandAsync;
        }

        /// <summary>
        /// Creates a merge complete set command against the specified database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new merge complete set command.</returns>
        public static MergeCompleteSetCommandAsync<TId, TModel, TMetadata> MergeCompleteSetAsync<TId, TModel, TMetadata>(
            MongoDatabase database)
        {
            // remove from set b where not in set a
            var addOrUpdateManyCommandAsync = AddOrUpdateManyAsync<TId, TModel, TMetadata>(database);

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

                var client = database.CreateClient();
                var collection = client.GetCollection<StorageModel<TModel, TMetadata>>(modelTypeName);
                await collection.DeleteManyAsync(notEqualFilter, cancellationToken);

                // Add or update the rest
                await addOrUpdateManyCommandAsync(models, modelTypeName, cancellationToken);
            };

            return commandAsync;
        }
    }
}
