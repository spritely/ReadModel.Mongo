// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveManyTestBase.cs">
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

    public abstract class RemoveManyTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        protected string TestStorageName { get; set; }

        [Test]
        public void Removes_all_expected_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var removeTask = Database.ModelCommands.RemoveManyAsync(m => m.Name.StartsWith(TestStorageName));
            removeTask.Wait();

            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Removes_all_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var removeTask = Database.StorageModelCommands.RemoveManyAsync(m => m.Model.Name.StartsWith(TestStorageName));
            removeTask.Wait();

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Removes_expected_subset_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var ids = TestModels.Skip(2).Take(3).Select(m => m.Id);
            var removeTask = Database.ModelCommands.RemoveManyAsync(m => ids.Contains(m.Id));
            removeTask.Wait();

            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels.Take(2).ToList());
        }

        [Test]
        public void Removes_expected_subset_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var ids = StorageModels.Skip(2).Take(3).Select(m => m.Model.Id);
            var removeTask = Database.StorageModelCommands.RemoveManyAsync(m => ids.Contains(m.Model.Id));
            removeTask.Wait();

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels.Take(2).ToList());
        }

        [Test]
        public void Remove_does_nothing_when_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var removeTask = Database.ModelCommands.RemoveManyAsync(m => m.Id == Guid.NewGuid());
            removeTask.Wait();

            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels);
        }

        [Test]
        public void Remove_does_nothing_when_querying_for_nonexistent_data_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var removeTask = Database.StorageModelCommands.RemoveManyAsync(m => m.Model.Id == Guid.NewGuid());
            removeTask.Wait();

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels);
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => Database.ModelCommands.RemoveManyAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => Database.StorageModelCommands.RemoveManyAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
