// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddManyCommand.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using MongoDB.Driver;

    public static partial class Create
    {
        public static AddManyCommandAsync<TModel> AddManyCommandAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            AddManyCommandAsync<TModel> addManyCommandAsync = async (models, collectionName, cancellationToken) =>
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

            return addManyCommandAsync;
        }

        public static AddManyCommandAsync<TModel, TMetadata> AddManyCommandAsync<TDatabase, TModel, TMetadata>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var addManyModelsCommandAsync = AddManyCommandAsync<TDatabase, StorageModel<TModel, TMetadata>>(readModelDatabase);

            AddManyCommandAsync<TModel, TMetadata> addManyCommandAsync =
                async (storageModels, collectionName, cancellationToken) =>
                {
                    var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                    await addManyModelsCommandAsync(storageModels, modelTypeName, cancellationToken);
                };

            return addManyCommandAsync;
        }
    }
}
