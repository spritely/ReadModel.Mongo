// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestReadModelDatabase.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    public sealed class TestReadModelDatabase : ReadModelDatabase<TestReadModelDatabase>
    {
        public TestReadModelDatabase()
        {
            this.ConnectionSettings = new MongoConnectionSettings()
            {
                Database = "Test"
            };
        }
    }
}
