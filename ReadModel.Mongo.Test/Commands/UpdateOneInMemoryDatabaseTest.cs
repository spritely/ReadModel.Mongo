// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateOneInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class UpdateOneInMemoryDatabaseTest : UpdateOneTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(UpdateOneInMemoryDatabaseTest), count: 3);
            TestModels = TestModel.CreateMany(nameof(UpdateOneInMemoryDatabaseTest), count: 3);
        }

        [TearDown]
        public void CleanUp()
        {
            Database.Drop();
        }
    }
}
