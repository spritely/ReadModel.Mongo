﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectOne.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
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
        /// Creates a project one query against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new project one query.</returns>
        public static ProjectOneQueryAsync<TModel, TProjection> ProjectOneAsync<TDatabase, TModel, TProjection>(
            TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            ProjectOneQueryAsync<TModel, TProjection> queryAsync = async (where, project, collectionName, cancellationToken) =>
            {
                if (where == null)
                {
                    throw new ArgumentNullException((nameof(where)));
                }

                if (project == null)
                {
                    throw new ArgumentNullException((nameof(project)));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);

                var projectionDefinition = Builders<TModel>.Projection.Expression(project);
                var findOptions = new FindOptions<TModel, TProjection>()
                {
                    Projection = projectionDefinition
                };

                var findResults = await collection.FindAsync(where, findOptions, cancellationToken);
                var results = await findResults.ToListAsync(cancellationToken);

                return results.SingleOrDefault();
            };

            return queryAsync;
        }

        /// <summary>
        /// Creates a project one query against the specified database.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <returns>A new project one query.</returns>
        public static ProjectOneQueryAsync<TModel, TMetadata, TProjection> ProjectOneAsync<TDatabase, TModel, TMetadata, TProjection>(
            TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var projectOneQueryAsync = ProjectOneAsync<TDatabase, StorageModel<TModel, TMetadata>, TProjection>(readModelDatabase);

            ProjectOneQueryAsync<TModel, TMetadata, TProjection> queryAsync = async (where, project, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                return await projectOneQueryAsync(where, project, modelTypeName, cancellationToken);
            };

            return queryAsync;
        }
    }
}
