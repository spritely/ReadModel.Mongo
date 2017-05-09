// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestInMemoryDatabase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class TestInMemoryDatabase : ITestDatabase
    {
        public InMemoryDatabase Database { get; private set; } = new InMemoryDatabase();

        public IQueries<TestModel> ModelQueries { get; }

        public IQueries<TestModel, TestMetadata> StorageModelQueries { get; }

        public ICommands<Guid, TestModel> ModelCommands { get; }

        public ICommands<Guid, TestModel, TestMetadata> StorageModelCommands { get; }

        public TestInMemoryDatabase()
        {
            ModelQueries = Database.GetQueriesInterface<TestModel>();
            StorageModelQueries = Database.GetQueriesInterface<TestModel, TestMetadata>();
            ModelCommands = Database.GetCommandsInterface<Guid, TestModel>();
            StorageModelCommands = Database.GetCommandsInterface<Guid, TestModel, TestMetadata>();
        }

        public void Drop()
        {
            Database = new InMemoryDatabase();
        }

        public void AddTestModelsToDatabase(IEnumerable<TestModel> testModels)
        {
            var addTask = Task.Run(() => Database.AddManyCommandAsync(testModels, "TestModel", default(CancellationToken)));
            addTask.Wait();
        }

        public void AddStorageModelsToDatabase(IEnumerable<StorageModel<TestModel, TestMetadata>> storageModels)
        {
            var addTask = Task.Run(() => Database.AddManyCommandAsync(storageModels, "TestModel", default(CancellationToken)));
            addTask.Wait();
        }
    }
}
