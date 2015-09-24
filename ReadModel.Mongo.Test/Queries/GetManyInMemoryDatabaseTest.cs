// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetManyInMemoryDatabaseTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class GetManyInMemoryDatabaseTest : GetManyTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(GetManyInMemoryDatabaseTest), count: 5);
            TestModels = TestModel.CreateMany(nameof(GetManyInMemoryDatabaseTest), count: 5);
            TestStorageName = nameof(GetManyInMemoryDatabaseTest);
        }

        [TearDown]
        public void CleanUp()
        {
            Database.Drop();
        }
    }
}
