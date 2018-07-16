// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetOne.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Linq;
    using MongoDB.Driver;

    public static partial class Queries
    {
        /// <summary>
        /// Creates a get one query against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new get one query.</returns>
        public static GetOneQueryAsync<TModel> GetOneAsync<TModel>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            GetOneQueryAsync<TModel> queryAsync = async (where, collectionName, cancellationToken) =>
            {
                if (where == null)
                {
                    throw new ArgumentNullException(nameof(where));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);

                var results = await collection.Find(where).ToListAsync(cancellationToken);
                return results.SingleOrDefault();
            };

            return queryAsync;
        }

        /// <summary>
        /// Creates a get one query against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new get one query.</returns>
        public static GetOneQueryUsingFilterDefinitionAsync<TModel> GetOneUsingFilterDefinitionAsync<TModel>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            GetOneQueryUsingFilterDefinitionAsync<TModel> queryAsync = async (filterDefinition, collectionName, cancellationToken) =>
            {
                if (filterDefinition == null)
                {
                    throw new ArgumentNullException(nameof(filterDefinition));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);

                var results = await collection.Find(filterDefinition).ToListAsync(cancellationToken);
                return results.SingleOrDefault();
            };

            return queryAsync;
        }

        /// <summary>
        /// Creates a get one query against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new get one query.</returns>
        public static GetOneQueryAsync<TModel, TMetadata> GetOneAsync<TModel, TMetadata>(MongoDatabase database)
        {
            var getOneQueryAsync = GetOneAsync<StorageModel<TModel, TMetadata>>(database);

            GetOneQueryAsync<TModel, TMetadata> queryAsync = async (where, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var result = await getOneQueryAsync(where, modelTypeName, cancellationToken);

                return result == null ? default(TModel) : result.Model;
            };

            return queryAsync;
        }
    }
}
