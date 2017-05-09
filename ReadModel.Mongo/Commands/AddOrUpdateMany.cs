// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrUpdateMany.cs">
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
        /// Creates an add or update many command against the specified database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new add or update many command.</returns>
        public static AddOrUpdateManyCommandAsync<TId, TModel> AddOrUpdateManyAsync<TId, TModel>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            AddOrUpdateManyCommandAsync<TId, TModel> commandAsync =
                async (models, collectionName, cancellationToken) =>
                {
                    await ReplaceManyAsync(database, models, collectionName, cancellationToken, isUpsert: true);
                };

            return commandAsync;
        }

        /// <summary>
        /// Creates an add or update many command against the specified database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new add or update many command.</returns>
        public static AddOrUpdateManyCommandAsync<TId, TModel, TMetadata> AddOrUpdateManyAsync<TId, TModel, TMetadata>(
            MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            AddOrUpdateManyCommandAsync<TId, TModel, TMetadata> commandAsync =
                async (models, collectionName, cancellationToken) =>
                {
                    await ReplaceManyAsync(database, models, collectionName, cancellationToken, isUpsert: true);
                };

            return commandAsync;
        }
    }
}
