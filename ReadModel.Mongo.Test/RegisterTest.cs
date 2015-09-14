// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    [TestFixture]
    public class RegisterTest
    {
        [Test]
        public void Queries_calls_register_with_expected_model_types()
        {
            var calls = new Dictionary<Type, Func<TestReadModelDatabase, object>>();
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { calls.Add(type, getType); };

            var database = new TestReadModelDatabase();

            Register.Queries(register, typeof(TestModel));

            Assert.That(calls.Count, Is.EqualTo(3));
            AssertContainsType<GetOneQueryAsync<TestModel>>(calls, database);
            AssertContainsType<GetManyQueryAsync<TestModel>>(calls, database);
            AssertContainsType<GetAllQueryAsync<TestModel>>(calls, database);
        }

        [Test]
        public void Queries_calls_register_with_expected_storage_model_types()
        {
            var calls = new Dictionary<Type, Func<TestReadModelDatabase, object>>();
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { calls.Add(type, getType); };

            var database = new TestReadModelDatabase();

            Register.Queries(register, typeof(StorageModel<TestModel, TestMetadata>));

            Assert.That(calls.Count, Is.EqualTo(3));
            AssertContainsType<GetOneQueryAsync<TestModel, TestMetadata>>(calls, database);
            AssertContainsType<GetManyQueryAsync<TestModel, TestMetadata>>(calls, database);
            AssertContainsType<GetAllQueryAsync<TestModel, TestMetadata>>(calls, database);
        }

        [Test]
        public void Queries_calls_register_with_expected_model_types_for_mixed_types()
        {
            var calls = new Dictionary<Type, Func<TestReadModelDatabase, object>>();
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { calls.Add(type, getType); };

            var database = new TestReadModelDatabase();

            Register.Queries(register, typeof(TestModel), typeof(StorageModel<TestModel, TestMetadata>));

            Assert.That(calls.Count, Is.EqualTo(6));
            AssertContainsType<GetOneQueryAsync<TestModel>>(calls, database);
            AssertContainsType<GetManyQueryAsync<TestModel>>(calls, database);
            AssertContainsType<GetAllQueryAsync<TestModel>>(calls, database);
            AssertContainsType<GetOneQueryAsync<TestModel, TestMetadata>>(calls, database);
            AssertContainsType<GetManyQueryAsync<TestModel, TestMetadata>>(calls, database);
            AssertContainsType<GetAllQueryAsync<TestModel, TestMetadata>>(calls, database);
        }

        [Test]
        public void Projectors_calls_register_with_expected_model_types()
        {
            var calls = new Dictionary<Type, Func<TestReadModelDatabase, object>>();
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { calls.Add(type, getType); };

            var database = new TestReadModelDatabase();

            Register.Projectors(register, typeof(TestModel));

            Assert.That(calls.Count, Is.EqualTo(1));
            AssertContainsType<Projector<TestReadModelDatabase, TestModel>>(calls, database);
        }

        [Test]
        public void Projectors_calls_register_with_expected_storage_model_types()
        {
            var calls = new Dictionary<Type, Func<TestReadModelDatabase, object>>();
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { calls.Add(type, getType); };

            var database = new TestReadModelDatabase();

            Register.Projectors(register, typeof(StorageModel<TestModel, TestMetadata>));

            Assert.That(calls.Count, Is.EqualTo(1));
            AssertContainsType<Projector<TestReadModelDatabase, TestModel, TestMetadata>>(calls, database);
        }

        [Test]
        public void Projectors_calls_register_with_expected_model_types_for_mixed_types()
        {
            var calls = new Dictionary<Type, Func<TestReadModelDatabase, object>>();
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { calls.Add(type, getType); };

            var database = new TestReadModelDatabase();

            Register.Projectors(register, typeof(TestModel), typeof(StorageModel<TestModel, TestMetadata>));

            Assert.That(calls.Count, Is.EqualTo(2));
            AssertContainsType<Projector<TestReadModelDatabase, TestModel>>(calls, database);
            AssertContainsType<Projector<TestReadModelDatabase, TestModel, TestMetadata>>(calls, database);
        }

        [Test]
        public void Commands_calls_register_with_expected_model_types()
        {
            var calls = new Dictionary<Type, Func<TestReadModelDatabase, object>>();
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { calls.Add(type, getType); };

            var database = new TestReadModelDatabase();

            Register.Commands(register, typeof(TestModel));

            Assert.That(calls.Count, Is.EqualTo(8));
            AssertContainsType<AddOneCommandAsync<TestModel>>(calls, database);
            AssertContainsType<AddManyCommandAsync<TestModel>>(calls, database);

            AssertContainsType<RemoveOneCommandAsync<TestModel>>(calls, database);
            AssertContainsType<RemoveManyCommandAsync<TestModel>>(calls, database);
            AssertContainsType<RemoveAllCommandAsync>(calls, database);
            AssertContainsType<RemoveAllCommandAsync<TestModel>>(calls, database);

            AssertContainsType<UpdateOneCommandAsync<TestModel>>(calls, database);
            AssertContainsType<UpdateManyCommandAsync<Guid, TestModel>>(calls, database);
        }

        [Test]
        public void Commands_calls_register_with_expected_storage_model_types()
        {
            var calls = new Dictionary<Type, Func<TestReadModelDatabase, object>>();
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { calls.Add(type, getType); };

            var database = new TestReadModelDatabase();

            Register.Commands(register, typeof(StorageModel<TestModel, TestMetadata>));

            Assert.That(calls.Count, Is.EqualTo(8));
            AssertContainsType<AddOneCommandAsync<TestModel, TestMetadata>>(calls, database);
            AssertContainsType<AddManyCommandAsync<TestModel, TestMetadata>>(calls, database);

            AssertContainsType<RemoveOneCommandAsync<TestModel>>(calls, database);
            AssertContainsType<RemoveManyCommandAsync<TestModel, TestMetadata>>(calls, database);
            AssertContainsType<RemoveAllCommandAsync>(calls, database);
            AssertContainsType<RemoveAllCommandAsync<TestModel>>(calls, database);

            AssertContainsType<UpdateOneCommandAsync<TestModel, TestMetadata>>(calls, database);
            AssertContainsType<UpdateManyCommandAsync<Guid, TestModel, TestMetadata>>(calls, database);
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling",
            Justification =
                "Method is designed to test dependency injection scenario which is designed for managing class coupling scenarios.")]
        [Test]
        public void Commands_calls_register_with_expected_model_types_for_mixed_types()
        {
            var calls = new Dictionary<Type, Func<TestReadModelDatabase, object>>();
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { calls.Add(type, getType); };

            var database = new TestReadModelDatabase();

            Register.Commands(register, typeof(TestModel), typeof(StorageModel<TestModel, TestMetadata>));

            Assert.That(calls.Count, Is.EqualTo(13));
            AssertContainsType<AddOneCommandAsync<TestModel>>(calls, database);
            AssertContainsType<AddManyCommandAsync<TestModel>>(calls, database);
            AssertContainsType<RemoveOneCommandAsync<TestModel>>(calls, database);
            AssertContainsType<RemoveManyCommandAsync<TestModel>>(calls, database);
            AssertContainsType<RemoveAllCommandAsync>(calls, database);
            AssertContainsType<RemoveAllCommandAsync<TestModel>>(calls, database);
            AssertContainsType<UpdateOneCommandAsync<TestModel>>(calls, database);
            AssertContainsType<UpdateManyCommandAsync<Guid, TestModel>>(calls, database);

            AssertContainsType<AddOneCommandAsync<TestModel, TestMetadata>>(calls, database);
            AssertContainsType<AddManyCommandAsync<TestModel, TestMetadata>>(calls, database);
            AssertContainsType<RemoveManyCommandAsync<TestModel, TestMetadata>>(calls, database);
            AssertContainsType<UpdateOneCommandAsync<TestModel, TestMetadata>>(calls, database);
            AssertContainsType<UpdateManyCommandAsync<Guid, TestModel, TestMetadata>>(calls, database);
        }

        [Test]
        public void Queries_throws_on_invalid_arguments()
        {
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { };

            Assert.That(() => Register.Queries<TestReadModelDatabase>(null, typeof(TestModel)), Throws.TypeOf<ArgumentNullException>());
            Assert.That(() => Register.Queries(register, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Commands_throws_on_invalid_arguments()
        {
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { };

            Assert.That(() => Register.Commands<TestReadModelDatabase>(null, typeof(TestModel)), Throws.TypeOf<ArgumentNullException>());
            Assert.That(() => Register.Commands(register, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Projectors_throws_on_invalid_arguments()
        {
            Action<Type, Func<TestReadModelDatabase, object>> register = (type, getType) => { };

            Assert.That(() => Register.Projectors<TestReadModelDatabase>(null, typeof(TestModel)), Throws.TypeOf<ArgumentNullException>());
            Assert.That(() => Register.Projectors(register, null), Throws.TypeOf<ArgumentNullException>());
        }

        private static void AssertContainsType<T>(
            IDictionary<Type, Func<TestReadModelDatabase, object>> calls,
            TestReadModelDatabase database)
        {
            Assert.That(calls.ContainsKey(typeof(T)));
            Assert.That(calls[typeof(T)], Is.Not.Null);
            Assert.That(calls[typeof(T)](database), Is.InstanceOf(typeof(T)));
        }
    }
}
