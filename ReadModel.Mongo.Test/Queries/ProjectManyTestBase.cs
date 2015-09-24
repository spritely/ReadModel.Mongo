// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectManyTestBase.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;

    public abstract class ProjectManyTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        protected string TestStorageName { get; set; }

        [Test]
        public void Gets_expected_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectManyAsync(m => m.Name.StartsWith(TestStorageName), m => m.Name);
            getTask.Wait();

            var results = getTask.Result.ToList();
            results.Should().Contain(TestModels.Select(m => m.Name));
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.ProjectManyAsync(
                m => m.Model.Name.StartsWith(TestStorageName),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.Wait();

            var results = getTask.Result.ToList();
            foreach (var storageModel in StorageModels)
            {
                var result = results.Single(m => m.Name == storageModel.Model.Name);
                result.FirstName.Should().Be(storageModel.Metadata.FirstName);
                result.LastName.Should().Be(storageModel.Metadata.LastName);
            }
        }

        [Test]
        public void Gets_expected_subset_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var ids = TestModels.Skip(1).Take(3).Select(m => m.Id);
            var getTask = Database.ModelQueries.ProjectManyAsync(m => ids.Contains(m.Id), m => m.Name);
            getTask.Wait();

            var results = getTask.Result.ToList();
            results.Should().Contain(TestModels.Skip(1).Take(3).Select(m => m.Name));
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling",
            Justification = "Despite the class coupling this code isn't overly complex.")]
        [Test]
        public void Gets_expected_subset_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var ids = StorageModels.Skip(1).Take(3).Select(m => m.Model.Id);
            var getTask = Database.StorageModelQueries.ProjectManyAsync(
                m => ids.Contains(m.Model.Id),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.Wait();

            var results = getTask.Result.ToList();
            foreach (var storageModel in StorageModels.Skip(1).Take(3))
            {
                var result = results.Single(m => m.Name == storageModel.Model.Name);
                result.FirstName.Should().Be(storageModel.Metadata.FirstName);
                result.LastName.Should().Be(storageModel.Metadata.LastName);
            }
        }

        [Test]
        public void Gets_empty_results_with_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectManyAsync(m => m.Id == Guid.NewGuid(), m => m);
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Gets_empty_results_with_querying_for_nonexistent_data_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.ProjectManyAsync(m => m.Model.Id == Guid.NewGuid(), m => m);
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => Database.ModelQueries.ProjectManyAsync(null, m => m.Name)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());

            Assert.That(
                () => Task.Run(() => Database.ModelQueries.ProjectManyAsync<ProjectManyTestBase>(m => true, null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => Database.StorageModelQueries.ProjectManyAsync(null, m => m.Model.Name)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());

            Assert.That(
                () => Task.Run(() => Database.StorageModelQueries.ProjectManyAsync<ProjectManyTestBase>(m => true, null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
