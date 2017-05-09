// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddManyTestBase.cs">
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

    public abstract class AddManyTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        [Test]
        public void Adds_records_to_database()
        {
            var addTask = Task.Run(() => Database.ModelCommands.AddManyAsync(TestModels));
            addTask.Wait();

            var getTask = Task.Run(() => Database.ModelQueries.GetAllAsync());
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, TestModels);
        }

        [Test]
        public void Adds_record_to_database_with_custom_metadata()
        {
            var addTask = Task.Run(() => Database.StorageModelCommands.AddManyAsync(StorageModels));
            addTask.Wait();

            var getTask = Task.Run(() => Database.StorageModelQueries.GetAllAsync());
            getTask.Wait();

            var results = getTask.Result.ToList();
            AssertResults.Match(results, StorageModels);
        }

        [Test]
        public void Throws_when_record_with_same_id_already_present_in_database()
        {
            var addTask = Task.Run(() => Database.ModelCommands.AddManyAsync(TestModels));
            addTask.Wait();

            Assert.That(
                () => Task.Run(() => Database.ModelCommands.AddManyAsync(TestModels)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<DatabaseException>());
        }

        [Test]
        public void Throws_when_record_with_same_id_already_present_in_database_with_custom_metadata()
        {
            var addTask = Task.Run(() => Database.StorageModelCommands.AddManyAsync(StorageModels));
            addTask.Wait();

            Assert.That(
                () => Task.Run(() => Database.StorageModelCommands.AddManyAsync(StorageModels)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<DatabaseException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => Database.ModelCommands.AddManyAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => Database.StorageModelCommands.AddManyAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
