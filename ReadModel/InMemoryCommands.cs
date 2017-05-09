// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryCommands.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides an object model for executing queries for simple models using a real database.
    /// Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal sealed class InMemoryCommands<TId, TModel> : ICommands<TId, TModel>
    {
        private readonly InMemoryDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCommands{TDatabase,TId,TModel}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal InMemoryCommands(InMemoryDatabase database)
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
            await database.RemoveOneCommandAsync(model, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveManyAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.RemoveManyCommandAsync(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.RemoveAllCommandAsync<TModel>(collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.AddOneCommandAsync(model, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddManyAsync(
            IEnumerable<TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.AddManyCommandAsync(models, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.UpdateOneCommandAsync(model, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateManyAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.UpdateManyCommandAsync(models, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOrUpdateOneAsync(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.AddOrUpdateOneCommandAsync(model, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOrUpdateManyAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.AddOrUpdateManyCommandAsync(models, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task MergeCompleteSetAsync(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.MergeCompleteSetCommandAsync(models, collectionName, cancellationToken);
        }
    }

    /// <summary>
    /// Provides an object model for executing queries for storage models using a real database.
    /// Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    internal sealed class InMemoryCommands<TId, TModel, TMetadata> : ICommands<TId, TModel, TMetadata>
    {
        private readonly InMemoryDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCommands{TDatabase, TId, TModel,
        /// TMetadata}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal InMemoryCommands(InMemoryDatabase database)
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
            await database.RemoveOneCommandAsync(model, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveManyAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.RemoveManyCommandAsync(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RemoveAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.RemoveAllCommandAsync<TModel>(collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.AddOneCommandAsync(model, metadata, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddManyAsync(
            IEnumerable<StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.AddManyCommandAsync(storageModels, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.UpdateOneCommandAsync(model, metadata, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateManyAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.UpdateManyCommandAsync(storageModels, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOrUpdateOneAsync(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.AddOrUpdateOneCommandAsync(model, metadata, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task AddOrUpdateManyAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.AddOrUpdateManyCommandAsync(storageModels, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task MergeCompleteSetAsync(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await database.MergeCompleteSetCommandAsync(storageModels, collectionName, cancellationToken);
        }
    }
}
