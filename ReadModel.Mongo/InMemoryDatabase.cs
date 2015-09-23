// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryDatabase.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Spritely.Recipes;

    public class InMemoryDatabase
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, object>> database =
            new ConcurrentDictionary<Type, ConcurrentDictionary<object, object>>();

        public IEnumerable<TModel> GetModels<TModel>()
        {
            var models = GetModelDatabase<TModel>();

            return models.Values.Cast<TModel>();
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "This is the desired interface for users of this class.")]
        public IEnumerable<TModel> GetModels<TModel, TMetadata>()
        {
            var models = GetModelDatabase<StorageModel<TModel, TMetadata>>();

            return models.Values.Cast<StorageModel<TModel, TMetadata>>().Select(sm => sm.Model);
        }

        public AddOrUpdateOneCommandAsync<TModel> AddOrUpdateOneCommandAsync<TModel>()
        {
            AddOrUpdateOneCommandAsync<TModel> command = (model, name, token) =>
            {
                var id = IdReader.ReadValue(model);
                var models = GetModelDatabase<TModel>();

                models.AddOrUpdate(id, _ => model, (_, __) => model);
                return Task.FromResult<object>(null);
            };

            return command;
        }

        public AddOrUpdateOneCommandAsync<TModel, TMetadata> AddOrUpdateOneCommandAsync<TModel, TMetadata>()
        {
            AddOrUpdateOneCommandAsync<TModel, TMetadata> command = (model, metadata, name, token) =>
            {
                var id = IdReader.ReadValue(model);
                var models = GetModelDatabase<StorageModel<TModel, TMetadata>>();

                var storageModel = new StorageModel<TModel, TMetadata>
                {
                    Id = id,
                    Model = model,
                    Metadata = metadata
                };

                models.AddOrUpdate(id, _ => storageModel, (_, __) => storageModel);
                return Task.FromResult<object>(null);
            };

            return command;
        }

        public MergeCompleteSetCommandAsync<TId, TModel> MergeCompleteSetCommandAsync<TId, TModel>()
        {
            MergeCompleteSetCommandAsync<TId, TModel> command = (models, name, token) =>
            {
                var modelDatabase = new ConcurrentDictionary<object, object>();

                models.ForEach(kv => modelDatabase.GetOrAdd(kv.Key, kv.Value));

                database.AddOrUpdate(typeof(TModel), _ => modelDatabase, (__, ___) => modelDatabase);

                return Task.FromResult<object>(null);
            };

            return command;
        }

        public MergeCompleteSetCommandAsync<TId, TModel, TMetadata> MergeCompleteSetCommandAsync<TId, TModel, TMetadata>()
        {
            MergeCompleteSetCommandAsync<TId, TModel, TMetadata> command = (models, name, token) =>
            {
                var modelDatabase = new ConcurrentDictionary<object, object>();

                models.ForEach(kv => modelDatabase.GetOrAdd(kv.Key, kv.Value));

                database.AddOrUpdate(typeof(TModel), _ => modelDatabase, (__, ___) => modelDatabase);

                return Task.FromResult<object>(null);
            };

            return command;
        }

        private ConcurrentDictionary<object, object> GetModelDatabase<TModel>()
        {
            var models = database.GetOrAdd(typeof(TModel), _ => new ConcurrentDictionary<object, object>());

            return models;
        }
    }
}
