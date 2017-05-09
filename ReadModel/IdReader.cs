// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdReader.cs">
//     Copyright (c) 2017. All rights reserved. Licensed under the MIT license. See LICENSE file in
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
    using System.Reflection;
    using static System.FormattableString;

    /// <summary>
    /// Class capable of reading Id properties from objects.
    /// </summary>
    public static class IdReader
    {
        private static readonly ConcurrentDictionary<Type, IdDefinition> idDefinitions = new ConcurrentDictionary<Type, IdDefinition>();

        /// <summary>
        /// Read the value from an Id member of the specified model instance.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="model">The model.</param>
        /// <returns>The value of the Id member.</returns>
        public static object ReadValue<TModel>(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var idDefinition = idDefinitions.GetOrAdd(typeof(TModel), new IdDefinition(typeof(TModel)));
            return idDefinition.ReadValue(model);
        }

        /// <summary>
        /// Reads the name of the Id member.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>The name of the Id member.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "This type safe method is preferred to its non-type safe equivalent.")]
        public static string ReadName<TModel>()
        {
            var idDefinition = idDefinitions.GetOrAdd(typeof(TModel), new IdDefinition(typeof(TModel)));
            return idDefinition.Name;
        }

        /// <summary>
        /// Reads the type of the Id member.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>The type of the Id member.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is an overload that doesn't require a generic parameter.")]
        public static Type ReadType<TModel>()
        {
            var idDefinition = idDefinitions.GetOrAdd(typeof(TModel), new IdDefinition(typeof(TModel)));
            return idDefinition.Type;
        }

        /// <summary>
        /// Reads the type of the Id member from the given model type.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>The type of the Id member.</returns>
        public static Type ReadType(Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            var idDefinition = idDefinitions.GetOrAdd(modelType, new IdDefinition(modelType));
            return idDefinition.Type;
        }

        /// <summary>
        /// Sets the identifier member used during reads of a particular model type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="idMember">The identifier member.</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "This type safe method is preferred to its non-type safe equivalent.")]
        public static void SetIdMember<TModel>(string idMember)
        {
            if (string.IsNullOrWhiteSpace(idMember))
            {
                throw new ArgumentNullException(nameof(idMember));
            }

            var idDefinition = idDefinitions.GetOrAdd(typeof(TModel), new IdDefinition(typeof(TModel)));
            idDefinition.Names = new[] { idMember };
        }

        internal class IdDefinition
        {
            private static readonly object Lock = new object();
            private ICollection<string> names = new[] { "Id", "id", "_id" };
            private Func<object, object> readValue;
            private string name;
            private Type idType;
            private readonly Type modelType;

            public IdDefinition(Type modelType)
            {
                this.modelType = modelType;
            }

            public ICollection<string> Names
            {
                get { return names; }
                set
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }

                    lock (Lock)
                    {
                        names = value;
                        ReadValue = null;
                        Type = null;
                    }
                }
            }

            public Func<object, object> ReadValue
            {
                get
                {
                    if (readValue == null)
                    {
                        Initialize();
                    }
                    return readValue;
                }
                private set { readValue = value; }
            }

            public string Name
            {
                get
                {
                    if (name == null)
                    {
                        Initialize();
                    }
                    return name;
                }
                private set { name = value; }
            }

            public Type Type
            {
                get
                {
                    if (idType == null)
                    {
                        Initialize();
                    }
                    return idType;
                }
                private set { idType = value; }
            }

            private void Initialize()
            {
                var allMembers =
                    modelType.GetMembers(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                var elegibleMembers =
                    allMembers.Where(
                        m => m.MemberType == MemberTypes.Field || (m.MemberType == MemberTypes.Property && ((PropertyInfo)m).CanRead));

                lock (Lock)
                {
                    var matchingMembers = elegibleMembers.Where(m => Names.Contains(m.Name));
                    var idMember = Names.SelectMany(idName => matchingMembers.Where(m => m.Name == idName)).FirstOrDefault();

                    if (idMember == null)
                    {
                        throw new ArgumentException(
                            Invariant(
                                $"{modelType.Name} does not have an Id property or it is not readable (set only). Add a property or field named [{Names}] or if your object has a different name call IdReader.SetIdMember<{modelType.Name}>(\"YourIdProperty\")"));
                    }

                    if (idMember.MemberType == MemberTypes.Field)
                    {
                        var idField = (FieldInfo)idMember;

                        ReadValue = model =>
                        {
                            var id = idField.GetValue(model);

                            return id;
                        };

                        Name = idField.Name;
                        Type = idField.FieldType;
                    }
                    else
                    {
                        var idProperty = (PropertyInfo)idMember;

                        ReadValue = model =>
                        {
                            var id = idProperty.GetMethod.Invoke(model, new object[] { });

                            return id;
                        };

                        Name = idProperty.Name;
                        Type = idProperty.GetMethod.ReturnType;
                    }
                }
            }
        }
    }
}
