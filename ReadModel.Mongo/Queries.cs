// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Queries.cs">
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
    /// Provides an object model for executing queries for simple models using a real database.
    /// Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal sealed class Queries<TModel> : IQueries<TModel>
    {
        private readonly MongoDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Queries{TModel}" /> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal Queries(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <inheritdoc />
        public async Task<TModel> GetOneAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetOneAsync<TModel>(database);
            return await query(where, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> GetManyAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetManyAsync<TModel>(database);
            return await query(where, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> GetAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetAllAsync<TModel>(database);
            return await query(collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<TProjection> ProjectOneAsync<TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectOneAsync<TModel, TProjection>(database);
            return await query(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectManyAsync<TModel, TProjection>(database);
            return await query(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TProjection>> ProjectAllAsync<TProjection>(
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectAllAsync<TModel, TProjection>(database);
            return await query(project, collectionName, cancellationToken);
        }
    }

    /// <summary>
    /// Provides an object model for executing queries for storage models using a real database.
    /// Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    internal sealed class Queries<TModel, TMetadata> : IQueries<TModel, TMetadata>
    {
        private readonly MongoDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Queries{TModel, TMetadata}" /> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal Queries(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <inheritdoc />
        public async Task<TModel> GetOneAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetOneAsync<TModel, TMetadata>(database);
            return await query(where, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> GetManyAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetManyAsync<TModel, TMetadata>(database);
            return await query(where, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TModel>> GetAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetAllAsync<TModel, TMetadata>(database);
            return await query(collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<TProjection> ProjectOneAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectOneAsync<TModel, TMetadata, TProjection>(database);
            return await query(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectManyAsync<TModel, TMetadata, TProjection>(database);
            return await query(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TProjection>> ProjectAllAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectAllAsync<TModel, TMetadata, TProjection>(database);
            return await query(project, collectionName, cancellationToken);
        }
    }
}
