// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestReadModelDatabase.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Spritely.Cqrs;

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

        public TestReadModelDatabase()
        {
            ConnectionSettings = new MongoConnectionSettings
            {
                Database = "test",
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
