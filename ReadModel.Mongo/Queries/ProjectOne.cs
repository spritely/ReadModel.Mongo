﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectOne.cs">
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
        /// Creates a project one query against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new project one query.</returns>
        public static ProjectOneQueryAsync<TModel, TProjection> ProjectOneAsync<TModel, TProjection>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
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

                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);

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
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new project one query.</returns>
        public static ProjectOneQueryUsingFilterDefinitionAsync<TModel, TProjection> ProjectOneUsingFilterDefinitionAsync<TModel, TProjection>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            ProjectOneQueryUsingFilterDefinitionAsync<TModel, TProjection> queryAsync = async (filterDefinition, project, collectionName, cancellationToken) =>
            {
                if (filterDefinition == null)
                {
                    throw new ArgumentNullException((nameof(filterDefinition)));
                }

                if (project == null)
                {
                    throw new ArgumentNullException((nameof(project)));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);

                var projectionDefinition = Builders<TModel>.Projection.Expression(project);
                var findOptions = new FindOptions<TModel, TProjection>()
                {
                    Projection = projectionDefinition
                };

                var findResults = await collection.FindAsync(filterDefinition, findOptions, cancellationToken);
                var results = await findResults.ToListAsync(cancellationToken);

                return results.SingleOrDefault();
            };

            return queryAsync;
        }

        /// <summary>
        /// Creates a project one query against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new project one query.</returns>
        public static ProjectOneQueryAsync<TModel, TMetadata, TProjection> ProjectOneAsync<TModel, TMetadata, TProjection>(
            MongoDatabase database)
        {
            var projectOneQueryAsync = ProjectOneAsync<StorageModel<TModel, TMetadata>, TProjection>(database);

            ProjectOneQueryAsync<TModel, TMetadata, TProjection> queryAsync = async (where, project, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                return await projectOneQueryAsync(where, project, modelTypeName, cancellationToken);
            };

            return queryAsync;
        }
    }
}
