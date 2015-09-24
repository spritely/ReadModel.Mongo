// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetManyTestBase.cs">
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

    public abstract class GetManyTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        protected string TestStorageName { get; set; }

        [Test]
        public void Gets_expected_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.GetManyAsync(m => m.Name.StartsWith(TestStorageName));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels);
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.GetManyAsync(m => m.Model.Name.StartsWith(TestStorageName));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels);
        }

        [Test]
        public void Gets_expected_subset_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var ids = TestModels.Skip(1).Take(3).Select(m => m.Id);
            var getTask = Database.ModelQueries.GetManyAsync(m => ids.Contains(m.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels.Skip(1).Take(3).ToList());
        }

        [Test]
        public void Gets_expected_subset_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var ids = StorageModels.Skip(1).Take(3).Select(m => m.Model.Id);
            var getTask = Database.StorageModelQueries.GetManyAsync(m => ids.Contains(m.Model.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels.Skip(1).Take(3).ToList());
        }

        [Test]
        public void Gets_empty_results_with_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.GetManyAsync(m => m.Id == Guid.NewGuid());
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Gets_empty_results_with_querying_for_nonexistent_data_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.GetManyAsync(m => m.Model.Id == Guid.NewGuid());
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => Database.ModelQueries.GetManyAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => Database.StorageModelQueries.GetManyAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
