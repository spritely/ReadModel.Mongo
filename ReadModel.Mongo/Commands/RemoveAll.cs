// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveAll.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
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
        /// <param name="database">The database.</param>
        /// <returns>A new remove all command.</returns>
        public static RemoveAllCommandAsync RemoveAllAsync(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            RemoveAllCommandAsync commandAsync = async (collectionName, cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(collectionName))
                {
                    throw new ArgumentNullException(nameof(collectionName));
                }

                var client = database.CreateClient();
                await client.DropCollectionAsync(collectionName, cancellationToken);
            };

            return commandAsync;
        }

        /// <summary>
        /// Creates a remove all command against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new remove all command.</returns>
        public static RemoveAllCommandAsync<TModel> RemoveAllAsync<TModel>(MongoDatabase database)
        {
            var removeAllCommandAsync = RemoveAllAsync(database);

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
