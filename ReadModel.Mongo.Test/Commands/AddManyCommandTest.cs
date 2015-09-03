// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddManyCommandTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MongoDB.Driver;
    using NUnit.Framework;

    [TestFixture]
    public class AddManyCommandTest
    {
        private readonly IMongoDatabase databaseConnection;

        private readonly IReadOnlyList<StorageModel<TestModel, TestMetadata>> storageModels =
            StorageModel.CreateMany(nameof(AddManyCommandTest), count: 3);

        private readonly IReadOnlyList<TestModel> testModels = TestModel.CreateMany(nameof(AddManyCommandTest), count: 3);
        private readonly TestReadModelDatabase testReadModelDatabase = new TestReadModelDatabase();

        public AddManyCommandTest()
        {
            databaseConnection = testReadModelDatabase.CreateConnection();
        }

        [TearDown]
        public void CleanUp()
        {
            Task.Run(() => databaseConnection.DropCollectionAsync("TestModel")).Wait();
        }

        [Test]
        public void Adds_records_to_database()
        {
            var addTestModels = Create.AddManyCommandAsync<TestReadModelDatabase, TestModel>(testReadModelDatabase);

            var search = databaseConnection.GetCollection<TestModel>("TestModel")
                .Aggregate()
                .Match(m => true);

            var addTask = Task.Run(() => addTestModels(testModels));
            addTask.Wait();

            var searchTask = Task.Run(() => search.ToListAsync());
            searchTask.Wait();

            var results = searchTask.Result;
            Assert.That(results.Count, Is.EqualTo(testModels.Count));
            foreach (var testModel in testModels)
            {
                results.Should().Contain(m => m.Id == testModel.Id && m.Name == testModel.Name);
            }
        }

        [Test]
        public void Adds_record_to_database_with_custom_metadata()
        {
            var addTestModels = Create.AddManyCommandAsync<TestReadModelDatabase, TestModel, TestMetadata>(testReadModelDatabase);

            var search = databaseConnection.GetCollection<StorageModel<TestModel, TestMetadata>>("TestModel")
                .Aggregate()
                .Match(m => true);

            var addTask = Task.Run(() => addTestModels(storageModels));
            addTask.Wait();

            var searchTask = Task.Run(() => search.ToListAsync());
            searchTask.Wait();

            var results = searchTask.Result;
            Assert.That(results.Count, Is.EqualTo(storageModels.Count));
            foreach (var storageModel in storageModels)
            {
                results.Should()
                    .Contain(
                        m =>
                            m.Model.Id == storageModel.Model.Id && m.Model.Name == storageModel.Model.Name &&
                            m.Metadata.FirstName == storageModel.Metadata.FirstName && m.Metadata.LastName == storageModel.Metadata.LastName);
            }
        }
    }
}
