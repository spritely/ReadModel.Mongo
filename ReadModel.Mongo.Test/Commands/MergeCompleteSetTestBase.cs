﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeCompleteSetTestBase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
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

    public abstract class MergeCompleteSetTestBase
    {
        protected ITestDatabase Database { get; set; }
        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }
        protected IReadOnlyList<TestModel> TestModels { get; set; }

        [Test]
        public void Adds_when_model_does_not_exist_in_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var newModels = new[]
            {
                new TestModel("New name 1", Guid.NewGuid()),
                new TestModel("New name 2", Guid.NewGuid())
            };

            var models = newModels.ToDictionary(m => m.Id, m => m);

            var mergeTask = Database.ModelCommands.MergeCompleteSetAsync(models);
            mergeTask.Wait();

            var getTask = Database.ModelQueries.GetManyAsync(m => models.Keys.Contains(m.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, newModels);
        }

        [Test]
        public void Adds_when_model_does_not_exist_in_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var newModels = StorageModel.CreateMany("New models", count: 2);
            var models = newModels.ToDictionary(m => m.Model.Id, m => m);

            var mergeTask = Database.StorageModelCommands.MergeCompleteSetAsync(models);
            mergeTask.Wait();

            var getTask = Database.StorageModelQueries.GetManyAsync(m => models.Keys.Contains(m.Model.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, newModels);
        }

        [Test]
        public void Updates_each_when_models_exist_in_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            TestModels[1].Name = "Updated name 1";
            TestModels[2].Name = "Updated name 2";

            var models = TestModels.Skip(1).Take(2).ToDictionary(m => m.Id, m => m);
            var mergeTask = Database.ModelCommands.MergeCompleteSetAsync(models);
            mergeTask.Wait();

            var getTask = Database.ModelQueries.GetManyAsync(m => models.Keys.Contains(m.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels.Where(m => models.Keys.Contains(m.Id)).ToList());
        }

        [Test]
        public void Updates_each_when_models_exist_in_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            StorageModels[1].Model.Name = "Updated name 1";
            StorageModels[1].Metadata.FirstName = "Updated first name 1";
            StorageModels[1].Metadata.LastName = "Updated last name 1";
            StorageModels[2].Model.Name = "Updated name 2";
            StorageModels[2].Metadata.FirstName = "Updated first name 2";
            StorageModels[2].Metadata.LastName = "Updated last name 2";

            var models = StorageModels.Skip(1).Take(2).ToDictionary(m => m.Model.Id, m => m);
            var mergeTask = Database.StorageModelCommands.MergeCompleteSetAsync(models);
            mergeTask.Wait();

            var getTask = Database.StorageModelQueries.GetManyAsync(m => models.Keys.Contains(m.Model.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels.Where(m => models.Keys.Contains(m.Model.Id)).ToList());
        }

        [Test]
        public void Removes_other_records()
        {
            Database.AddTestModelsToDatabase(TestModels);

            TestModels[1].Name = "Updated name 1";
            TestModels[2].Name = "Updated name 2";

            var models = TestModels.Skip(1).Take(2).ToDictionary(m => m.Id, m => m);
            var mergeTask = Database.ModelCommands.MergeCompleteSetAsync(models);
            mergeTask.Wait();

            var getTask = Database.ModelQueries.GetManyAsync(m => !models.Keys.Contains(m.Id));
            getTask.Wait();

            getTask.Result.Should().BeEmpty();
        }

        [Test]
        public void Removes_other_records_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            StorageModels[1].Model.Name = "Updated name 1";
            StorageModels[1].Metadata.FirstName = "Updated first name 1";
            StorageModels[1].Metadata.LastName = "Updated last name 1";
            StorageModels[2].Model.Name = "Updated name 2";
            StorageModels[2].Metadata.FirstName = "Updated first name 2";
            StorageModels[2].Metadata.LastName = "Updated last name 2";

            var models = StorageModels.Skip(1).Take(2).ToDictionary(m => m.Model.Id, m => m);
            var mergeTask = Database.StorageModelCommands.MergeCompleteSetAsync(models);
            mergeTask.Wait();

            var getTask = Database.StorageModelQueries.GetManyAsync(m => !models.Keys.Contains(m.Model.Id));
            getTask.Wait();

            getTask.Result.Should().BeEmpty();
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => Database.ModelCommands.MergeCompleteSetAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => Database.StorageModelCommands.MergeCompleteSetAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
