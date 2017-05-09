// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetCommandsInterface.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel
{
    /// <summary>
    /// Gets interfaces that allow executing commands against specific models in a database.
    /// </summary>
    public interface IGetCommandsInterface
    {
        /// <summary>
        /// Gets an interface that only allows executing commands against a specific model for this database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        ICommands<TId, TModel> GetCommandsInterface<TId, TModel>();

        /// <summary>
        /// Gets an interface that only allows executing commands against a specific model for this database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <returns>A database wrapped in a standard interface.</returns>
        ICommands<TId, TModel, TMetadata> GetCommandsInterface<TId, TModel, TMetadata>();
    }
}


