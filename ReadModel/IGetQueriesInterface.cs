// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetQueriesInterface.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel
{
    /// <summary>
    /// Gets interfaces that allow querying specific models in a database.
    /// </summary>
    public interface IGetQueriesInterface
    {
        /// <summary>
        /// Gets an interface that only allows querying a specific model for this database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        IQueries<TModel> GetQueriesInterface<TModel>();

        /// <summary>
        /// Gets an interface that only allows querying a specific model for this database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        IQueries<TModel, TMetadata> GetQueriesInterface<TModel, TMetadata>();
    }
}
