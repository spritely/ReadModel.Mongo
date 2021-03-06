﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageModel.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel
{
    /// <summary>
    /// Representation of the model in the store.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
    public class StorageModel<TModel, TMetadata>
    {
        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public TMetadata Metadata { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public TModel Model { get; set; }

        /// <summary>
        /// Gets or sets the identifier. This property is designed for consumption by database
        /// specific packages like Spritely.ReadModel.Mongo and not for general pupose consumption.
        /// </summary>
        /// <value>The identifier.</value>
        public object Id { get; set; }
    }
}
