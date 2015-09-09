// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssertResults.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System.Collections.Generic;
    using FluentAssertions;
    using NUnit.Framework;
    using Spritely.Cqrs;

    internal static class AssertResults
    {
        public static void Match(IReadOnlyCollection<TestModel> results, IReadOnlyCollection<TestModel> testModels)
        {
            Assert.That(results.Count, Is.EqualTo(testModels.Count));
            foreach (var testModel in testModels)
            {
                results.Should().Contain(m => m.Id == testModel.Id && m.Name == testModel.Name);
            }
        }

        public static void Match(
            IReadOnlyCollection<StorageModel<TestModel, TestMetadata>> results,
            IReadOnlyCollection<StorageModel<TestModel, TestMetadata>> storageModels)
        {
            Assert.That(results.Count, Is.EqualTo(storageModels.Count));
            foreach (var storageModel in storageModels)
            {
                results.Should()
                    .Contain(
                        m =>
                            m.Model.Id == storageModel.Model.Id && m.Model.Name == storageModel.Model.Name &&
                            m.Metadata.FirstName == storageModel.Metadata.FirstName && m.Metadata.LastName == storageModel.Metadata.LastName);
            }
        }

        public static void Match(
            IReadOnlyCollection<TestModel> results,
            IReadOnlyCollection<StorageModel<TestModel, TestMetadata>> storageModels)
        {
            Assert.That(results.Count, Is.EqualTo(storageModels.Count));
            foreach (var storageModel in storageModels)
            {
                results.Should().Contain(m => m.Id == storageModel.Model.Id && m.Name == storageModel.Model.Name);
            }
        }
    }
}
