// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectManyTest.cs">
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
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ProjectManyTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();

        private readonly IReadOnlyList<StorageModel<TestModel, TestMetadata>> storageModels =
            StorageModel.CreateMany(nameof(ProjectManyTest), count: 5);

        private readonly IReadOnlyList<TestModel> testModels = TestModel.CreateMany(nameof(ProjectManyTest), count: 5);

        [TearDown]
        public void CleanUp()
        {
            var databaseConnection = database.CreateConnection();
            Task.Run(() => databaseConnection.DropCollectionAsync(nameof(TestModel))).Wait();
        }

        [Test]
        public void Gets_expected_results_from_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var getTask = database.ProjectManyTestModels(m => m.Name.StartsWith(nameof(ProjectManyTest)), m => m.Name);
            getTask.Wait();

            var results = getTask.Result.ToList();
            results.Should().Contain(testModels.Select(m => m.Name));
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.ProjectManyStorageModels(
                m => m.Model.Name.StartsWith(nameof(ProjectManyTest)),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.Wait();

            var results = getTask.Result.ToList();
            foreach (var storageModel in storageModels)
            {
                var result = results.Single(m => m.Name == storageModel.Model.Name);
                result.FirstName.Should().Be(storageModel.Metadata.FirstName);
                result.LastName.Should().Be(storageModel.Metadata.LastName);
            }
        }

        [Test]
        public void Gets_expected_subset_results_from_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var ids = testModels.Skip(1).Take(3).Select(m => m.Id);
            var getTask = database.ProjectManyTestModels(m => ids.Contains(m.Id), m => m.Name);
            getTask.Wait();

            var results = getTask.Result.ToList();
            results.Should().Contain(testModels.Skip(1).Take(3).Select(m => m.Name));
        }

        [Test]
        public void Gets_expected_subset_results_from_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var ids = storageModels.Skip(1).Take(3).Select(m => m.Model.Id);
            var getTask = database.ProjectManyStorageModels(
                m => ids.Contains(m.Model.Id),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.Wait();

            var results = getTask.Result.ToList();
            foreach (var storageModel in storageModels.Skip(1).Take(3))
            {
                var result = results.Single(m => m.Name == storageModel.Model.Name);
                result.FirstName.Should().Be(storageModel.Metadata.FirstName);
                result.LastName.Should().Be(storageModel.Metadata.LastName);
            }
        }

        [Test]
        public void Gets_empty_results_with_querying_for_nonexistent_data()
        {
            database.AddTestModelsToDatabase(testModels);

            var getTask = database.ProjectManyTestModels(m => m.Id == Guid.NewGuid(), m => m);
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Gets_empty_results_with_querying_for_nonexistent_data_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.ProjectManyStorageModels(m => m.Model.Id == Guid.NewGuid(), m => m);
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Queries.ProjectManyAsync<TestReadModelDatabase, TestModel, ProjectManyTest>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Queries.ProjectManyAsync<TestReadModelDatabase, TestModel, TestMetadata, ProjectManyTest>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => database.ProjectManyTestModels(null, m => m.Name)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());

            Assert.That(
                () => Task.Run(() => database.ProjectManyTestModels<ProjectManyTest>(m => true, null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => database.ProjectManyStorageModels(null, m => m.Model.Name)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());

            Assert.That(
                () => Task.Run(() => database.ProjectManyStorageModels<ProjectManyTest>(m => true, null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
