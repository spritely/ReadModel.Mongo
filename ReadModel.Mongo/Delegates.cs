namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    // Doesn't throw - returns one or null if not found
    public delegate TModel GetOneQuery<TModel, TMetadata>(Expression<Func<TModel, TMetadata>> where);

    // Doesn't throw - returns all matching results or empty collection
    public delegate IReadOnlyCollection<TModel> GetManyQuery<TModel, TMetadata>(Expression<Func<StorageModel<TModel, TMetadata>, bool>> where, string modelType = null);

    public delegate void RemoveManyCommand<TModel, TMetadata>(Expression<Func<TModel, TMetadata>> where);

    // maybe this should take a StorageModel<TModel, TMetadata> called storageModel for consistency
    // - thoughts? Adds throw if object is already present in database
    public delegate void AddOneCommand<TModel, TMetadata>(TModel model, TMetadata metadata, string modelType = null);

    public delegate void AddManyCommand<TModel, TMetadata>(IEnumerable<StorageModel<TModel, TMetadata>> storageModels, string modelType = null);

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
