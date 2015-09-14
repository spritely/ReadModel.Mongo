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
        /// <summary>
        /// Creates a remove all command against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new remove all command.</returns>
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

        /// <summary>
        /// Creates a remove all command against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new remove all command.</returns>
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
