﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestReadModelDatabase.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public sealed class TestReadModelDatabase : ReadModelDatabase<TestReadModelDatabase>
    {
        internal readonly GetAllQueryAsync<TestModel, TestMetadata> GetAllStorageModels;
        internal readonly GetAllQueryAsync<TestModel> GetAllTestModels;
        internal readonly GetManyQueryAsync<TestModel, TestMetadata> GetManyStorageModels;
        internal readonly GetManyQueryAsync<TestModel> GetManyTestModels;
        internal readonly GetOneQueryAsync<TestModel, TestMetadata> GetOneStorageModel;
        internal readonly GetOneQueryAsync<TestModel> GetOneTestModel;
        internal readonly RemoveAllCommandAsync RemoveAll;
        internal readonly RemoveAllCommandAsync<TestModel> RemoveAllModels;
        internal readonly RemoveManyCommandAsync<TestModel, TestMetadata> RemoveManyStorageModels;
        internal readonly RemoveManyCommandAsync<TestModel> RemoveManyTestModels;
        internal readonly RemoveOneCommandAsync<TestModel> RemoveOneTestModel;
        internal readonly UpdateManyCommandAsync<Guid, TestModel, TestMetadata> UpdateManyStorageModels;
        internal readonly UpdateManyCommandAsync<Guid, TestModel> UpdateManyTestModels;
        internal readonly UpdateOneCommandAsync<TestModel, TestMetadata> UpdateOneStorageModel;
        internal readonly UpdateOneCommandAsync<TestModel> UpdateOneTestModel;
        internal readonly AddOrUpdateManyCommandAsync<Guid, TestModel, TestMetadata> AddOrUpdateManyStorageModels;
        internal readonly AddOrUpdateManyCommandAsync<Guid, TestModel> AddOrUpdateManyTestModels;
        internal readonly AddOrUpdateOneCommandAsync<TestModel, TestMetadata> AddOrUpdateOneStorageModel;
        internal readonly AddOrUpdateOneCommandAsync<TestModel> AddOrUpdateOneTestModel;
        internal readonly MergeCompleteSetCommandAsync<Guid, TestModel, TestMetadata> MergeCompleteSetOfStorageModels;
        internal readonly MergeCompleteSetCommandAsync<Guid, TestModel> MergeCompleteSetOfTestModels;

        public TestReadModelDatabase()
        {
            ConnectionSettings = new MongoConnectionSettings
            {
                Database = "test"
            };

            GetAllTestModels = Queries.GetAllAsync<TestReadModelDatabase, TestModel>(this);
            GetAllStorageModels = Queries.GetAllAsync<TestReadModelDatabase, TestModel, TestMetadata>(this);
            GetManyTestModels = Queries.GetManyAsync<TestReadModelDatabase, TestModel>(this);
            GetManyStorageModels = Queries.GetManyAsync<TestReadModelDatabase, TestModel, TestMetadata>(this);
            GetOneTestModel = Queries.GetOneAsync<TestReadModelDatabase, TestModel>(this);
            GetOneStorageModel = Queries.GetOneAsync<TestReadModelDatabase, TestModel, TestMetadata>(this);

            RemoveAll = Commands.RemoveAllAsync(this);
            RemoveAllModels = Commands.RemoveAllAsync<TestReadModelDatabase, TestModel>(this);
            RemoveManyTestModels = Commands.RemoveManyAsync<TestReadModelDatabase, TestModel>(this);
            RemoveManyStorageModels = Commands.RemoveManyAsync<TestReadModelDatabase, TestModel, TestMetadata>(this);
            RemoveOneTestModel = Commands.RemoveOneAsync<TestReadModelDatabase, TestModel>(this);
            UpdateOneTestModel = Commands.UpdateOneAsync<TestReadModelDatabase, TestModel>(this);
            UpdateOneStorageModel = Commands.UpdateOneAsync<TestReadModelDatabase, TestModel, TestMetadata>(this);
            UpdateManyTestModels = Commands.UpdateManyAsync<TestReadModelDatabase, Guid, TestModel>(this);
            UpdateManyStorageModels = Commands.UpdateManyAsync<TestReadModelDatabase, Guid, TestModel, TestMetadata>(this);
            AddOrUpdateOneTestModel = Commands.AddOrUpdateOneAsync<TestReadModelDatabase, TestModel>(this);
            AddOrUpdateOneStorageModel = Commands.AddOrUpdateOneAsync<TestReadModelDatabase, TestModel, TestMetadata>(this);
            AddOrUpdateManyTestModels = Commands.AddOrUpdateManyAsync<TestReadModelDatabase, Guid, TestModel>(this);
            AddOrUpdateManyStorageModels = Commands.AddOrUpdateManyAsync<TestReadModelDatabase, Guid, TestModel, TestMetadata>(this);
            MergeCompleteSetOfTestModels = Commands.MergeCompleteSetAsync<TestReadModelDatabase, Guid, TestModel>(this);
            MergeCompleteSetOfStorageModels = Commands.MergeCompleteSetAsync<TestReadModelDatabase, Guid, TestModel, TestMetadata>(this);
        }

        internal Task<IEnumerable<TProjection>> ProjectAllTestModels<TProjection>(
            Expression<Func<TestModel, TProjection>> project)
        {
            var query = Queries.ProjectAllAsync<TestReadModelDatabase, TestModel, TProjection>(this);
            return query(project);
        }

        internal Task<IEnumerable<TProjection>> ProjectAllStorageModels<TProjection>(
            Expression<Func<StorageModel<TestModel, TestMetadata>, TProjection>> project)
        {
            var query = Queries.ProjectAllAsync<TestReadModelDatabase, TestModel, TestMetadata, TProjection>(this);
            return query(project);
        }

        internal Task<IEnumerable<TProjection>> ProjectManyTestModels<TProjection>(
            Expression<Func<TestModel, bool>> where,
            Expression<Func<TestModel, TProjection>> project)
        {
            var query = Queries.ProjectManyAsync<TestReadModelDatabase, TestModel, TProjection>(this);
            return query(where, project);
        }

        internal Task<IEnumerable<TProjection>> ProjectManyStorageModels<TProjection>(
            Expression<Func<StorageModel<TestModel, TestMetadata>, bool>> where,
            Expression<Func<StorageModel<TestModel, TestMetadata>, TProjection>> project)
        {
            var query = Queries.ProjectManyAsync<TestReadModelDatabase, TestModel, TestMetadata, TProjection>(this);
            return query(where, project);
        }

        internal Task<TProjection> ProjectOneTestModel<TProjection>(
            Expression<Func<TestModel, bool>> where,
            Expression<Func<TestModel, TProjection>> project)
        {
            var query = Queries.ProjectOneAsync<TestReadModelDatabase, TestModel, TProjection>(this);
            return query(where, project);
        }

        internal Task<TProjection> ProjectOneStorageModel<TProjection>(
            Expression<Func<StorageModel<TestModel, TestMetadata>, bool>> where,
            Expression<Func<StorageModel<TestModel, TestMetadata>, TProjection>> project)
        {
            var query = Queries.ProjectOneAsync<TestReadModelDatabase, TestModel, TestMetadata, TProjection>(this);
            return query(where, project);
        }

        internal void AddTestModelsToDatabase(IEnumerable<TestModel> testModels)
        {
            var addTestModels = Commands.AddManyAsync<TestReadModelDatabase, TestModel>(this);

            var addTask = Task.Run(() => addTestModels(testModels));
            addTask.Wait();
        }

        internal void AddStorageModelsToDatabase(IEnumerable<StorageModel<TestModel, TestMetadata>> storageModels)
        {
            var addTestModels = Commands.AddManyAsync<TestReadModelDatabase, TestModel, TestMetadata>(this);

            var addTask = Task.Run(() => addTestModels(storageModels));
            addTask.Wait();
        }
    }
}
