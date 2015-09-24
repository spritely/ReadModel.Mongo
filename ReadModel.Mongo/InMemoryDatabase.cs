// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryDatabase.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Spritely.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Provides an in memory database that supplies the same interface as a real read model
    /// database would.
    /// </summary>
    public class InMemoryDatabase
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<object, object>> collections =
            new ConcurrentDictionary<string, ConcurrentDictionary<object, object>>();

        /// <summary>
        /// Executes query to get the specified model with metadata into the requested form. Throws
        /// if filter does not produce a single result.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="where">The filter clause to find a single result.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future result.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task<TModel> GetOneQueryAsync<TModel>(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            var models = GetCollection<TModel>(collectionName);
            var whereCompiled = where.Compile();

            var result = models.Values.Cast<TModel>().SingleOrDefault(whereCompiled);

            return Task.FromResult(result);
        }

        /// <summary>
        /// Executes query to get the specified model with metadata into the requested form. Throws
        /// if filter does not produce a single result.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="where">The filter clause to find a single result.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future result.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task<TModel> GetOneQueryAsync<TModel, TMetadata>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            var models = GetCollection<TModel>(collectionName);
            var whereCompiled = where.Compile();

            var result = models.Values.Cast<StorageModel<TModel, TMetadata>>().SingleOrDefault(whereCompiled);

            return Task.FromResult((result == null) ? default(TModel) : result.Model);
        }

        /// <summary>
        /// Executes query to get the specified subset of models with metadata into the requested form.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="where">The filter clause limiting results.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future set of filtered results.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task<IEnumerable<TModel>> GetManyQueryAsync<TModel>(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            var models = GetCollection<TModel>(collectionName);
            var whereCompiled = where.Compile();

            var results = models.Values.Cast<TModel>().Where(whereCompiled);

            return Task.FromResult(results);
        }

        /// <summary>
        /// Executes query to get the specified subset of models with metadata into the requested form.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="where">The filter clause limiting results.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future set of filtered results.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task<IEnumerable<TModel>> GetManyQueryAsync<TModel, TMetadata>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            var models = GetCollection<TModel>(collectionName);
            var whereCompiled = where.Compile();

            var results = models.Values.Cast<StorageModel<TModel, TMetadata>>().Where(whereCompiled);

            return Task.FromResult(results.Select(sm => sm.Model));
        }

        /// <summary>
        /// Executes query to get all models into the requested form.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future set of results.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task<IEnumerable<TModel>> GetAllQueryAsync<TModel>(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var models = GetCollection<TModel>(collectionName);

            var results = models.Values.Cast<TModel>();

            return Task.FromResult(results);
        }

        /// <summary>
        /// Executes query to get all models with metadata into the requested form.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future set of results.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public Task<IEnumerable<TModel>> GetAllQueryAsync<TModel, TMetadata>(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var models = GetCollection<TModel>(collectionName);

            var results = models.Values.Cast<StorageModel<TModel, TMetadata>>();

            return Task.FromResult(results.Select(sm => sm.Model));
        }

        /// <summary>
        /// Executes query and projects the specified model into the requested form. Throws if
        /// filter does not produce a single result.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause to find a single result.</param>
        /// <param name="project">The callback responsible for creating a projection from a model.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected result.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task<TProjection> ProjectOneQueryAsync<TModel, TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var models = GetCollection<TModel>(collectionName);
            var whereCompiled = where.Compile();
            var projectCompiled = project.Compile();

            var result = models.Values.Cast<TModel>().Where(whereCompiled).Select(projectCompiled).SingleOrDefault();

            return Task.FromResult(result);
        }

        /// <summary>
        /// Executes query and projects the specified model with metadata into the requested form.
        /// Throws if filter does not produce a single result.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause to find a single result.</param>
        /// <param name="project">
        /// The callback responsible for creating a projection from a model and metadata.
        /// </param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected result.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task<TProjection> ProjectOneQueryAsync<TModel, TMetadata, TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var models = GetCollection<TModel>(collectionName);
            var whereCompiled = where.Compile();
            var projectCompiled = project.Compile();

            var result =
                models.Values.Cast<StorageModel<TModel, TMetadata>>().Where(whereCompiled).Select(projectCompiled).SingleOrDefault();

            return Task.FromResult(result);
        }

        /// <summary>
        /// Executes query and projects the specified subset of models into the requested form.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause limiting results.</param>
        /// <param name="project">The callback responsible for creating a projection from a model.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected set of filtered results.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task<IEnumerable<TProjection>> ProjectManyQueryAsync<TModel, TProjection>(
            Expression<Func<TModel, bool>> where,
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var models = GetCollection<TModel>(collectionName);
            var whereCompiled = where.Compile();
            var projectCompiled = project.Compile();

            var results = models.Values.Cast<TModel>().Where(whereCompiled).Select(projectCompiled);

            return Task.FromResult(results);
        }

        /// <summary>
        /// Executes query and projects the specified subset of models with metadata into the
        /// requested form.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="where">The filter clause limiting results.</param>
        /// <param name="project">
        /// The callback responsible for creating a projection from a model and metadata.
        /// </param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected set of filtered results.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task<IEnumerable<TProjection>> ProjectManyQueryAsync<TModel, TMetadata, TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var models = GetCollection<TModel>(collectionName);
            var whereCompiled = where.Compile();
            var projectCompiled = project.Compile();

            var results = models.Values.Cast<StorageModel<TModel, TMetadata>>().Where(whereCompiled).Select(projectCompiled);

            return Task.FromResult(results);
        }

        /// <summary>
        /// Executes query and projects all models into the requested form.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="project">The callback responsible for creating a projection from a model.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected set of results.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task<IEnumerable<TProjection>> ProjectAllQueryAsync<TModel, TProjection>(
            Expression<Func<TModel, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var models = GetCollection<TModel>(collectionName);
            var projectCompiled = project.Compile();

            var results = models.Values.Cast<TModel>().Select(projectCompiled);

            return Task.FromResult(results);
        }

        /// <summary>
        /// Executes query and projects all models with metadata into the requested form.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <typeparam name="TProjection">The type of the projection.</typeparam>
        /// <param name="project">
        /// The callback responsible for creating a projection from a model and metadata.
        /// </param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task encapsulating the future projected set of results.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task<IEnumerable<TProjection>> ProjectAllQueryAsync<TModel, TMetadata, TProjection>(
            Expression<Func<StorageModel<TModel, TMetadata>, TProjection>> project,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var models = GetCollection<TModel>(collectionName);
            var projectCompiled = project.Compile();

            var results = models.Values.Cast<StorageModel<TModel, TMetadata>>().Select(projectCompiled);

            return Task.FromResult(results);
        }

        /// <summary>
        /// Executes the command to remove one model from the database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="model">The model to remove.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task RemoveOneCommandAsync<TModel>(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var id = IdReader.ReadValue(model);
            var models = GetCollection<TModel>(collectionName);

            object removedModel;
            models.TryRemove(id, out removedModel); // ignore if not found

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to remove a set of models matching the specified filter from the database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="where">The where filter indicating which models to remove.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task RemoveManyCommandAsync<TModel>(
            Expression<Func<TModel, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            var models = GetCollection<TModel>(collectionName);
            var whereCompiled = where.Compile();

            var results = models.Values.Cast<TModel>().Where(whereCompiled);

            object removedModel;
            results.ForEach(
                m =>
                {
                    var id = IdReader.ReadValue(m);
                    models.TryRemove(id, out removedModel); // ignore if not found
                });

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to remove a set of storage models matching the specified filter
        /// from the database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="where">The where filter indicating which storage models to remove.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task RemoveManyCommandAsync<TModel, TMetadata>(
            Expression<Func<StorageModel<TModel, TMetadata>, bool>> where,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            var models = GetCollection<TModel>(collectionName);
            var whereCompiled = where.Compile();

            var results = models.Values.Cast<StorageModel<TModel, TMetadata>>().Where(whereCompiled);

            object removedModel;
            results.ForEach(
                m =>
                {
                    var id = IdReader.ReadValue(m.Model);
                    models.TryRemove(id, out removedModel); // ignore if not found
                });

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to remove all models from the database for the specified collection name.
        /// </summary>
        /// <param name="collectionName">Name of the collection. Throws if null.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task RemoveAllCommandAsync(
            string collectionName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(collectionName))
            {
                throw new ArgumentNullException(nameof(collectionName));
            }

            ConcurrentDictionary<object, object> removed;
            collections.TryRemove(collectionName, out removed); // ignore if not found

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to remove all models from the database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task RemoveAllCommandAsync<TModel>(
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

            return RemoveAllCommandAsync(modelTypeName, cancellationToken);
        }

        /// <summary>
        /// Executes the command to add one model to the database. Throws if model is already present.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="model">The model to add.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task AddOneCommandAsync<TModel>(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

            var id = IdReader.ReadValue(model);
            var models = GetCollection<TModel>(modelTypeName);

            if (!models.TryAdd(id, model))
            {
                throw new DatabaseException(Invariant($"Database already contains model with id '{id}' in collection {modelTypeName}."));
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to add one model to the database. Throws if model is already present.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="model">The model to add.</param>
        /// <param name="metadata">The metadata to include.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task AddOneCommandAsync<TModel, TMetadata>(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

            var id = IdReader.ReadValue(model);
            var models = GetCollection<TModel>(modelTypeName);

            var storageModel = new StorageModel<TModel, TMetadata>
            {
                Id = id,
                Model = model,
                Metadata = metadata
            };

            if (!models.TryAdd(id, storageModel))
            {
                throw new DatabaseException(Invariant($"Database already contains model with id '{id}' in collection {modelTypeName}."));
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to add a set of models to the database. Throws if any models are
        /// already present.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="models">The models to add.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        public Task AddManyCommandAsync<TModel>(
            IEnumerable<TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            models.ForEach(m => AddOneCommandAsync(m, collectionName, cancellationToken));

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to add a set of storage models to the database. Throws if any
        /// storage models are already present.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="storageModels">The storage models to add.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        public Task AddManyCommandAsync<TModel, TMetadata>(
            IEnumerable<StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (storageModels == null)
            {
                throw new ArgumentNullException(nameof(storageModels));
            }

            storageModels.ForEach(sm => AddOneCommandAsync(sm.Model, sm.Metadata, collectionName, cancellationToken));

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to update a model in the database. Throws if model with matching
        /// identifier cannot be found.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="model">The model to update.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task UpdateOneCommandAsync<TModel>(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

            var id = IdReader.ReadValue(model);
            var models = GetCollection<TModel>(modelTypeName);

            object existingModel;
            if (!models.TryGetValue(id, out existingModel))
            {
                throw new DatabaseException(
                    Invariant($"Cannot find model to update with id '{id}' in database collection {modelTypeName}."));
            }

            if (!models.TryUpdate(id, model, existingModel))
            {
                throw new DatabaseException(
                    Invariant(
                        $"Model with id '{id}' in database collection {modelTypeName} was updated by another process while trying to update."));
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to update a model and its metadata in the database. Throws if
        /// storage model with matching identifier cannot be found.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="model">The model to update.</param>
        /// <param name="metadata">The metadata to include.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task UpdateOneCommandAsync<TModel, TMetadata>(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

            var id = IdReader.ReadValue(model);
            var models = GetCollection<TModel>(modelTypeName);

            var storageModel = new StorageModel<TModel, TMetadata>
            {
                Id = id,
                Model = model,
                Metadata = metadata
            };

            object existingModel;
            if (!models.TryGetValue(id, out existingModel))
            {
                throw new DatabaseException(Invariant($"Cannot find model with id '{id}' in database collection {modelTypeName}."));
            }

            if (!models.TryUpdate(id, storageModel, existingModel))
            {
                throw new DatabaseException(
                    Invariant(
                        $"Model with id '{id}' in database collection {modelTypeName} was updated by another process while trying to update."));
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to update a set of models in the database. Throws if any models
        /// cannot be found by their identifiers.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="models">The models to update.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        public Task UpdateManyCommandAsync<TId, TModel>(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            models.Values.ForEach(m => UpdateOneCommandAsync(m, collectionName, cancellationToken));

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to update a set of storage models in the database. Throws if any
        /// storage models cannot be found by their identifiers.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="storageModels">The storage models to update.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        public Task UpdateManyCommandAsync<TId, TModel, TMetadata>(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (storageModels == null)
            {
                throw new ArgumentNullException(nameof(storageModels));
            }

            storageModels.Values.ForEach(sm => UpdateOneCommandAsync(sm.Model, sm.Metadata, collectionName, cancellationToken));

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to add or update a model in the database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task AddOrUpdateOneCommandAsync<TModel>(
            TModel model,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var id = IdReader.ReadValue(model);
            var models = GetCollection<TModel>(collectionName);

            models.AddOrUpdate(id, _ => model, (_, __) => model);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to add or update a model and its metadata in the database.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task AddOrUpdateOneCommandAsync<TModel, TMetadata>(
            TModel model,
            TMetadata metadata = default(TMetadata),
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var id = IdReader.ReadValue(model);
            var models = GetCollection<TModel>(collectionName);

            var storageModel = new StorageModel<TModel, TMetadata>
            {
                Id = id,
                Model = model,
                Metadata = metadata
            };

            models.AddOrUpdate(id, _ => storageModel, (_, __) => storageModel);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to add or update a set of models in the database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="models">The models.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task AddOrUpdateManyCommandAsync<TId, TModel>(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            models.Values.ForEach(m => AddOrUpdateOneCommandAsync(m, collectionName, cancellationToken));

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command to add or update a set of storage models in the database.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="storageModels">The storage models.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        public Task AddOrUpdateManyCommandAsync<TId, TModel, TMetadata>(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (storageModels == null)
            {
                throw new ArgumentNullException(nameof(storageModels));
            }

            storageModels.Values.ForEach(sm => AddOrUpdateOneCommandAsync(sm.Model, sm.Metadata, collectionName, cancellationToken));

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command that assumes a complete set of models is provided and proceeds to
        /// add, update, or remove models in the database to bring it in sync.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="models">The models.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task MergeCompleteSetCommandAsync<TId, TModel>(
            IDictionary<TId, TModel> models,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;
            var modelDatabase = new ConcurrentDictionary<object, object>();

            models.ForEach(kv => modelDatabase.GetOrAdd(kv.Key, kv.Value));

            collections.AddOrUpdate(modelTypeName, _ => modelDatabase, (__, ___) => modelDatabase);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the command that assumes a complete set of storage models is provided and
        /// proceeds to add, update, or remove storage models in the database to bring it in sync.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
        /// <param name="storageModels">The storage models.</param>
        /// <param name="collectionName">
        /// Name of the collection. If null will use {TModel} to generate a collection name.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for tracking operation completion.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken",
            Justification = "This parameter is required to adhere to the interface.")]
        public Task MergeCompleteSetCommandAsync<TId, TModel, TMetadata>(
            IDictionary<TId, StorageModel<TModel, TMetadata>> storageModels,
            string collectionName = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (storageModels == null)
            {
                throw new ArgumentNullException(nameof(storageModels));
            }

            var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;
            var modelDatabase = new ConcurrentDictionary<object, object>();

            storageModels.ForEach(kv => modelDatabase.GetOrAdd(kv.Key, kv.Value));

            collections.AddOrUpdate(modelTypeName, _ => modelDatabase, (__, ___) => modelDatabase);

            return Task.FromResult<object>(null);
        }

        private ConcurrentDictionary<object, object> GetCollection<TModel>(string collectionName)
        {
            var modelTypeName = string.IsNullOrWhiteSpace(collectionName) ? typeof(TModel).Name : collectionName;

            var models = collections.GetOrAdd(modelTypeName, _ => new ConcurrentDictionary<object, object>());

            return models;
        }
    }
}
