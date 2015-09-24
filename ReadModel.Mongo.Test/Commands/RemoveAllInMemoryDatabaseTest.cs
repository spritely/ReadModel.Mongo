// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveAllInMemoryDatabaseTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class RemoveAllInMemoryDatabaseTest : RemoveAllTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestInMemoryDatabase();
            StorageModels = StorageModel.CreateMany(nameof(RemoveAllInMemoryDatabaseTest), count: 5);
            TestModels = TestModel.CreateMany(nameof(RemoveAllInMemoryDatabaseTest), count: 5);
        }

        [TearDown]
        public void CleanUp()
        {
            Database.Drop();
        }

        [Test]
        public void Throws_on_invalid_arguments()
        {
            Assert.That(
                () => Task.Run(() => ((TestInMemoryDatabase)Database).Database.RemoveAllCommandAsync(" ")).Wait(),
                Throws.TypeOf<AggregateException>().With.InnerException.TypeOf<ArgumentNullException>());
        }
    }
}
