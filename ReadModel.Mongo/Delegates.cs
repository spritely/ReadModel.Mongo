// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delegates.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public delegate Task<TModel> GetOneQueryAsync<TModel>(
        Expression<Func<TModel, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task<TModel> GetOneQueryAsync<TModel, TMetadata>(
        Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task<IEnumerable<TModel>> GetManyQueryAsync<TModel>(
        Expression<Func<TModel, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task<IEnumerable<TModel>> GetManyQueryAsync<TModel, TMetadata>(
        Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task<IEnumerable<TModel>> GetAllQueryAsync<TModel>(
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task<IEnumerable<TModel>> GetAllQueryAsync<TModel, TMetadata>(
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task RemoveOneCommandAsync<in TModel>(
        TModel model,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task RemoveManyCommandAsync<TModel>(
        Expression<Func<TModel, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task RemoveManyCommandAsync<TModel, TMetadata>(
        Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task RemoveAllCommandAsync(
        string collectionName,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task RemoveAllCommandAsync<TModel>(
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task AddOneCommandAsync<in TModel>(
        TModel model,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    // Create an overload that takes a StorageModel??
    public delegate Task AddOneCommandAsync<in TModel, in TMetadata>(
        TModel model,
        TMetadata metadata = default(TMetadata),
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task AddManyCommandAsync<in TModel>(
        IEnumerable<TModel> models,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task AddManyCommandAsync<TModel, TMetadata>(
        IEnumerable<StorageModel<TModel, TMetadata>> storageModels,
        string modelType = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task UpdateOneCommandAsync<in TModel>(
        TModel model,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    // Create an overload that takes a StorageModel??
    public delegate Task UpdateOneCommandAsync<in TModel, in TMetadata>(
        TModel model,
        TMetadata metadata = default(TMetadata),
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    public delegate Task UpdateManyCommandAsync<TId, TModel>(
        IDictionary<TId, TModel> models,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    // Create an overload that takes a StorageModel??
    public delegate Task UpdateManyCommandAsync<TId, TModel, TMetadata>(
        IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
        string collectionName = null,
        CancellationToken cancellationToken = default(CancellationToken));

    // maybe this should take a StorageModel<TModel, TMetadata> instead of TModel and TMetadata
    // Should not throw - though not sure if I can guarantee that in Mongo without additional work
    public delegate void AddOrUpdateOneCommand<TId, TModel, TMetadata>(TId id, TModel model, TMetadata metadata);

    public delegate void AddOrUpdateManyCommand<TId, TModel, TMetadata>(IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels);

    // if Id present in destination only - remove if Id present in source only - add otherwise update
    public delegate void MergeEntireSetCommand<TId, TModel, TMetadata>(IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels);
}
