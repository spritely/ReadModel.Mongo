// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Common.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public static partial class Commands
    {
        /// <summary>
        /// Executes a query to replace many models (for Update or AddOrUpdate many).
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <param name="models">The set of models to write.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isUpsert">
        /// If set to <c>true</c> models will be defined as upserts (AddOrUpdate); otherwise they
        /// will be defined as updates.
        /// </param>
        /// <returns>The bulk write task results.</returns>
        /// <exception cref="System.ArgumentNullException">If models is null.</exception>
        private static async Task<BulkWriteResult<BsonDocument>> ReplaceManyAsync<TDatabase, TId, TModel>(
            TDatabase readModelDatabase,
            IDictionary<TId, TModel> models,
            string collectionName,
            CancellationToken cancellationToken,
            bool isUpsert) where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

            var database = readModelDatabase.CreateConnection();
            var collection = database.GetCollection<BsonDocument>(modelTypeName);

            var updateModels = new List<WriteModel<BsonDocument>>();

            foreach (var keyValue in models)
            {
                var bsonDoc = keyValue.Value.ToBsonDocument();
                var filter = new BsonDocument("_id", BsonValue.Create(keyValue.Key));

                var replaceOne = new ReplaceOneModel<BsonDocument>(filter, bsonDoc) { IsUpsert = isUpsert };
                updateModels.Add(replaceOne);
            }

            var results = await collection.BulkWriteAsync(updateModels, null, cancellationToken);
            return results;
        }

        /// <summary>
        /// Executes a query to replace many storage models (for Update or AddOrUpdate many).
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <param name="models">The set of storage models to write.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isUpsert">
        /// If set to <c>true</c> models will be defined as upserts (AddOrUpdate); otherwise they
        /// will be defined as updates.
        /// </param>
        /// <returns>The bulk write task results.</returns>
        /// <exception cref="System.ArgumentNullException">If models is null.</exception>
        private static async Task<BulkWriteResult<BsonDocument>> ReplaceManyAsync<TDatabase, TId, TModel, TMetadata>(
            TDatabase readModelDatabase,
            IDictionary<TId, StorageModel<TModel, TMetadata>> models,
            string collectionName,
            CancellationToken cancellationToken,
            bool isUpsert)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

            var database = readModelDatabase.CreateConnection();
            var collection = database.GetCollection<BsonDocument>(modelTypeName);

            var updateModels = new List<WriteModel<BsonDocument>>();

            foreach (var keyValue in models)
            {
                var id = IdReader.ReadValue(keyValue.Value.Model);
                keyValue.Value.Id = id;
                var filter = new BsonDocument("_id", BsonValue.Create(id));
                var bsonDoc = keyValue.Value.ToBsonDocument();

                var replaceOne = new ReplaceOneModel<BsonDocument>(filter, bsonDoc) { IsUpsert = isUpsert };
                updateModels.Add(replaceOne);
            }

            var results = await collection.BulkWriteAsync(updateModels, null, cancellationToken);
            return results;
        }
    }
}
