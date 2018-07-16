// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetMongoCommandsInterface.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    /// <summary>
    /// Gets interfaces that allow executing commands against specific models in a database.
    /// </summary>
    public interface IGetMongoCommandsInterface : IGetCommandsInterface
    {
        /// <summary>
        /// Gets an interface that only allows executing commands against a specific model for this database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        IMongoCommands<TId, TModel> GetMongoCommandsInterface<TId, TModel>();
    }
}


