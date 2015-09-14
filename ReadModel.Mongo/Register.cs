// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Register.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// This class is designed to assist registering all the comands and queries with a dependency
    /// injection container.
    /// </summary>
    public static class Register
    {
        /// <summary>
        /// Calls back the specified register method with all the concrete queries for each model
        /// type so they can be easily registered in a dependency injection container.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <param name="register">
        /// The register method to call - put you container specific registration logic in this method.
        /// </param>
        /// <param name="types">The set of model types to register query methods for.</param>
        public static void Queries<TDatabase>(Action<Type, Func<TDatabase, object>> register, params Type[] types)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (register == null)
            {
                throw new ArgumentNullException(nameof(register));
            }

            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            var methods = typeof(Queries).GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var type in types)
            {
                var genericArgumentTypes = new List<Type> { typeof(TDatabase) };
                var additionalGenericArgumentTypes = GetGenericArgumentTypes(type);
                genericArgumentTypes.AddRange(additionalGenericArgumentTypes);

                RegisterMethod(register, methods, "GetOneAsync", genericArgumentTypes);
                RegisterMethod(register, methods, "GetManyAsync", genericArgumentTypes);
                RegisterMethod(register, methods, "GetAllAsync", genericArgumentTypes);
            }
        }

        /// <summary>
        /// Calls back the specified register method with all the concrete projector types for each
        /// model type so they can be easily registered in a dependency injection container.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <param name="register">
        /// The register method to call - put you container specific registration logic in this method.
        /// </param>
        /// <param name="types">The set of model types to register projectors methods for.</param>
        public static void Projectors<TDatabase>(Action<Type, Func<TDatabase, object>> register, params Type[] types)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (register == null)
            {
                throw new ArgumentNullException(nameof(register));
            }

            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            foreach (var type in types)
            {
                var genericArgumentTypes = new List<Type> { typeof(TDatabase) };
                var additionalGenericArgumentTypes = GetGenericArgumentTypes(type).ToList();
                genericArgumentTypes.AddRange(additionalGenericArgumentTypes);

                RegisterProjector(register, genericArgumentTypes);
            }
        }

        /// <summary>
        /// Calls back the specified register method with all the concrete commands for each model
        /// type so they can be easily registered in a dependency injection container.
        /// </summary>
        /// <typeparam name="TDatabase">The type of the database.</typeparam>
        /// <param name="register">
        /// The register method to call - put you container specific registration logic in this method.
        /// </param>
        /// <param name="types">The set of model types to register command methods for.</param>
        public static void Commands<TDatabase>(Action<Type, Func<TDatabase, object>> register, params Type[] types)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            if (register == null)
            {
                throw new ArgumentNullException(nameof(register));
            }

            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            // First generic is always the model
            var simpleTypes = types.Select(t => GetGenericArgumentTypes(t).First()).Distinct().ToArray();
            SimpleCommands(register, simpleTypes);
            GenericCommands(register, types);
        }

        private static void SimpleCommands<TDatabase>(Action<Type, Func<TDatabase, object>> register, params Type[] types)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var methods = typeof(Commands).GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var type in types)
            {
                var genericArgumentTypes = new List<Type> { typeof(TDatabase) };
                var additionalGenericArgumentTypes = GetGenericArgumentTypes(type);

                // First is always the model
                genericArgumentTypes.Add(additionalGenericArgumentTypes.First());

                RegisterMethod(register, methods, "RemoveOneAsync", genericArgumentTypes);
                RegisterMethod(register, methods, "RemoveAllAsync", new[] { typeof(TDatabase) });
                RegisterMethod(register, methods, "RemoveAllAsync", genericArgumentTypes);
            }
        }

        private static void GenericCommands<TDatabase>(Action<Type, Func<TDatabase, object>> register, params Type[] types)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var methods = typeof(Commands).GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var type in types)
            {
                var additionalGenericArgumentTypes = GetGenericArgumentTypes(type).ToList();

                // First is always the model
                var idType = IdReader.ReadType(additionalGenericArgumentTypes.First());

                var genericArgumentTypes = new List<Type> { typeof(TDatabase) };
                var genericWithIdArgumentTypes = new List<Type> { typeof(TDatabase), idType };

                genericArgumentTypes.AddRange(additionalGenericArgumentTypes);
                genericWithIdArgumentTypes.AddRange(additionalGenericArgumentTypes);

                RegisterMethod(register, methods, "AddOneAsync", genericArgumentTypes);
                RegisterMethod(register, methods, "AddManyAsync", genericArgumentTypes);
                RegisterMethod(register, methods, "RemoveManyAsync", genericArgumentTypes);
                RegisterMethod(register, methods, "UpdateOneAsync", genericArgumentTypes);
                RegisterMethod(register, methods, "UpdateManyAsync", genericWithIdArgumentTypes);
            }
        }

        private static IEnumerable<Type> GetGenericArgumentTypes(Type type)
        {
            var genericArgumentTypes = new[] { type };

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();

                if (genericType == typeof(StorageModel<,>))
                {
                    var modelType = type.GenericTypeArguments[0];
                    var metadataType = type.GenericTypeArguments[1];

                    genericArgumentTypes = new[] { modelType, metadataType };
                }
            }

            return genericArgumentTypes;
        }

        private static void RegisterMethod<TDatabase>(
            Action<Type, Func<TDatabase, object>> register,
            IEnumerable<MethodInfo> allMethods,
            string name,
            IReadOnlyCollection<Type> genericArgumentTypes)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var openMethod = allMethods.Single(mi => mi.Name == name && mi.GetGenericArguments().Length == genericArgumentTypes.Count);
            var closedMethod = openMethod.MakeGenericMethod(genericArgumentTypes.ToArray());

            register(closedMethod.ReturnType, database => closedMethod.Invoke(null, new object[] { database }));
        }

        private static void RegisterProjector<TDatabase>(
            Action<Type, Func<TDatabase, object>> register,
            IReadOnlyCollection<Type> genericArgumentTypes)
            where TDatabase : ReadModelDatabase<TDatabase>
        {
            var openProjectorType = (genericArgumentTypes.Count == 2) ? typeof(Projector<,>) : typeof(Projector<,,>);
            var closedProjectorType = openProjectorType.MakeGenericType(genericArgumentTypes.ToArray());
            var constructor = closedProjectorType.GetConstructor(new[] { typeof(TDatabase) });

            register(closedProjectorType, database => constructor.Invoke(new object[] { database }));
        }
    }
}
