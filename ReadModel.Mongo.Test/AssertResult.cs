// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssertResult.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using NUnit.Framework;
    using Spritely.Cqrs;

    internal static class AssertResult
    {
        public static void Matches(TestModel result, StorageModel<TestModel, TestMetadata> storageModel)
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(storageModel.Model.Id));
            Assert.That(result.Name, Is.EqualTo(storageModel.Model.Name));
        }
    }
}
