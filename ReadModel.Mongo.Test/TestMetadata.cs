﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestMetadata.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    public class TestMetadata
    {
        public TestMetadata(string name)
        {
            FirstName = "First " + name;
            LastName = "Last " + name;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
