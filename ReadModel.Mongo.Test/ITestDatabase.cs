// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITestDatabase.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;

    public interface ITestDatabase
    {
        IQueries<TestModel> ModelQueries { get; }

        IQueries<TestModel, TestMetadata> StorageModelQueries { get; }

        ICommands<Guid, TestModel> ModelCommands { get; }

        ICommands<Guid, TestModel, TestMetadata> StorageModelCommands { get; }

        void Drop();

        void AddTestModelsToDatabase(IEnumerable<TestModel> testModels);

        void AddStorageModelsToDatabase(IEnumerable<StorageModel<TestModel, TestMetadata>> storageModels);
    }
}
