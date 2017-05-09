// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrUpdateOneTestBase.cs">
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
    using NUnit.Framework;

    public abstract class AddOrUpdateOneTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        [Test]
        public void Adds_when_model_does_not_exist_in_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var model = new TestModel("New name", Guid.NewGuid());

            var addOrUpdateTask = Database.ModelCommands.AddOrUpdateOneAsync(model);
            addOrUpdateTask.Wait();

            var getTask = Database.ModelQueries.GetOneAsync(m => m.Id == model.Id);
            getTask.Wait();

            var result = getTask.Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(model.Name));
        }

        [Test]
        public void Adds_when_model_does_not_exist_in_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var model = new TestModel("New name", Guid.NewGuid());
            var metadata = new TestMetadata("New name");

            var addOrUpdateTask = Database.StorageModelCommands.AddOrUpdateOneAsync(model, metadata);
            addOrUpdateTask.Wait();

            var getTask = Database.StorageModelQueries.GetOneAsync(m => m.Model.Id == model.Id);
            getTask.Wait();

            var result = getTask.Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(model.Name));
        }

        [Test]
        public void Updates_when_model_exists_in_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var updatedModel = TestModels[1];
            updatedModel.Name = "Updated name";

            var addOrUpdateTask = Database.ModelCommands.AddOrUpdateOneAsync(updatedModel);
            addOrUpdateTask.Wait();

            var getTask = Database.ModelQueries.GetOneAsync(m => m.Id == updatedModel.Id);
            getTask.Wait();

            var result = getTask.Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(updatedModel.Name));
        }

        [Test]
        public void Updates_when_model_exists_in_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var updatedModel = StorageModels[1];
            updatedModel.Model.Name = "Updated name";
            updatedModel.Metadata.FirstName = "Updated first name";
            updatedModel.Metadata.LastName = "Updated last name";

            var addOrUpdateTask = Database.StorageModelCommands.AddOrUpdateOneAsync(updatedModel.Model, updatedModel.Metadata);
            addOrUpdateTask.Wait();

            var getTask = Database.StorageModelQueries.GetOneAsync(m => m.Model.Id == updatedModel.Model.Id);
            getTask.Wait();

            var result = getTask.Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(updatedModel.Model.Name));
        }

        [Test]
        public void Does_not_overwrite_other_records()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var updatedModel = TestModels[1];
            updatedModel.Name = "Updated name";

            var addOrUpdateTask = Database.ModelCommands.AddOrUpdateOneAsync(updatedModel);
            addOrUpdateTask.Wait();

            var getTask = Database.ModelQueries.GetManyAsync(m => m.Id != updatedModel.Id);
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels.Where(m => m.Id != updatedModel.Id).ToList());
        }

        [Test]
        public void Does_not_overwrite_other_records_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var updatedModel = StorageModels[1];
            updatedModel.Model.Name = "Updated name";
            updatedModel.Metadata.FirstName = "Updated first name";
            updatedModel.Metadata.LastName = "Updated last name";

            var addOrUpdateTask = Database.StorageModelCommands.AddOrUpdateOneAsync(updatedModel.Model, updatedModel.Metadata);
            addOrUpdateTask.Wait();

            var getTask = Database.StorageModelQueries.GetManyAsync(m => m.Model.Id != updatedModel.Model.Id);
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels.Where(m => m.Model.Id != updatedModel.Model.Id).ToList());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => Database.ModelCommands.AddOrUpdateOneAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => Database.StorageModelCommands.AddOrUpdateOneAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
