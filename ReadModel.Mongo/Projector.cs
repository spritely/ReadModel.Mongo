// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Projector.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Encapsulates a partially constructed set of projection queries for simple models. This helps
    /// with dependency injection because projection types are not known until the point they are
    /// called and not at injection time.
    /// </summary>
    /// <typeparam name="TDatabase">The type of the database.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public sealed class Projector<TDatabase, TModel>
        where TDatabase : ReadModelDatabase<TDatabase>
    {
        private readonly TDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Projector{TDatabase, TModel}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        public Projector(TDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <summary>
        /// Executes query and projects all models into the requested form.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="project">The callback responsible for creating a projection from a model.</param>
        /// <returns>A task encapsulating the future projected set of results.</returns>
        public async Task<IEnumerable<TProjection>> ProjectAllAsync<TProjection>(
            Expression<Func<TModel, TProjection>> project)
        {
            var query = Queries.ProjectAllAsync<TDatabase, TModel, TProjection>(database);
            return await query(project);
        }

        /// <summary>
        /// Executes query and projects the specified subset of models into the requested form.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause limiting results.</param>
        /// <param name="project">The callback responsible for creating a projection from a model.</param>
        /// <returns>A task encapsulating the future projected set of filtered results.</returns>
        public async Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project)
        {
            var query = Queries.ProjectManyAsync<TDatabase, TModel, TProjection>(database);
            return await query(where, project);
        }

        /// <summary>
        /// Executes query and projects the specified model into the requested form. Throws if
        /// filter does not produce a single result.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause to find a single result.</param>
        /// <param name="project">The callback responsible for creating a projection from a model.</param>
        /// <returns>A task encapsulating the future projected result.</returns>
        public async Task<TProjection> ProjectOneAsync<TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project)
        {
            var query = Queries.ProjectOneAsync<TDatabase, TModel, TProjection>(database);
            return await query(where, project);
        }
    }

    /// <summary>
    /// Encapsulates a partially constructed set of projection queries for storage models. This
    /// helps with dependency injection because projection types are not known until the point they
    /// are called and not at injection time.
    /// </summary>
    /// <typeparam name="TDatabase">The type of the database.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    public sealed class Projector<TDatabase, TModel, TMetadata>
        where TDatabase : ReadModelDatabase<TDatabase>
    {
        private readonly TDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Projector{TDatabase, TModel, TMetadata}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        public Projector(TDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            this.database = database;
        }

        /// <summary>
        /// Executes query and projects all models with metadata into the requested form.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="project">
        /// The callback responsible for creating a projection from a model and metadata.
        /// </param>
        /// <returns>A task encapsulating the future projected set of results.</returns>
        public async Task<IEnumerable<TProjection>> ProjectAllAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project)
        {
            var query = Queries.ProjectAllAsync<TDatabase, TModel, TMetadata, TProjection>(database);
            return await query(project);
        }

        /// <summary>
        /// Executes query and projects the specified subset of models with metadata into the
        /// requested form.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause limiting results.</param>
        /// <param name="project">
        /// The callback responsible for creating a projection from a model and metadata.
        /// </param>
        /// <returns>A task encapsulating the future projected set of filtered results.</returns>
        public async Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project)
        {
            var query = Queries.ProjectManyAsync<TDatabase, TModel, TMetadata, TProjection>(database);
            return await query(where, project);
        }

        /// <summary>
        /// Executes query and projects the specified model with metadata into the requested form.
        /// Throws if filter does not produce a single result.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause to find a single result.</param>
        /// <param name="project">
        /// The callback responsible for creating a projection from a model and metadata.
        /// </param>
        /// <returns>A task encapsulating the future projected result.</returns>
        internal async Task<TProjection> ProjectOneAsync<TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project)
        {
            var query = Queries.ProjectOneAsync<TDatabase, TModel, TMetadata, TProjection>(database);
            return await query(where, project);
        }
    }
}
