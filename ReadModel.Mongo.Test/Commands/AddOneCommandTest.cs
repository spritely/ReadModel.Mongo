// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOneCommandTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using NUnit.Framework;

    [TestFixture]
    public class AddOneCommandTest
    {
        private readonly IMongoDatabase databaseConnection;
        private readonly TestMetadata testMetadata = new TestMetadata(nameof(AddOneCommandTest));
        private readonly TestModel testModel = new TestModel(nameof(AddOneCommandTest));
        private readonly TestReadModelDatabase testReadModelDatabase = new TestReadModelDatabase();

        public AddOneCommandTest()
        {
            databaseConnection = testReadModelDatabase.CreateConnection();
        }

        [TearDown]
        public void CleanUp()
        {
            Task.Run(() => databaseConnection.DropCollectionAsync("TestModel")).Wait();
        }

        [Test]
        public void Adds_record_to_database()
        {
            var addTestModel = Create.AddOneCommandAsync<TestReadModelDatabase, TestModel>(testReadModelDatabase);

            var search = databaseConnection.GetCollection<TestModel>("TestModel")
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
            var addTestModel = Create.AddOneCommandAsync<TestReadModelDatabase, TestModel, TestMetadata>(testReadModelDatabase);

            var search = databaseConnection.GetCollection<StorageModel<TestModel, TestMetadata>>("TestModel")
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
    }
}
