// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectOneTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ProjectOneTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();

        private readonly IReadOnlyList<StorageModel<TestModel, TestMetadata>> storageModels =
            StorageModel.CreateMany(nameof(ProjectOneTest), count: 3);

        private readonly IReadOnlyList<TestModel> testModels = TestModel.CreateMany(nameof(ProjectOneTest), count: 3);

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

            var getTask = database.ProjectOneTestModel(m => m.Id == testModels[1].Id, m => m.Name);
            getTask.Wait();

            getTask.Result.Should().Be(testModels[1].Name);
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.ProjectOneStorageModel(
                sm => sm.Model.Id == storageModels[1].Model.Id,
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.Wait();

            var result = getTask.Result;
            result.Name.Should().Be(storageModels[1].Model.Name);
            result.FirstName.Should().Be(storageModels[1].Metadata.FirstName);
            result.LastName.Should().Be(storageModels[1].Metadata.LastName);
        }

        [Test]
        public void Gets_empty_result_when_querying_for_nonexistent_data()
        {
            database.AddTestModelsToDatabase(testModels);

            var getTask = database.ProjectOneTestModel(m => m.Id == Guid.NewGuid(), m => m.Name);
            getTask.Wait();

            Assert.That(getTask.Result, Is.Null);
        }

        [Test]
        public void Gets_empty_result_when_querying_for_nonexistent_data_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.ProjectOneStorageModel(
                sm => sm.Model.Id == Guid.NewGuid(),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.Wait();

            Assert.That(getTask.Result, Is.Null);
        }

        [Test]
        public void Throws_when_querying_for_multiple_records()
        {
            database.AddTestModelsToDatabase(testModels);

            var getTask = database.ProjectOneTestModel(m => m.Name.StartsWith(nameof(ProjectOneTest)), m => m.Name);
            Assert.That(() => getTask.Wait(), Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Throws_when_querying_for_multiple_records_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.ProjectOneStorageModel(
                sm => sm.Model.Name.StartsWith(nameof(ProjectOneTest)),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            Assert.That(() => getTask.Wait(), Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Queries.ProjectOneAsync<TestReadModelDatabase, TestModel, ProjectOneTest>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Queries.ProjectOneAsync<TestReadModelDatabase, TestModel, TestMetadata, ProjectOneTest>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => database.ProjectOneTestModel(null, m => m.Name)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());

            Assert.That(
                () => Task.Run(() => database.ProjectOneTestModel<ProjectManyTest>(m => true, null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => database.ProjectOneStorageModel(null, m => m.Model.Name)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());

            Assert.That(
                () => Task.Run(() => database.ProjectOneStorageModel<ProjectManyTest>(m => true, null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
