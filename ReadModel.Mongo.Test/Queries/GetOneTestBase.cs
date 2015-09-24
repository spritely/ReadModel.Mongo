// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetOneTestBase.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NUnit.Framework;

    public abstract class GetOneTestBase
    {
        protected ITestDatabase Database { get; set; }
        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }
        protected IReadOnlyList<TestModel> TestModels { get; set; }
        protected string TestStorageName { get; set; }

        [Test]
        public void Gets_expected_result_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.GetOneAsync(m => m.Id == TestModels[1].Id);
            getTask.Wait();

            AssertResult.Matches(getTask.Result, TestModels[1]);
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.GetOneAsync(m => m.Model.Id == StorageModels[1].Model.Id);
            getTask.Wait();

            AssertResult.Matches(getTask.Result, StorageModels[1]);
        }

        [Test]
        public void Gets_empty_result_when_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.GetOneAsync(m => m.Id == Guid.NewGuid());
            getTask.Wait();

            Assert.That(getTask.Result, Is.Null);
        }

        [Test]
        public void Gets_empty_result_when_querying_for_nonexistent_data_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.GetOneAsync(m => m.Model.Id == Guid.NewGuid());
            getTask.Wait();

            Assert.That(getTask.Result, Is.Null);
        }

        [Test]
        public void Throws_when_querying_for_multiple_records()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.GetOneAsync(m => m.Name.StartsWith(TestStorageName));
            Assert.That(() => getTask.Wait(), Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Throws_when_querying_for_multiple_records_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.GetOneAsync(m => m.Model.Name.StartsWith(TestStorageName));
            Assert.That(() => getTask.Wait(), Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => Database.ModelQueries.GetOneAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => Database.StorageModelQueries.GetOneAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
