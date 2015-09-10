// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectMany.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using MongoDB.Driver;

    public static partial class Queries
    {
        public static ProjectManyQueryAsync<TModel, TProjection> ProjectManyAsync<TDatabase, TModel, TProjection>(
            TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            ProjectManyQueryAsync<TModel, TProjection> queryAsync = async (where, project, collectionName, cancellationToken) =>
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
                return await findResults.ToListAsync(cancellationToken);
            };

            return queryAsync;
        }

        public static ProjectManyQueryAsync<TModel, TMetadata, TProjection> ProjectManyAsync<TDatabase, TModel, TMetadata, TProjection>(
            TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var projectManyQueryAsync = ProjectManyAsync<TDatabase, StorageModel<TModel, TMetadata>, TProjection>(readModelDatabase);

            ProjectManyQueryAsync<TModel, TMetadata, TProjection> queryAsync = async (where, project, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                return await projectManyQueryAsync(where, project, modelTypeName, cancellationToken);
            };

            return queryAsync;
        }
    }
}
