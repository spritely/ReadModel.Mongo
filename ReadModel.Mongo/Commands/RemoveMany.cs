// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveMany.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;

    public static partial class Commands
    {
        /// <summary>
        /// Creates a remove many command against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new remove many command.</returns>
        public static RemoveManyCommandAsync<TModel> RemoveManyAsync<TModel>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            RemoveManyCommandAsync<TModel> commandAsync = async (where, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);

                await collection.DeleteManyAsync(where, cancellationToken);
            };

            return commandAsync;
        }

        /// <summary>
        /// Creates a remove many command against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new remove many command.</returns>
        public static RemoveManyCommandUsingFilterDefinitionAsync<TModel> RemoveManyUsingFilterDefinitionAsync<TModel>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            RemoveManyCommandUsingFilterDefinitionAsync<TModel> commandAsync = async (filterDefinition, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);

                await collection.DeleteManyAsync(filterDefinition, cancellationToken);
            };

            return commandAsync;
        }

        /// <summary>
        /// Creates a remove many command against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new remove many command.</returns>
        public static RemoveManyCommandAsync<TModel, TMetadata> RemoveManyAsync<TModel, TMetadata>(MongoDatabase database)
        {
            var removeManyCommandAsync = RemoveManyAsync<StorageModel<TModel, TMetadata>>(database);

            RemoveManyCommandAsync<TModel, TMetadata> commandAsync =
                async (where, collectionName, cancellationToken) =>
                {
                    var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                    await removeManyCommandAsync(where, modelTypeName, cancellationToken);
                };

            return commandAsync;
        }
    }
}
