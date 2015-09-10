// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectAll.cs">
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
        public static ProjectAllQueryAsync<TModel, TProjection> ProjectAllAsync<TDatabase, TModel, TProjection>(
            TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException(nameof(readModelDatabase));
            }

            ProjectAllQueryAsync<TModel, TProjection> queryAsync = async (project, collectionName, cancellationToken) =>
            {
                if (project == null)
                {
                    throw new ArgumentNullException((nameof(project)));
                }

                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                var database = readModelDatabase.CreateConnection();
                var collection = database.GetCollection<TModel>(modelTypeName);

                var projectionDefinition = Builders<TModel>.Projection.Expression(project);

                var search = collection.Aggregate().Project(projectionDefinition);
                return await search.ToListAsync(cancellationToken);
            };

            return queryAsync;
        }

        public static ProjectAllQueryAsync<TModel, TMetadata, TProjection> ProjectAllAsync<TDatabase, TModel, TMetadata, TProjection>(
            TDatabase readModelDatabase)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var projectAllQueryAsync = ProjectAllAsync<TDatabase, StorageModel<TModel, TMetadata>, TProjection>(readModelDatabase);

            ProjectAllQueryAsync<TModel, TMetadata, TProjection> queryAsync = async (project, collectionName, cancellationToken) =>
            {
                var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

                return await projectAllQueryAsync(project, modelTypeName, cancellationToken);
            };

            return queryAsync;
        }
    }
}
