// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateOneTest.cs">
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
    public class UpdateOneTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();
        private IReadOnlyList<StorageModel<TestModel, TestMetadata>> storageModels;
        private IReadOnlyList<TestModel> testModels;

        [SetUp]
        public void Setup()
        {
            storageModels = StorageModel.CreateMany(nameof(UpdateOneTest), count: 3);
            testModels = TestModel.CreateMany(nameof(UpdateOneTest), count: 3);
        }

        [TearDown]
        public void CleanUp()
        {
            var databaseConnection = database.CreateConnection();
            Task.Run(() => databaseConnection.DropCollectionAsync(nameof(TestModel))).Wait();
        }

        [Test]
        public void Updates_when_model_exists_in_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var updatedModel = testModels[1];
            updatedModel.Name = "Updated name";

            var updateTask = database.UpdateOneTestModel(updatedModel);
            updateTask.Wait();

            var getTask = database.GetOneTestModel(m => m.Id == updatedModel.Id);
            getTask.Wait();

            var result = getTask.Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(updatedModel.Name));
        }

        [Test]
        public void Updates_when_model_exists_in_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var updatedModel = storageModels[1];
            updatedModel.Model.Name = "Updated name";
            updatedModel.Metadata.FirstName = "Updated first name";
            updatedModel.Metadata.LastName = "Updated last name";

            var updateTask = database.UpdateOneStorageModel(updatedModel.Model, updatedModel.Metadata);
            updateTask.Wait();

            var getTask = database.GetOneStorageModel(m => m.Model.Id == updatedModel.Model.Id);
            getTask.Wait();

            var result = getTask.Result;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(updatedModel.Model.Name));
        }

        [Test]
        public void Does_not_overwrite_other_records()
        {
            database.AddTestModelsToDatabase(testModels);

            var updatedModel = testModels[1];
            updatedModel.Name = "Updated name";

            var updateTask = database.UpdateOneTestModel(updatedModel);
            updateTask.Wait();

            var getTask = database.GetManyTestModels(m => m.Id != updatedModel.Id);
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, testModels.Where(m => m.Id != updatedModel.Id).ToList());
        }

        [Test]
        public void Does_not_overwrite_other_records_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var updatedModel = storageModels[1];
            updatedModel.Model.Name = "Updated name";
            updatedModel.Metadata.FirstName = "Updated first name";
            updatedModel.Metadata.LastName = "Updated last name";

            var updateTask = database.UpdateOneStorageModel(updatedModel.Model, updatedModel.Metadata);
            updateTask.Wait();

            var getTask = database.GetManyStorageModels(m => m.Model.Id != updatedModel.Model.Id);
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, storageModels.Where(m => m.Model.Id != updatedModel.Model.Id).ToList());
        }

        [Test]
        public void Throws_when_model_does_not_exist_in_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var updatedModel = new TestModel("Updated name", Guid.NewGuid());

            Assert.That(
                () => Task.Run(() => database.UpdateOneTestModel(updatedModel)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<DatabaseException>());
        }

        [Test]
        public void Throws_when_model_does_not_exist_in_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var updatedModel = new TestModel("Updated name", Guid.NewGuid());
            var updatedMetadata = new TestMetadata("Updated name");

            Assert.That(
                () => Task.Run(() => database.UpdateOneStorageModel(updatedModel, updatedMetadata)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<DatabaseException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Commands.UpdateOneAsync<TestReadModelDatabase, TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Commands.UpdateOneAsync<TestReadModelDatabase, TestModel, TestMetadata>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => database.UpdateOneTestModel(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => database.UpdateOneStorageModel(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
