// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delegates.cs">
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

    // Doesn't throw - returns one or null if not found
    public delegate TModel GetOneQuery<TModel, TMetadata>(Expression<Func<TModel, TMetadata>> where);

    // Doesn't throw - returns all matching results or empty collection
    public delegate Task<IEnumerable<TModel>> GetManyQueryAsync<TModel>(
        Expression<Func<TModel, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task<IEnumerable<TModel>> GetManyQueryAsync<TModel, TMetadata>(
        Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate void RemoveManyCommandAsync<TModel>(
        Expression<Func<TModel>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate void RemoveManyCommandAsync<TModel, TMetadata>(
        Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task AddOneCommandAsync<in TModel>(
        TModel model,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task AddOneCommandAsync<in TModel, in TMetadata>(
        TModel model,
        TMetadata metadata = default(TMetadata),
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task AddManyCommandAsync<TModel, TMetadata>(
        IEnumerable<StorageModel<TModel, TMetadata>> storageModels,
        string modelType = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task AddManyCommandAsync<in TModel>(
        IEnumerable<TModel> models,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    // maybe this should take a StorageModel<TModel, TMetadata> instead of TModel and TMetadata
    // Updates throw if model is not present in database
    public delegate void UpdateOneCommand<TId, TModel, TMetadata>(TId id, TModel model, TMetadata metadata);

    public delegate void UpdateManyCommand<TId, TModel, TMetadata>(IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels);

    // maybe this should take a StorageModel<TModel, TMetadata> instead of TModel and TMetadata
    // Should not throw - though not sure if I can guarantee that in Mongo without additional work
    public delegate void AddOrUpdateOneCommand<TId, TModel, TMetadata>(TId id, TModel model, TMetadata metadata);

    public delegate void AddOrUpdateManyCommand<TId, TModel, TMetadata>(IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels);

    // if Id present in destination only - remove if Id present in source only - add otherwise update
    public delegate void MergeEntireSetCommand<TId, TModel, TMetadata>(IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels);
}
