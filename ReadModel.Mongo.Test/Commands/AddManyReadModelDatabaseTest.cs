// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddManyReadModelDatabaseTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class AddManyReadModelDatabaseTest : AddManyTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestMongoDatabase();
            StorageModels = StorageModel.CreateMany(nameof(AddManyReadModelDatabaseTest), count: 3);
            TestModels = TestModel.CreateMany(nameof(AddManyReadModelDatabaseTest), count: 3);
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
                () => Commands.AddManyAsync<TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Commands.AddManyAsync<TestModel, TestMetadata>(null),
                Throws.TypeOf<ArgumentNullException>());
        }
    }
}
