// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestMongoDatabase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class TestMongoDatabase : MongoDatabase, ITestDatabase
    {
        internal readonly RemoveAllCommandAsync RemoveAll;

        public IQueries<TestModel> ModelQueries { get; }

        public IQueries<TestModel, TestMetadata> StorageModelQueries { get; }

        public ICommands<Guid, TestModel> ModelCommands { get; }

        public ICommands<Guid, TestModel, TestMetadata> StorageModelCommands { get; }

        public TestMongoDatabase()
            : base(new MongoConnectionSettings { Database = "test" })
        {
            RemoveAll = Commands.RemoveAllAsync(this);

            ModelQueries = GetQueriesInterface<TestModel>();
            StorageModelQueries = GetQueriesInterface<TestModel, TestMetadata>();
            ModelCommands = GetCommandsInterface<Guid, TestModel>();
            StorageModelCommands = GetCommandsInterface<Guid, TestModel, TestMetadata>();
        }

        public void Drop()
        {
            var client = CreateClient();
            Task.Run(() => client.DropCollectionAsync(nameof(TestModel))).Wait();
        }

        public void AddTestModelsToDatabase(IEnumerable<TestModel> testModels)
        {
            var addTestModels = Commands.AddManyAsync<TestModel>(this);

            var addTask = Task.Run(() => addTestModels(testModels));
            addTask.Wait();
        }

        public void AddStorageModelsToDatabase(IEnumerable<StorageModel<TestModel, TestMetadata>> storageModels)
        {
            var addTestModels = Commands.AddManyAsync<TestModel, TestMetadata>(this);

            var addTask = Task.Run(() => addTestModels(storageModels));
            addTask.Wait();
        }
    }
}
