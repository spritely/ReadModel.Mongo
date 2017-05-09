// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveManyInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class RemoveManyInMemoryDatabaseTest : RemoveManyTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(RemoveManyInMemoryDatabaseTest), count: 5);
            TestModels = TestModel.CreateMany(nameof(RemoveManyInMemoryDatabaseTest), count: 5);
            TestStorageName = nameof(RemoveManyInMemoryDatabaseTest);
        }

        [TearDown]
        public void CleanUp()
        {
            Database.Drop();
        }
    }
}
