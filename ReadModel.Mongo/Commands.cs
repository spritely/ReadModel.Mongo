// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Commands.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using MongoDB.Driver;

    /// <summary>
    /// Provides an object model for executing commands with simple models using a real database.
    /// Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal sealed class Commands<TId, TModel> : IMongoCommands<TId, TModel>
    {
        private readonly MongoDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Commands{TId,TModel}" /> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal Commands(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <inheritdoc />
        public async Task RemoveOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveOneAsync<TModel>(database);
            await command(model, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task RemoveManyAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveManyAsync<TModel>(database);
            await command(where, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task RemoveManyAsync(
            FilterDefinition<TModel> filterDefinition,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveManyUsingFilterDefinitionAsync<TModel>(database);
            await command(filterDefinition, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task RemoveAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveAllAsync<TModel>(database);
            await command(collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task AddOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOneAsync<TModel>(database);
            await command(model, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task AddManyAsync(
            IEnumerable<TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddManyAsync<TModel>(database);
            await command(models, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task UpdateOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.UpdateOneAsync<TModel>(database);
            await command(model, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task UpdateManyAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.UpdateManyAsync<TId, TModel>(database);
            await command(models, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task AddOrUpdateOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOrUpdateOneAsync<TModel>(database);
            await command(model, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task AddOrUpdateManyAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOrUpdateManyAsync<TId, TModel>(database);
            await command(models, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task MergeCompleteSetAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.MergeCompleteSetAsync<TId, TModel>(database);
            await command(models, collectionName, cancellationToken);
        }
    }

    /// <summary>
    /// Provides an object model for executing commands with storage models using a real database.
    /// Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    internal sealed class Commands<TId, TModel, TMetadata> : ICommands<TId, TModel, TMetadata>
    {
        private readonly MongoDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Commands{TId, TModel, TMetadata}" /> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal Commands(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <inheritdoc />
        public async Task RemoveOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveOneAsync<TModel>(database);
            await command(model, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task RemoveManyAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveManyAsync<TModel, TMetadata>(database);
            await command(where, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task RemoveAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveAllAsync<TModel>(database);
            await command(collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task AddOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOneAsync<TModel, TMetadata>(database);
            await command(model, metadata, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task AddManyAsync(
            IEnumerable<StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddManyAsync<TModel, TMetadata>(database);
            await command(storageModels, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task UpdateOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.UpdateOneAsync<TModel, TMetadata>(database);
            await command(model, metadata, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task UpdateManyAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.UpdateManyAsync<TId, TModel, TMetadata>(database);
            await command(storageModels, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task AddOrUpdateOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOrUpdateOneAsync<TModel, TMetadata>(database);
            await command(model, metadata, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task AddOrUpdateManyAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOrUpdateManyAsync<TId, TModel, TMetadata>(database);
            await command(storageModels, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task MergeCompleteSetAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.MergeCompleteSetAsync<TId, TModel, TMetadata>(database);
            await command(storageModels, collectionName, cancellationToken);
        }
    }
}
