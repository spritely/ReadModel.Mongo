// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetManyReadModelDatabaseTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ProjectManyReadModelDatabaseTest : ProjectManyTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestReadModelDatabase();
            StorageModels = StorageModel.CreateMany(nameof(ProjectManyReadModelDatabaseTest), count: 5);
            TestModels = TestModel.CreateMany(nameof(ProjectManyReadModelDatabaseTest), count: 5);
            TestStorageName = nameof(ProjectManyReadModelDatabaseTest);
        }

        [TearDown]
        public void CleanUp()
        {
            Database.Drop();
        }

        [Test]
        public void Create_throws_on_invalid_arguments()
        {
            Assert.That(
                () => Queries.ProjectManyAsync<TestReadModelDatabase, TestModel, ProjectManyTestBase>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Queries.ProjectManyAsync<TestReadModelDatabase, TestModel, TestMetadata, ProjectManyTestBase>(null),
                Throws.TypeOf<ArgumentNullException>());
        }
    }
}
