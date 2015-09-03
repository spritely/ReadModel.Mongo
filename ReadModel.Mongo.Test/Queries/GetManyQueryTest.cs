// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetManyQueryTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MongoDB.Driver;
    using NUnit.Framework;

    [TestFixture]
    public class GetManyQueryTest
    {
        private readonly IMongoDatabase databaseConnection;

        private readonly IReadOnlyList<StorageModel<TestModel, TestMetadata>> storageModels =
            StorageModel.CreateMany(nameof(GetManyQueryTest), count: 5);

        private readonly IReadOnlyList<TestModel> testModels = TestModel.CreateMany(nameof(GetManyQueryTest), count: 5);
        private readonly TestReadModelDatabase testReadModelDatabase = new TestReadModelDatabase();

        public GetManyQueryTest()
        {
            databaseConnection = testReadModelDatabase.CreateConnection();
        }

        [TearDown]
        public void CleanUp()
        {
            Task.Run(() => databaseConnection.DropCollectionAsync("TestModel")).Wait();
        }

        [Test]
        public void Gets_expected_results_from_database()
        {
            var addTestModels = Create.AddManyCommandAsync<TestReadModelDatabase, TestModel>(testReadModelDatabase);

            var addTask = Task.Run(() => addTestModels(testModels));
            addTask.Wait();

            var getTestModels = Create.GetManyQueryAsync<TestReadModelDatabase, TestModel>(testReadModelDatabase);

            var getTask = getTestModels(m => m.Name.StartsWith(nameof(GetManyQueryTest)));
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results.Count, Is.EqualTo(testModels.Count));
            foreach (var testModel in testModels)
            {
                results.Should().Contain(m => m.Id == testModel.Id && m.Name == testModel.Name);
            }
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            var addTestModels = Create.AddManyCommandAsync<TestReadModelDatabase, TestModel, TestMetadata>(testReadModelDatabase);

            var addTask = Task.Run(() => addTestModels(storageModels));
            addTask.Wait();

            var getTestModels = Create.GetManyQueryAsync<TestReadModelDatabase, TestModel, TestMetadata>(testReadModelDatabase);

            var getTask = getTestModels(m => m.Model.Name.StartsWith(nameof(GetManyQueryTest)));
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results.Count, Is.EqualTo(storageModels.Count));
            foreach (var storageModel in storageModels)
            {
                results.Should().Contain(m => m.Id == storageModel.Model.Id && m.Name == storageModel.Model.Name);
            }
        }
    }
}
