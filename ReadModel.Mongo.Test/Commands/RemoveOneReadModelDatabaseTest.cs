// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveOneReadModelDatabaseTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class RemoveOneReadModelDatabaseTest : RemoveOneTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestReadModelDatabase();
            StorageModels = StorageModel.CreateMany(nameof(RemoveOneReadModelDatabaseTest), count: 5);
            TestModels = TestModel.CreateMany(nameof(RemoveOneReadModelDatabaseTest), count: 5);
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
                () => Commands.RemoveOneAsync<TestReadModelDatabase, TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }
    }
}
