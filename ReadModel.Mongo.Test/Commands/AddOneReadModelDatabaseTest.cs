﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOneReadModelDatabaseTest.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class AddOneReadModelDatabaseTest : AddOneTestBase
    {
        [SetUp]
        public void Setup()
        {
            Database = new TestMongoDatabase();
            TestModel = new TestModel(nameof(AddOneReadModelDatabaseTest));
            TestMetadata = new TestMetadata(nameof(AddOneReadModelDatabaseTest));
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
                () => Commands.AddOneAsync<TestModel>(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Create_throws_on_invalid_arguments_with_custom_metadata()
        {
            Assert.That(
                () => Commands.AddOneAsync<TestModel, TestMetadata>(null),
                Throws.TypeOf<ArgumentNullException>());
        }
    }
}
