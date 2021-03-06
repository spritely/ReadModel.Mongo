// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Spritely.ReadModel.Mongo",
        Justification = "This is designed to separate the dependency on Mongo.")]
