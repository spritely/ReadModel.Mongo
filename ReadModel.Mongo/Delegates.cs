// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delegates.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MongoDB.Driver;

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Queries for a single result using the specified filter. Throws if more than one result is returned.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="filterDefinition">The filter limiting data to one result.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>One model result.</returns>
    public delegate Task<TModel> GetOneQueryUsingFilterDefinitionAsync<TModel>(
        FilterDefinition<TModel> filterDefinition,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Queries for many results using the specified filter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="filterDefinition">The filter limiting results.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The matching results.</returns>
    public delegate Task<IEnumerable<TModel>> GetManyQueryUsingFilterDefinitionAsync<TModel>(
        FilterDefinition<TModel> filterDefinition,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Projects a single result into a new type using the specified filter. Throws if more than one
    /// result is returned.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TProjection">The type of the projection.</typeparam>
    /// <param name="filterDefinition">The filter limiting data to one result.</param>
    /// <param name="project">The callback responsible for creating a projection from a model.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>One model projection.</returns>
    public delegate Task<TProjection> ProjectOneQueryUsingFilterDefinitionAsync<TModel, TProjection>(
        FilterDefinition<TModel> filterDefinition,
        Expression<Func<TModel, TProjection>> project,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Projects many results into new types using the specified filter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TProjection">The type of the projection.</typeparam>
    /// <param name="filterDefinition">The filter limiting results.</param>
    /// <param name="project">The callback responsible for creating a projection from a model.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The matching projections.</returns>
    public delegate Task<IEnumerable<TProjection>> ProjectManyQueryUsingFilterDefinitionAsync<TModel, TProjection>(
        FilterDefinition<TModel> filterDefinition,
        Expression<Func<TModel, TProjection>> project,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Removes models that match the specified filter.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="filterDefinition">The filter specifying which results should be removed.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task to track and manage progress.</returns>
    public delegate Task RemoveManyCommandUsingFilterDefinitionAsync<TModel>(
        FilterDefinition<TModel> filterDefinition,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));
}
