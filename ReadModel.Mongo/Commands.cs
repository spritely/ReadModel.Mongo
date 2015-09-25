// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Commands.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
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

    /// <summary>
    /// Provides an object model for executing commands with simple models using a real database.
    /// Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TDatabase">The type of the database.</typeparam>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal sealed class Commands<TDatabase, TId, TModel> : ICommands<TId, TModel> where TDatabase : ReadModelDatabase<TDatabase>
    {
        private readonly TDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Commands{TDatabase,TId,TModel}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal Commands(TDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <inheritdoc/>
        public async Task RemoveOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveOneAsync<TDatabase, TModel>(database);
            await command(model, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveManyAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveManyAsync<TDatabase, TModel>(database);
            await command(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveAllAsync<TDatabase, TModel>(database);
            await command(collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOneAsync<TDatabase, TModel>(database);
            await command(model, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddManyAsync(
            IEnumerable<TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddManyAsync<TDatabase, TModel>(database);
            await command(models, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.UpdateOneAsync<TDatabase, TModel>(database);
            await command(model, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateManyAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.UpdateManyAsync<TDatabase, TId, TModel>(database);
            await command(models, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOrUpdateOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOrUpdateOneAsync<TDatabase, TModel>(database);
            await command(model, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOrUpdateManyAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOrUpdateManyAsync<TDatabase, TId, TModel>(database);
            await command(models, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task MergeCompleteSetAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.MergeCompleteSetAsync<TDatabase, TId, TModel>(database);
            await command(models, collectionName, cancellationToken);
        }
    }

    /// <summary>
    /// Provides an object model for executing commands with storage models using a real database.
    /// Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TDatabase">The type of the database.</typeparam>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    internal sealed class Commands<TDatabase, TId, TModel, TMetadata> : ICommands<TId, TModel, TMetadata>
        where TDatabase : ReadModelDatabase<TDatabase>
    {
        private readonly TDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Commands{TDatabase, TId, TModel,
        /// TMetadata}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal Commands(TDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <inheritdoc/>
        public async Task RemoveOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveOneAsync<TDatabase, TModel>(database);
            await command(model, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveManyAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveManyAsync<TDatabase, TModel, TMetadata>(database);
            await command(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.RemoveAllAsync<TDatabase, TModel>(database);
            await command(collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOneAsync<TDatabase, TModel, TMetadata>(database);
            await command(model, metadata, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddManyAsync(
            IEnumerable<StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddManyAsync<TDatabase, TModel, TMetadata>(database);
            await command(storageModels, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.UpdateOneAsync<TDatabase, TModel, TMetadata>(database);
            await command(model, metadata, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateManyAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.UpdateManyAsync<TDatabase, TId, TModel, TMetadata>(database);
            await command(storageModels, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOrUpdateOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOrUpdateOneAsync<TDatabase, TModel, TMetadata>(database);
            await command(model, metadata, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOrUpdateManyAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.AddOrUpdateManyAsync<TDatabase, TId, TModel, TMetadata>(database);
            await command(storageModels, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task MergeCompleteSetAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var command = Commands.MergeCompleteSetAsync<TDatabase, TId, TModel, TMetadata>(database);
            await command(storageModels, collectionName, cancellationToken);
        }
    }
}
