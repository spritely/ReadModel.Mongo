// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveManyTest.cs">
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
    public class RemoveManyTest
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
        public void Removes_all_expected_results_from_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var removeTask = database.RemoveManyTestModels(m => m.Name.StartsWith(nameof(RemoveManyTest)));
            removeTask.Wait();

            var getTask = database.GetAllTestModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Removes_all_expected_results_from_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var removeTask = database.RemoveManyStorageModels(m => m.Model.Name.StartsWith(nameof(RemoveManyTest)));
            removeTask.Wait();

            var getTask = database.GetAllStorageModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Removes_expected_subset_results_from_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var ids = testModels.Skip(2).Take(3).Select(m => m.Id);
            var removeTask = database.RemoveManyTestModels(m => ids.Contains(m.Id));
            removeTask.Wait();

            var getTask = database.GetAllTestModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, testModels.Take(2).ToList());
        }

        [Test]
        public void Removes_expected_subset_results_from_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var ids = storageModels.Skip(2).Take(3).Select(m => m.Model.Id);
            var removeTask = database.RemoveManyStorageModels(m => ids.Contains(m.Model.Id));
            removeTask.Wait();

            var getTask = database.GetAllStorageModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, storageModels.Take(2).ToList());
        }

        [Test]
        public void Remove_does_nothing_when_querying_for_nonexistent_data()
        {
            database.AddTestModelsToDatabase(testModels);

            var removeTask = database.RemoveManyTestModels(m => m.Id == Guid.NewGuid());
            removeTask.Wait();

            var getTask = database.GetAllTestModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, testModels);
        }

        [Test]
        public void Remove_does_nothing_when_querying_for_nonexistent_data_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var removeTask = database.RemoveManyStorageModels(m => m.Model.Id == Guid.NewGuid());
            removeTask.Wait();

            var getTask = database.GetAllStorageModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, storageModels);
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Commands.RemoveManyAsync<TestReadModelDatabase, TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Commands.RemoveManyAsync<TestReadModelDatabase, TestModel, TestMetadata>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => database.RemoveManyTestModels(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => database.RemoveManyStorageModels(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
