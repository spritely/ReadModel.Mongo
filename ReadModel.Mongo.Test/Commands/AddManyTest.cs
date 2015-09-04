// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddManyTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using NUnit.Framework;

    [TestFixture]
    public class AddManyTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();
        private readonly IMongoDatabase databaseConnection;

        private readonly IReadOnlyList<StorageModel<TestModel, TestMetadata>> storageModels =
            StorageModel.CreateMany(nameof(AddManyTest), count: 3);

        private readonly IReadOnlyList<TestModel> testModels = TestModel.CreateMany(nameof(AddManyTest), count: 3);

        public AddManyTest()
        {
            databaseConnection = database.CreateConnection();
        }

        [TearDown]
        public void CleanUp()
        {
            Task.Run(() => databaseConnection.DropCollectionAsync("TestModel")).Wait();
        }

        [Test]
        public void Adds_records_to_database()
        {
            var addTestModels = Commands.AddManyAsync<TestReadModelDatabase, TestModel>(database);

            var search = databaseConnection.GetCollection<TestModel>("TestModel")
                .Aggregate()
                .Match(m => true);

            var addTask = Task.Run(() => addTestModels(testModels));
            addTask.Wait();

            var searchTask = Task.Run(() => search.ToListAsync());
            searchTask.Wait();

            var results = searchTask.Result;
            AssertResults.Match(results, testModels);
        }

        [Test]
        public void Adds_record_to_database_with_custom_metadata()
        {
            var addTestModels = Commands.AddManyAsync<TestReadModelDatabase, TestModel, TestMetadata>(database);

            var search = databaseConnection.GetCollection<StorageModel<TestModel, TestMetadata>>("TestModel")
                .Aggregate()
                .Match(m => true);

            var addTask = Task.Run(() => addTestModels(storageModels));
            addTask.Wait();

            var searchTask = Task.Run(() => search.ToListAsync());
            searchTask.Wait();

            var results = searchTask.Result;
            AssertResults.Match(results, storageModels);
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Commands.AddManyAsync<TestReadModelDatabase, TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Commands.AddManyAsync<TestReadModelDatabase, TestModel, TestMetadata>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            var addTestModels = Commands.AddManyAsync<TestReadModelDatabase, TestModel>(database);

            Assert.That(
                () => Task.Run(() => addTestModels(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var addTestModels = Commands.AddManyAsync<TestReadModelDatabase, TestModel, TestMetadata>(database);

            Assert.That(
                () => Task.Run(() => addTestModels(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
