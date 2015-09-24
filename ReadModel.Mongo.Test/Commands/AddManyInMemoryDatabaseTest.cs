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
    public class AddManyInMemoryDatabaseTest : AddManyTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(AddManyInMemoryDatabaseTest), count: 3);
            TestModels = TestModel.CreateMany(nameof(AddManyInMemoryDatabaseTest), count: 3);
        }

        [TearDown]
        public void CleanUp()
        {
            Database.Drop();
        }
    }
}
