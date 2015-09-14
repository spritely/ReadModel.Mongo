// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateManyTest.cs">
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
    public class UpdateManyTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();
        private IReadOnlyList<StorageModel<TestModel, TestMetadata>> storageModels;
        private IReadOnlyList<TestModel> testModels;

        [SetUp]
        public void Setup()
        {
            storageModels = StorageModel.CreateMany(nameof(UpdateManyTest), count: 5);
            testModels = TestModel.CreateMany(nameof(UpdateManyTest), count: 5);
        }

        [TearDown]
        public void CleanUp()
        {
            var databaseConnection = database.CreateConnection();
            Task.Run(() => databaseConnection.DropCollectionAsync(nameof(TestModel))).Wait();
        }

        [Test]
        public void Updates_each_when_models_exist_in_database()
        {
            database.AddTestModelsToDatabase(testModels);

            testModels[1].Name = "Updated name 1";
            testModels[2].Name = "Updated name 2";

            var models = testModels.Skip(1).Take(2).ToDictionary(m => m.Id, m => m);
            var updateTask = database.UpdateManyTestModels(models);
            updateTask.Wait();

            var getTask = database.GetManyTestModels(m => models.Keys.Contains(m.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, testModels.Where(m => models.Keys.Contains(m.Id)).ToList());
        }

        [Test]
        public void Updates_each_when_models_exist_in_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            storageModels[1].Model.Name = "Updated name 1";
            storageModels[1].Metadata.FirstName = "Updated first name 1";
            storageModels[1].Metadata.LastName = "Updated last name 1";
            storageModels[2].Model.Name = "Updated name 2";
            storageModels[2].Metadata.FirstName = "Updated first name 2";
            storageModels[2].Metadata.LastName = "Updated last name 2";

            var models = storageModels.Skip(1).Take(2).ToDictionary(m => m.Model.Id, m => m);
            var updateTask = database.UpdateManyStorageModels(models);
            updateTask.Wait();

            var getTask = database.GetManyStorageModels(m => models.Keys.Contains(m.Model.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, storageModels.Where(m => models.Keys.Contains(m.Model.Id)).ToList());
        }

        [Test]
        public void Does_not_overwrite_other_records()
        {
            database.AddTestModelsToDatabase(testModels);

            testModels[1].Name = "Updated name 1";
            testModels[2].Name = "Updated name 2";

            var models = testModels.Skip(1).Take(2).ToDictionary(m => m.Id, m => m);
            var updateTask = database.UpdateManyTestModels(models);
            updateTask.Wait();

            var getTask = database.GetManyTestModels(m => !models.Keys.Contains(m.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, testModels.Where(m => !models.Keys.Contains(m.Id)).ToList());
        }

        [Test]
        public void Does_not_overwrite_other_records_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            storageModels[1].Model.Name = "Updated name 1";
            storageModels[1].Metadata.FirstName = "Updated first name 1";
            storageModels[1].Metadata.LastName = "Updated last name 1";
            storageModels[2].Model.Name = "Updated name 2";
            storageModels[2].Metadata.FirstName = "Updated first name 2";
            storageModels[2].Metadata.LastName = "Updated last name 2";

            var models = storageModels.Skip(1).Take(2).ToDictionary(m => m.Model.Id, m => m);
            var updateTask = database.UpdateManyStorageModels(models);
            updateTask.Wait();

            var getTask = database.GetManyStorageModels(m => !models.Keys.Contains(m.Model.Id));
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, storageModels.Where(m => !models.Keys.Contains(m.Model.Id)).ToList());
        }

        [Test]
        public void Throws_when_model_does_not_exist_in_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var updatedModels = new[]
            {
                new TestModel("Updated name 1", Guid.NewGuid()),
                new TestModel("Updated name 2", Guid.NewGuid())
            };

            var models = updatedModels.ToDictionary(m => m.Id, m => m);

            Assert.That(
                () => Task.Run(() => database.UpdateManyTestModels(models)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<DatabaseException>());
        }

        [Test]
        public void Throws_when_model_does_not_exist_in_database_with_custom_metadata()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var updatedModels = StorageModel.CreateMany("Updated models", count: 2);
            var models = updatedModels.ToDictionary(m => m.Model.Id, m => m);

            Assert.That(
                () => Task.Run(() => database.UpdateManyStorageModels(models)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<DatabaseException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Commands.UpdateManyAsync<TestReadModelDatabase, Guid, TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Commands.UpdateManyAsync<TestReadModelDatabase, Guid, TestModel, TestMetadata>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => database.UpdateManyTestModels(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => database.UpdateManyStorageModels(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
