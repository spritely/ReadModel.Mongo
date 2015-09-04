﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveMany.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;

    public static partial class Commands
    {
        public static RemoveManyCommandAsync<TModel> RemoveManyAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            RemoveManyCommandAsync<TModel> commandAsync = async (where, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);

                await collection.DeleteManyAsync(where, cancellationToken);
            };

            return commandAsync;
        }

        public static RemoveManyCommandAsync<TModel, TMetadata> RemoveManyAsync<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var removeManyCommandAsync = RemoveManyAsync<TDatabase, StorageModel<TModel, TMetadata>>(readModelDatabase);

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