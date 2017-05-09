// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectOneTestBase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
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

    public abstract class ProjectOneTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        protected string TestStorageName { get; set; }

        [Test]
        public void Gets_expected_result_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectOneAsync(m => m.Id == TestModels[1].Id, m => m.Name);
            getTask.Wait();

            getTask.Result.Should().Be(TestModels[1].Name);
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.ProjectOneAsync(
                sm => sm.Model.Id == StorageModels[1].Model.Id,
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.Wait();

            var result = getTask.Result;
            result.Name.Should().Be(StorageModels[1].Model.Name);
            result.FirstName.Should().Be(StorageModels[1].Metadata.FirstName);
            result.LastName.Should().Be(StorageModels[1].Metadata.LastName);
        }

        [Test]
        public void Gets_empty_result_when_querying_for_nonexistent_data()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectOneAsync(m => m.Id == Guid.NewGuid(), m => m.Name);
            getTask.Wait();

            Assert.That(getTask.Result, Is.Null);
        }

        [Test]
        public void Gets_empty_result_when_querying_for_nonexistent_data_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.ProjectOneAsync(
                sm => sm.Model.Id == Guid.NewGuid(),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.Wait();

            Assert.That(getTask.Result, Is.Null);
        }

        [Test]
        public void Throws_when_querying_for_multiple_records()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectOneAsync(m => m.Name.StartsWith(TestStorageName), m => m.Name);
            Assert.That(() => getTask.Wait(), Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Throws_when_querying_for_multiple_records_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask = Database.StorageModelQueries.ProjectOneAsync(
                sm => sm.Model.Name.StartsWith(TestStorageName),
                sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            Assert.That(() => getTask.Wait(), Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => Database.ModelQueries.ProjectOneAsync(null, m => m.Name)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());

            Assert.That(
                () => Task.Run(() => Database.ModelQueries.ProjectOneAsync<ProjectManyTestBase>(m => true, null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => Database.StorageModelQueries.ProjectOneAsync(null, m => m.Model.Name)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());

            Assert.That(
                () => Task.Run(() => Database.StorageModelQueries.ProjectOneAsync<ProjectManyTestBase>(m => true, null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
