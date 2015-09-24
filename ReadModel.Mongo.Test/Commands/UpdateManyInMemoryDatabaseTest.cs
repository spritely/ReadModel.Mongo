// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateManyInMemoryDatabaseTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class UpdateManyInMemoryDatabaseTest : UpdateManyTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(UpdateManyInMemoryDatabaseTest), count: 5);
            TestModels = TestModel.CreateMany(nameof(UpdateManyInMemoryDatabaseTest), count: 5);
        }

        [TearDown]
        public void CleanUp()
        {
            Database.Drop();
        }
    }
}
