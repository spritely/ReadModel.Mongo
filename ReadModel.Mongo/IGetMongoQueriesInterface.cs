// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetMongoQueriesInterface.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    /// <summary>
    /// Gets interfaces that allow querying specific models in a database.
    /// </summary>
    public interface IGetMongoQueriesInterface : IGetQueriesInterface
    {
        /// <summary>
        /// Gets an interface that only allows querying a specific model for this database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        IMongoQueries<TModel> GetMongoQueriesInterface<TModel>();
    }
}
