// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddManyInMemoryDatabaseTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class AddOrUpdateOneInMemoryDatabaseTest : AddOrUpdateOneTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(AddOrUpdateOneInMemoryDatabaseTest), count: 3);
            TestModels = TestModel.CreateMany(nameof(AddOrUpdateOneInMemoryDatabaseTest), count: 3);
        }

        [TearDown]
        public void CleanUp()
        {
            Database.Drop();
        }
    }
}
