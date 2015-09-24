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

    public sealed class TestReadModelDatabase : ReadModelDatabase<TestReadModelDatabase>, ITestDatabase
    {
        internal readonly RemoveAllCommandAsync RemoveAll;

        public IQueries<TestModel> ModelQueries { get; }

        public IQueries<TestModel, TestMetadata> StorageModelQueries { get; }

        public ICommands<Guid, TestModel> ModelCommands { get; }

        public ICommands<Guid, TestModel, TestMetadata> StorageModelCommands { get; }

        public TestReadModelDatabase()
        {
            ConnectionSettings = new MongoConnectionSettings
            {
                Database = "test"
            };

            RemoveAll = Commands.RemoveAllAsync(this);

            ModelQueries = Queries.CreateInterface<TestReadModelDatabase, TestModel>(this);
            StorageModelQueries = Queries.CreateInterface<TestReadModelDatabase, TestModel, TestMetadata>(this);
            ModelCommands = Commands.CreateInterface<TestReadModelDatabase, Guid, TestModel>(this);
            StorageModelCommands = Commands.CreateInterface<TestReadModelDatabase, Guid, TestModel, TestMetadata>(this);
        }

        public void Drop()
        {
            var databaseConnection = CreateConnection();
            Task.Run(() => databaseConnection.DropCollectionAsync(nameof(TestModel))).Wait();
        }

        public void AddTestModelsToDatabase(IEnumerable<TestModel> testModels)
        {
            var addTestModels = Commands.AddManyAsync<TestReadModelDatabase, TestModel>(this);

            var addTask = Task.Run(() => addTestModels(testModels));
            addTask.Wait();
        }

        public void AddStorageModelsToDatabase(IEnumerable<StorageModel<TestModel, TestMetadata>> storageModels)
        {
            var addTestModels = Commands.AddManyAsync<TestReadModelDatabase, TestModel, TestMetadata>(this);

            var addTask = Task.Run(() => addTestModels(storageModels));
            addTask.Wait();
        }
    }
}
