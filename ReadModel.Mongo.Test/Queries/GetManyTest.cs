// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetManyTest.cs">
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
    public class GetManyTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();

        private readonly IReadOnlyList<StorageModel<TestModel, TestMetadata>> storageModels =
            StorageModel.CreateMany(nameof(GetManyTest), count: 5);

        private readonly IReadOnlyList<TestModel> testModels = TestModel.CreateMany(nameof(GetManyTest), count: 5);

        [TearDown]
        public void CleanUp()
        {
            var databaseConnection = database.CreateConnection();
            Task.Run(() => databaseConnection.DropCollectionAsync("TestModel")).Wait();
        }

        [Test]
        public void Gets_expected_results_from_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var getTask = database.GetManyTestModels(m => m.Name.StartsWith(nameof(GetManyTest)));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, testModels);
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.GetManyStorageModels(m => m.Model.Name.StartsWith(nameof(GetManyTest)));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, storageModels);
        }

        [Test]
        public void Gets_expected_subset_results_from_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var ids = testModels.Skip(1).Take(3).Select(m => m.Id);
            var getTask = database.GetManyTestModels(m => ids.Contains(m.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, testModels.Skip(1).Take(3).ToList());
        }

        [Test]
        public void Gets_expected_subset_results_from_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var ids = storageModels.Skip(1).Take(3).Select(m => m.Model.Id);
            var getTask = database.GetManyStorageModels(m => ids.Contains(m.Model.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, storageModels.Skip(1).Take(3).ToList());
        }

        [Test]
        public void Gets_empty_results_with_querying_for_non_existent_data()
        {
            database.AddTestModelsToDatabase(testModels);

            var getTask = database.GetManyTestModels(m => m.Id == Guid.NewGuid());
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Gets_empty_results_with_querying_for_non_existent_data_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.GetManyStorageModels(m => m.Model.Id == Guid.NewGuid());
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Queries.GetManyAsync<TestReadModelDatabase, TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Queries.GetManyAsync<TestReadModelDatabase, TestModel, TestMetadata>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => database.GetManyTestModels(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => database.GetManyStorageModels(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
