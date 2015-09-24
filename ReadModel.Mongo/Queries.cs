﻿// --------------------------------------------------------------------------------------------------------------------
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

    public static partial class Queries
    {
        /// <summary>
        /// Creates an interface for querying a specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A database wrapped in a standard interface.</returns>
        public static IQueries<TModel> CreateInterface<TDatabase, TModel>(TDatabase database) where TDatabase : ReadModelDatabase<TDatabase>
        {
            var queries = new Queries<TDatabase, TModel>(database);
            return queries;
        }

        /// <summary>
        /// Creates an interface for querying a specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A database wrapped in a standard interface.</returns>
        public static IQueries<TModel, TMetadata> CreateInterface<TDatabase, TModel, TMetadata>(TDatabase database)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var queries = new Queries<TDatabase, TModel, TMetadata>(database);
            return queries;
        }

        /// <summary>
        /// Creates an interface for querying a specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A database wrapped in a standard interface.</returns>
        public static IQueries<TModel> CreateInterface<TModel>(InMemoryDatabase database)
        {
            var queries = new InMemoryQueries<TModel>(database);
            return queries;
        }

        /// <summary>
        /// Creates an interface for querying a specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A database wrapped in a standard interface.</returns>
        public static IQueries<TModel, TMetadata> CreateInterface<TModel, TMetadata>(InMemoryDatabase database)
        {
            var queries = new InMemoryQueries<TModel, TMetadata>(database);
            return queries;
        }
    }

    /// <summary>
    /// Provides an object model for executing queries for simple models using a real database.
    /// Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TDatabase">The type of the database.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    internal sealed class Queries<TDatabase, TModel> : IQueries<TModel> where TDatabase : ReadModelDatabase<TDatabase>
    {
        private readonly TDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Queries{TDatabase,TModel}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal Queries(TDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <inheritdoc/>
        public async Task<TModel> GetOneAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetOneAsync<TDatabase, TModel>(database);
            return await query(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TModel>> GetManyAsync(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetManyAsync<TDatabase, TModel>(database);
            return await query(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TModel>> GetAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetAllAsync<TDatabase, TModel>(database);
            return await query(collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TProjection> ProjectOneAsync<TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectOneAsync<TDatabase, TModel, TProjection>(database);
            return await query(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectManyAsync<TDatabase, TModel, TProjection>(database);
            return await query(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TProjection>> ProjectAllAsync<TProjection>(
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectAllAsync<TDatabase, TModel, TProjection>(database);
            return await query(project, collectionName, cancellationToken);
        }
    }

    /// <summary>
    /// Provides an object model for executing queries for storage models using a real database.
    /// Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TDatabase">The type of the database.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    internal sealed class Queries<TDatabase, TModel, TMetadata> : IQueries<TModel, TMetadata> where TDatabase : ReadModelDatabase<TDatabase>
    {
        private readonly TDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Queries{TDatabase, TModel, TMetadata}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        internal Queries(TDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <inheritdoc/>
        public async Task<TModel> GetOneAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetOneAsync<TDatabase, TModel, TMetadata>(database);
            return await query(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TModel>> GetManyAsync(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetManyAsync<TDatabase, TModel, TMetadata>(database);
            return await query(where, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TModel>> GetAllAsync(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.GetAllAsync<TDatabase, TModel, TMetadata>(database);
            return await query(collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TProjection> ProjectOneAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectOneAsync<TDatabase, TModel, TMetadata, TProjection>(database);
            return await query(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectManyAsync<TDatabase, TModel, TMetadata, TProjection>(database);
            return await query(where, project, collectionName, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TProjection>> ProjectAllAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = Queries.ProjectAllAsync<TDatabase, TModel, TMetadata, TProjection>(database);
            return await query(project, collectionName, cancellationToken);
        }
    }
}