// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestModel.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;

    public class TestModel
    {
        public TestModel(string name, Guid id = default(Guid))
        {
            Name = name;
            Id = (id == default(Guid)) ? Guid.NewGuid() : id;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public static IReadOnlyList<TestModel> CreateMany(string namePrefix, int count = 3)
        {
            var testModels = new List<TestModel>(count);

            for (var i = 0; i < count; i++)
            {
                var model = new TestModel(namePrefix + i);

                testModels.Add(model);
            }

            return testModels;
        }
    }
}
