﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveAllTestBase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    public abstract class RemoveAllTestBase
    {
        protected ITestDatabase Database { get; set; }

        protected IReadOnlyList<StorageModel<TestModel, TestMetadata>> StorageModels { get; set; }

        protected IReadOnlyList<TestModel> TestModels { get; set; }

        [Test]
        public void Removes_all_results_from_database()
        {
            Database.AddTestModelsToDatabase(TestModels);

            var removeTask = Database.ModelCommands.RemoveAllAsync();
            removeTask.Wait();

            var getTask = Database.ModelQueries.GetAllAsync();
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Removes_all_results_from_database_with_custom_metadata()
        {
            Database.AddStorageModelsToDatabase(StorageModels);

            var removeTask = Database.StorageModelCommands.RemoveAllAsync();
            removeTask.Wait();

            var getTask = Database.StorageModelQueries.GetAllAsync();
            getTask.Wait();

            var results = getTask.Result.ToList();
            Assert.That(results, Is.Empty);
        }
    }
}
