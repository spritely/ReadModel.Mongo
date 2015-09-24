// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectOneReadModelDatabaseTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ProjectOneReadModelDatabaseTest : ProjectOneTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestReadModelDatabase();
            StorageModels = StorageModel.CreateMany(nameof(ProjectOneReadModelDatabaseTest), count: 3);
            TestModels = TestModel.CreateMany(nameof(ProjectOneReadModelDatabaseTest), count: 3);
            TestStorageName = nameof(ProjectOneReadModelDatabaseTest);
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
                () => Queries.ProjectOneAsync<TestReadModelDatabase, TestModel, ProjectOneTestBase>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Queries.ProjectOneAsync<TestReadModelDatabase, TestModel, TestMetadata, ProjectOneTestBase>(null),
                Throws.TypeOf<ArgumentNullException>());
        }
    }
}
