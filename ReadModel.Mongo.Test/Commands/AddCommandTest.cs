// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddCommandTest.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo.Test
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using NUnit.Framework;

    [TestFixture]
    public class AddCommandTest
    {
        [Test]
        public void Adds_record_to_database()
        {
            var testReadModelDatabase = new TestReadModelDatabase();

            var model = new TestModel()
            {
                Id = Guid.NewGuid(),
                Name = "Adds_record_to_database"
            };

            var addTestModel = Create.AddOneCommand<TestReadModelDatabase, TestModel, NullMetadata>(testReadModelDatabase);

            addTestModel(model, null);

            var database = testReadModelDatabase.CreateConnection();

            var search =
                database.GetCollection<StorageModel<TestModel, NullMetadata>>("TestModel").Aggregate().Match(sm => sm.Model.Id == model.Id);
            var task = Task.Run(() => search.ToListAsync());
            task.Wait();

            Assert.That(task.Result, Is.Not.Empty);
            var result = task.Result.Single();
            Assert.That(result.Model.Name, Is.EqualTo(model.Name));

            Task.Run(() => database.DropCollectionAsync("TestModel")).Wait();
        }

        [Test]
        public void Adds_record_to_database2()
        {
            var testReadModelDatabase = new TestReadModelDatabase();

            var model = new TestModel()
            {
                Id = Guid.NewGuid(),
                Name = "Adds_record_to_database"
            };

            var addTestModel = Create.AddOneCommand<TestReadModelDatabase, TestModel, NullMetadata>(testReadModelDatabase);

            addTestModel(model, null);

            var getManyCommand = Create.GetManyQuery<TestReadModelDatabase, TestModel, NullMetadata>(testReadModelDatabase);

            var result = getManyCommand(sm => sm.Model.Id == model.Id).Single();

            Assert.That(result.Name, Is.EqualTo(model.Name));

            /*var command = new AddCommand<TestModel, NullMetadata>
            {
                Model = model
            };

            var commandHandler = new AddCommandHandler<TestReadModelDatabase, TestModel, NullMetadata>(testReadModelDatabase);

            commandHandler.Handle(command);*/

            /*var addTestModels = Create.AddManyCommand<TestReadModelDatabase, TestModel, NullMetadata>(testReadModelDatabase);

            var testModels = new List<TestModel>
            {
                new TestModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Test 1"
                },
                new TestModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Test 2"
                }
            };

            var storageModels = testModels.Select(tm => new StorageModel<TestModel, NullMetadata> { Model = tm });

            addTestModels(storageModels);
            */

            //var storageModel = new StorageModel<TestModel, TestModelMetadata>();

            /*var testModel = new TestModel()
            {
                Id = Guid.NewGuid(),
                Name = "Hello world"
            };

            var metadata = new TestModelMetadata()
            {
                Ancestors = new[] { 5, 10 }
            };

            var addNewModel = Create.AddOneCommand<TestReadModelDatabase, TestModel, TestModelMetadata>(testReadModelDatabase);
            addNewModel(testModel, metadata);

            GetManyQuery<TestModel, TestModelMetadata> getMyEntities;// = Create.GetManyQuery<TestReadModelDatabase, TestModel, TestModelMetadata>(testReadModelDatabase);
            var myEntities = getMyEntities((model, metadata) => model.Name.StartsWith("J") && metadata.);
            foreach (var entity in myEntities)
            {
            }*/
        }
    }

    /*public class TestModelMetadata
    {
        public int[] Ancestors { get; set; }
    }*/
}
