// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMongoCommands.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using MongoDB.Driver;

namespace Spritely.ReadModel.Mongo
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an object for executing commands with simple models. Useful for dependency injection.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IMongoCommands<TId, TModel> : ICommands<TId, TModel>
    {
        /// <summary>
        /// Executes the command to remove a set of models matching the specified filter from the database.
        /// </summary>
        /// <param name="filterDefinition">The filter indicating which models to remove.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        Task RemoveManyAsync(
            FilterDefinition<TModel> filterDefinition,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
