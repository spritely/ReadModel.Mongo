// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddMany.cs">
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

                await collection.InsertManyAsync(models, insertManyOptions, cancellationToken);
            };

            return commandAsync;
        }

        public static AddManyCommandAsync<TModel, TMetadata> AddManyAsync<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var addManyCommandAsync = AddManyAsync<TDatabase, StorageModel<TModel, TMetadata>>(readModelDatabase);

            AddManyCommandAsync<TModel, TMetadata> commandAsync =
                async (storageModels, collectionName, cancellationToken) =>
                {
                    var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                    await addManyCommandAsync(storageModels, modelTypeName, cancellationToken);
                };

            return commandAsync;
        }
    }
}
