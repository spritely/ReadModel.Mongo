// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOneTestBase.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;

    public abstract class AddOneTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected TestMetadata TestMetadata { get; set; }

        protected TestModel TestModel { get; set; }

        [Test]
        public void Adds_record_to_database()
        {
            var addTask = Task.Run(() => Database.ModelCommands.AddOneAsync(TestModel));
            addTask.Wait();

            var getTask = Task.Run(() => Database.ModelQueries.GetOneAsync(m => m.Id == TestModel.Id));
            getTask.Wait();

            var result = getTask.Result;
            Assert.That(result.Name, Is.EqualTo(TestModel.Name));
        }

        [Test]
        public void Adds_record_to_database_with_custom_metadata()
        {
            var addTask = Task.Run(() => Database.StorageModelCommands.AddOneAsync(TestModel, TestMetadata));
            addTask.Wait();

            var getTask = Task.Run(() => Database.StorageModelQueries.GetOneAsync(sm => sm.Model.Id == TestModel.Id));
            getTask.Wait();

            var result = getTask.Result;
            Assert.That(result.Name, Is.EqualTo(TestModel.Name));
        }

        [Test]
        public void Throws_when_record_with_same_id_already_present_in_database()
        {
            var addTask = Task.Run(() => Database.ModelCommands.AddOneAsync(TestModel));
            addTask.Wait();

            Assert.That(
                () => Task.Run(() => Database.ModelCommands.AddOneAsync(TestModel)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<DatabaseException>());
        }

        [Test]
        public void Throws_when_record_with_same_id_already_present_in_database_with_custom_metadata()
        {
            var addTask = Task.Run(() => Database.StorageModelCommands.AddOneAsync(TestModel, TestMetadata));
            addTask.Wait();

            Assert.That(
                () => Task.Run(() => Database.StorageModelCommands.AddOneAsync(TestModel, TestMetadata)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<DatabaseException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => Database.ModelCommands.AddOneAsync(null)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Task.Run(() => Database.StorageModelCommands.AddOneAsync(null, TestMetadata)).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
