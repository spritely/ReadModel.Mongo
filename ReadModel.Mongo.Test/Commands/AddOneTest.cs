// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOneTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using NUnit.Framework;
    using Spritely.Cqrs;

    [TestFixture]
    public class AddOneTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();
        private readonly IMongoDatabase databaseConnection;
        private readonly TestMetadata testMetadata = new TestMetadata(nameof(AddOneTest));
        private readonly TestModel testModel = new TestModel(nameof(AddOneTest));

        public AddOneTest()
        {
            databaseConnection = database.CreateConnection();
        }

        [TearDown]
        public void CleanUp()
        {
            Task.Run(() => databaseConnection.DropCollectionAsync(nameof(TestModel))).Wait();
        }

        [Test]
        public void Adds_record_to_database()
        {
            var addTestModel = Commands.AddOneAsync<TestReadModelDatabase, TestModel>(database);

            var search = databaseConnection.GetCollection<TestModel>(nameof(TestModel))
                .Aggregate()
                .Match(m => m.Id == testModel.Id);

            var addTask = Task.Run(() => addTestModel(testModel));
            addTask.Wait();

            var searchTask = Task.Run(() => search.ToListAsync());
            searchTask.Wait();

            var result = searchTask.Result.Single();
            Assert.That(result.Name, Is.EqualTo(testModel.Name));
        }

        [Test]
        public void Adds_record_to_database_with_custom_metadata()
        {
            var addTestModel = Commands.AddOneAsync<TestReadModelDatabase, TestModel, TestMetadata>(database);

            var search = databaseConnection.GetCollection<StorageModel<TestModel, TestMetadata>>(nameof(TestModel))
                .Aggregate()
                .Match(sm => sm.Model.Id == testModel.Id);

            var addTask = Task.Run(() => addTestModel(testModel, testMetadata));
            addTask.Wait();

            var searchTask = Task.Run(() => search.ToListAsync());
            searchTask.Wait();

            var result = searchTask.Result.Single();
            Assert.That(result.Model.Name, Is.EqualTo(testModel.Name));
            Assert.That(result.Metadata.FirstName, Is.EqualTo(testMetadata.FirstName));
            Assert.That(result.Metadata.LastName, Is.EqualTo(testMetadata.LastName));
        }

        [Test]
        public void Throws_when_record_with_same_id_already_present_in_database()
        {
            var addTestModel = Commands.AddOneAsync<TestReadModelDatabase, TestModel>(database);

            var addTask = Task.Run(() => addTestModel(testModel));
            addTask.Wait();

            Assert.That(
                () => Task.Run(() => addTestModel(testModel)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<DatabaseException>());
        }

        [Test]
        public void Throws_when_record_with_same_id_already_present_in_database_with_custom_metadata()
        {
            var addTestModel = Commands.AddOneAsync<TestReadModelDatabase, TestModel, TestMetadata>(database);

            var addTask = Task.Run(() => addTestModel(testModel, testMetadata));
            addTask.Wait();

            Assert.That(
                () => Task.Run(() => addTestModel(testModel, testMetadata)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<DatabaseException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Commands.AddOneAsync<TestReadModelDatabase, TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Commands.AddOneAsync<TestReadModelDatabase, TestModel, TestMetadata>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            var addTestModel = Commands.AddOneAsync<TestReadModelDatabase, TestModel>(database);

            Assert.That(
                () => Task.Run(() => addTestModel(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            var addTestModel = Commands.AddOneAsync<TestReadModelDatabase, TestModel, TestMetadata>(database);

            Assert.That(
                () => Task.Run(() => addTestModel(null, testMetadata)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
