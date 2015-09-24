// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectAllTestBase.cs">
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

    public abstract class ProjectAllTestBase
    {
        protected ITestDatabase Database { get; set; }
        protected IReadOnlyCollection<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }
        protected IReadOnlyCollection<TestModel> TestModels { get; set; }

        [Test]
        public void Gets_empty_results_when_no_data_present()
        {
            var getTask = Database.ModelQueries.ProjectAllAsync(m => m.Name);
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Gets_empty_results_when_no_data_present_with_custom_metadata()
        {
            var getTask =
                Database.StorageModelQueries.ProjectAllAsync(sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Gets_expected_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var getTask = Database.ModelQueries.ProjectAllAsync(m => m.Name);
            getTask.Wait();

            var results = getTask.Result.ToList();
            results.Should().Contain(TestModels.Select(m => m.Name));
        }

        [Test]
        public void Gets_expected_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var getTask =
                Database.StorageModelQueries.ProjectAllAsync(sm => new { sm.Model.Name, sm.Metadata.FirstName, sm.Metadata.LastName });
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
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => Database.ModelQueries.ProjectAllAsync((Expression<Func<TestModel, ProjectAllTestBase>>)null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () =>
                    Task.Run(
                        () =>
                            Database.StorageModelQueries.ProjectAllAsync(
                                (Expression<Func<StorageModel<TestModel, TestMetadata>, ProjectAllTestBase>>)null))
                        .Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
