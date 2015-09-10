// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectAllTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ProjectAllTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();

        private readonly IReadOnlyCollection<StorageModel<TestModel, TestMetadata>> storageModels =
            StorageModel.CreateMany(nameof(ProjectAllTest), count: 5);

        private readonly IReadOnlyCollection<TestModel> testModels = TestModel.CreateMany(nameof(ProjectAllTest), count: 5);

        [TearDown]
        public void CleanUp()
        {
            var databaseConnection = database.CreateConnection();
            Task.Run(() => databaseConnection.DropCollectionAsync(nameof(TestModel))).Wait();
        }

        [Test]
        public void Gets_empty_results_when_no_data_present()
        {
            var getTask = database.ProjectAllTestModels(m => m.Name);
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Gets_empty_results_when_no_data_present_with_custom_metadata()
        {
            var getTask = database.ProjectAllStorageModels(sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Gets_expected_results_from_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var getTask = database.ProjectAllTestModels(m => m.Name);
            getTask.Wait();

            var results = getTask.Result.ToList();
            results.Should().Contain(testModels.Select(m => m.Name));
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var getTask = database.ProjectAllStorageModels(sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
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
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Queries.ProjectAllAsync<TestReadModelDatabase, TestModel, ProjectAllTest>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Queries.ProjectAllAsync<TestReadModelDatabase, TestModel, TestMetadata, ProjectAllTest>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => database.ProjectAllTestModels((Expression<Func<TestModel, ProjectAllTest>>)null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () =>
                    Task.Run(
                        () =>
                            database.ProjectAllStorageModels((Expression<Func<StorageModel<TestModel, TestMetadata>, ProjectAllTest>>)null))
                        .Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
