// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeCompleteSetInMemoryDatabaseTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class AddOneInMemoryDatabaseTest : AddOneTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestInMemoryDatabase();
            TestModel = new TestModel(nameof(AddOneInMemoryDatabaseTest));
            TestMetadata = new TestMetadata(nameof(AddOneInMemoryDatabaseTest));
        }

        [TearDown]
        public void CleanUp()
        {
            Database.Drop();
        }
    }
}
