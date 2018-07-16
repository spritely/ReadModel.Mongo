// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMongoQueries.cs">
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
    /// Represents an object for executing queries for simple models. Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IMongoQueries<TModel> : IQueries<TModel>
    {
        /// <summary>
        /// Executes query to get the specified model into the requested form. Throws if filter does
        /// not produce a single result.
        /// </summary>
        /// <param name="filterDefinition">The filter to find a single result.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future result.</returns>
        Task<TModel> GetOneAsync(
            FilterDefinition<TModel> filterDefinition,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query to get the specified subset of models into the requested form.
        /// </summary>
        /// <param name="filterDefinition">The filter limiting results.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future set of filtered results.</returns>
        Task<IEnumerable<TModel>> GetManyAsync(
            FilterDefinition<TModel> filterDefinition,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query and projects the specified model into the requested form. Throws if
        /// filter does not produce a single result.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="filterDefinition">The filter to find a single result.</param>
        /// <param name="project">The callback responsible for creating a projection from a model.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected result.</returns>
        Task<TProjection> ProjectOneAsync<TProjection>(
            FilterDefinition<TModel> filterDefinition,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Executes query and projects the specified subset of models into the requested form.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="filterDefinition">The filter limiting results.</param>
        /// <param name="project">The callback responsible for creating a projection from a model.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected set of filtered results.</returns>
        Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
            FilterDefinition<TModel> filterDefinition,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
