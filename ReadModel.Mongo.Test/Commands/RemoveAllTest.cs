// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveAllTest.cs">
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
    using Spritely.Cqrs;

    [TestFixture]
    public class RemoveAllTest
    {
        private readonly TestReadModelDatabase database = new TestReadModelDatabase();

        private readonly IReadOnlyList<StorageModel<TestModel, TestMetadata>> storageModels =
            StorageModel.CreateMany(nameof(RemoveAllTest), count: 5);

        private readonly IReadOnlyList<TestModel> testModels = TestModel.CreateMany(nameof(RemoveAllTest), count: 5);

        [Test]
        public void Removes_all_results_from_database()
        {
            database.AddTestModelsToDatabase(testModels);

            var removeTask = database.RemoveAll(nameof(TestModel));
            removeTask.Wait();

            var getTask = database.GetAllTestModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Removes_all_results_from_database_with_model_overload()
        {
            database.AddStorageModelsToDatabase(storageModels);

            var removeTask = database.RemoveAllModels();
            removeTask.Wait();

            var getTask = database.GetAllStorageModels();
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Commands.RemoveAllAsync<TestReadModelDatabase>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_model_overload()
        {
            Assert.That(
                () => Commands.RemoveAllAsync<TestReadModelDatabase, TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => database.RemoveAll(" ")).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
