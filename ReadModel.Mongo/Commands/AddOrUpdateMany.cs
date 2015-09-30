// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrUpdateMany.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
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
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new add or update many command.</returns>
        public static AddOrUpdateManyCommandAsync<TId, TModel> AddOrUpdateManyAsync<TDatabase, TId, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            AddOrUpdateManyCommandAsync<TId, TModel> commandAsync =
                async (models, collectionName, cancellationToken) =>
                {
                    await ReplaceManyAsync(readModelDatabase, models, collectionName, cancellationToken, isUpsert: true);
                };

            return commandAsync;
        }

        /// <summary>
        /// Creates an add or update many command against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new add or update many command.</returns>
        public static AddOrUpdateManyCommandAsync<TId, TModel, TMetadata> AddOrUpdateManyAsync<TDatabase, TId, TModel, TMetadata>(
            TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            AddOrUpdateManyCommandAsync<TId, TModel, TMetadata> commandAsync =
                async (models, collectionName, cancellationToken) =>
                {
                    await ReplaceManyAsync(readModelDatabase, models, collectionName, cancellationToken, isUpsert: true);
                };

            return commandAsync;
        }
    }
}
