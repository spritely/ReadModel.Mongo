// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectAllReadModelDatabaseTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ProjectAllReadModelDatabaseTest : ProjectAllTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestMongoDatabase();
            StorageModels = StorageModel.CreateMany(nameof(ProjectAllReadModelDatabaseTest), count: 5);
            TestModels = TestModel.CreateMany(nameof(ProjectAllReadModelDatabaseTest), count: 5);
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
                () => Queries.ProjectAllAsync<TestModel, ProjectAllTestBase>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Queries.ProjectAllAsync<TestModel, TestMetadata, ProjectAllTestBase>(null),
                Throws.TypeOf<ArgumentNullException>());
        }
    }
}
