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
    using System.Linq;
    using System.Threading.Tasks;

    internal class InMemoryDatabase
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, object>> database =
            new ConcurrentDictionary<Type, ConcurrentDictionary<object, object>>();

        public IEnumerable<TModel> GetModels<TModel>()
        {
            var models = GetModelDatabase<TModel>();

            return models.Values.Cast<TModel>();
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

        private ConcurrentDictionary<object, object> GetModelDatabase<TModel>()
        {
            var models = database.GetOrAdd(typeof(TModel), _ => new ConcurrentDictionary<object, object>());

            return models;
        }
    }
}
