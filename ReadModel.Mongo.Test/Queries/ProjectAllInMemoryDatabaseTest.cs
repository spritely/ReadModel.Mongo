// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectAllInMemoryDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class ProjectAllInMemoryDatabaseTest : ProjectAllTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(ProjectAllInMemoryDatabaseTest), count: 5);
            TestModels = TestModel.CreateMany(nameof(ProjectAllInMemoryDatabaseTest), count: 5);
        }

        [TearDown]
        public void CleanUp()
        {
            Database.Drop();
        }
    }
}
