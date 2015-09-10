// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveAll.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;

    public static partial class Commands
    {
        public static RemoveAllCommandAsync RemoveAllAsync<TDatabase>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            RemoveAllCommandAsync commandAsync = async (collectionName, cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(collectionName))
                {
                    throw new ArgumentNullException(nameof(collectionName));
                }

                var database = readModelDatabase.CreateConnection();
                await database.DropCollectionAsync(collectionName, cancellationToken);
            };

            return commandAsync;
        }

        public static RemoveAllCommandAsync<TModel> RemoveAllAsync<TDatabase, TModel>(TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var removeAllCommandAsync = RemoveAllAsync(readModelDatabase);

            RemoveAllCommandAsync<TModel> commandAsync =
                async (collectionName, cancellationToken) =>
                {
                    var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                    await removeAllCommandAsync(modelTypeName, cancellationToken);
                };

            return commandAsync;
        }
    }
}
