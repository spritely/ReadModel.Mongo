// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectAll.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public static partial class Queries
    {
        /// <summary>
        /// Creates a project all query against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new project all query.</returns>
        public static ProjectAllQueryAsync<TModel, TProjection> ProjectAllAsync<TModel, TProjection>(MongoDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            ProjectAllQueryAsync<TModel, TProjection> queryAsync = async (project, collectionName, cancellationToken) =>
            {
                if (project == null)
                {
                    throw new ArgumentNullException((nameof(project)));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var client = database.CreateClient();
                var collection = client.GetCollection<TModel>(modelTypeName);

                var projectionDefinition = Builders<TModel>.Projection.Expression(project);

                var filter = new BsonDocument();
                var findOptions = new FindOptions<TModel, TProjection>()
                {
                    Projection = projectionDefinition
                };

                var findResults = await collection.FindAsync(filter, findOptions, cancellationToken);
                return await findResults.ToListAsync(cancellationToken);
            };

            return queryAsync;
        }

        /// <summary>
        /// Creates a project all query against the specified database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="database">The database.</param>
        /// <returns>A new project all query.</returns>
        public static ProjectAllQueryAsync<TModel, TMetadata, TProjection> ProjectAllAsync<TModel, TMetadata, TProjection>(
            MongoDatabase database)
        {
            var projectAllQueryAsync = ProjectAllAsync<StorageModel<TModel, TMetadata>, TProjection>(database);

            ProjectAllQueryAsync<TModel, TMetadata, TProjection> queryAsync = async (project, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                return await projectAllQueryAsync(project, modelTypeName, cancellationToken);
            };

            return queryAsync;
        }
    }
}
