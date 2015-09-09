// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetOneTest.cs">
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
    using Spritely.Cqrs;

    [TestFixture]
    public class GetOneTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();

        private readonly IReadOnlyList<StorageModel<TestModel, TestMetadata>> storageModels =
            StorageModel.CreateMany(nameof(GetOneTest), count: 3);

        private readonly IReadOnlyList<TestModel> testModels = TestModel.CreateMany(nameof(GetOneTest), count: 3);

        [TearDown]
        public void CleanUp()
        {
            var databaseConnection = database.CreateConnection();
            Task.Run(() => databaseConnection.DropCollectionAsync(nameof(TestModel))).Wait();
        }

        [Test]
        public void Gets_expected_result_from_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var getTask = database.GetOneTestModel(m => m.Id == testModels[1].Id);
            getTask.Wait();
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.GetOneStorageModel(m => m.Model.Id == storageModels[1].Model.Id);
            getTask.Wait();

            AssertResult.Matches(getTask.Result, storageModels[1]);
        }

        [Test]
        public void Gets_empty_result_when_querying_for_nonexistent_data()
        {
            database.AddTestModelsToDatabase(testModels);

            var getTask = database.GetOneTestModel(m => m.Id == Guid.NewGuid());
            getTask.Wait();

            Assert.That(getTask.Result, Is.Null);
        }

        [Test]
        public void Gets_empty_result_when_querying_for_nonexistent_data_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.GetOneStorageModel(m => m.Model.Id == Guid.NewGuid());
            getTask.Wait();

            Assert.That(getTask.Result, Is.Null);
        }

        [Test]
        public void Throws_when_querying_for_multiple_records()
        {
            database.AddTestModelsToDatabase(testModels);

            var getTask = database.GetOneTestModel(m => m.Name.StartsWith(nameof(GetOneTest)));
            Assert.That(() => getTask.Wait(), Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Throws_when_querying_for_multiple_records_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.GetOneStorageModel(m => m.Model.Name.StartsWith(nameof(GetOneTest)));
            Assert.That(() => getTask.Wait(), Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Queries.GetOneAsync<TestReadModelDatabase, TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Queries.GetOneAsync<TestReadModelDatabase, TestModel, TestMetadata>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => database.GetOneTestModel(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => database.GetOneStorageModel(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
