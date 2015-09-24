// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveOneTestBase.cs">
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

    public abstract class RemoveOneTestBase
    {
        protected ITestDatabase Database { get; set; }
        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }
        protected IReadOnlyList<TestModel> TestModels { get; set; }

        [Test]
        public void Removes_expected_result_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var removeTask = Database.ModelCommands.RemoveOneAsync(TestModels[1]);
            removeTask.Wait();

            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels.Where(m => m.Id != TestModels[1].Id).ToList());
        }

        [Test]
        public void Removes_expected_result_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var removeTask = Database.StorageModelCommands.RemoveOneAsync(StorageModels[1].Model);
            removeTask.Wait();

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels.Where(m => m.Model.Id != StorageModels[1].Model.Id).ToList());
        }

        [Test]
        public void Remove_does_nothing_when_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var nonExistent = new TestModel("me no exist", Guid.NewGuid());
            var removeTask = Database.ModelCommands.RemoveOneAsync(nonExistent);
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

            var nonExistent = new TestModel("me no exist", Guid.NewGuid());
            var removeTask = Database.StorageModelCommands.RemoveOneAsync(nonExistent);
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
                () => Task.Run(() => Database.ModelCommands.RemoveOneAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => Database.StorageModelCommands.RemoveOneAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
