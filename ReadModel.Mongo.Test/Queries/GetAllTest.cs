// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllTest.cs">
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
    public class GetAllTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();

        private readonly IReadOnlyCollection<StorageModel<TestModel, TestMetadata>> storageModels =
            StorageModel.CreateMany(nameof(GetAllTest), count: 5);

        private readonly IReadOnlyCollection<TestModel> testModels = TestModel.CreateMany(nameof(GetAllTest), count: 5);

        [TearDown]
        public void CleanUp()
        {
            var databaseConnection = database.CreateConnection();
            Task.Run(() => databaseConnection.DropCollectionAsync("TestModel")).Wait();
        }

        [Test]
        public void Gets_empty_results_when_no_data_present()
        {
            var getTask = database.GetAllTestModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Gets_empty_results_when_no_data_present_with_custom_metadata()
        {
            var getTask = database.GetAllStorageModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Gets_expected_results_from_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var getTask = database.GetAllTestModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, testModels);
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.GetAllStorageModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, storageModels);
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Queries.GetAllAsync<TestReadModelDatabase, TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Queries.GetAllAsync<TestReadModelDatabase, TestModel, TestMetadata>(null),
                Throws.TypeOf<ArgumentNullException>());
        }
    }
}
