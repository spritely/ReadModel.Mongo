// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveOneTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class RemoveOneTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();

        private readonly IReadOnlyList<StorageModel<TestModel, TestMetadata>> storageModels =
            StorageModel.CreateMany(nameof(RemoveManyTest), count: 5);

        private readonly IReadOnlyList<TestModel> testModels = TestModel.CreateMany(nameof(RemoveManyTest), count: 5);

        [TearDown]
        public void CleanUp()
        {
            var databaseConnection = database.CreateConnection();
            Task.Run(() => databaseConnection.DropCollectionAsync(nameof(TestModel))).Wait();
        }

        [Test]
        public void Removes_expected_result_from_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var removeTask = database.RemoveOneTestModel(testModels[1]);
            removeTask.Wait();

            var getTask = database.GetAllTestModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, testModels.Where(m => m.Id != testModels[1].Id).ToList());
        }

        [Test]
        public void Removes_expected_result_from_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var removeTask = database.RemoveOneTestModel(storageModels[1].Model);
            removeTask.Wait();

            var getTask = database.GetAllStorageModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, storageModels.Where(m => m.Model.Id != storageModels[1].Model.Id).ToList());
        }

        [Test]
        public void Remove_does_nothing_when_querying_for_non_existent_data()
        {
            database.AddTestModelsToDatabase(testModels);

            var nonExistent = new TestModel("me no exist", Guid.NewGuid());
            var removeTask = database.RemoveOneTestModel(nonExistent);
            removeTask.Wait();

            var getTask = database.GetAllTestModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, testModels);
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Commands.RemoveOneAsync<TestReadModelDatabase, TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => database.RemoveOneTestModel(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
