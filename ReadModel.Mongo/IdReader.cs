// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdReader.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;

    internal class IdReader<TModel>
    {
        public IdReader()
        {
            // only instance members: priorities: public, then protected, then internal, then private (check all names first) properties then fields
            //"Id", "id", "_id"
            var idProperty = typeof(TModel).GetProperty("Id");

            if (idProperty == null || !idProperty.CanRead)
            {
                throw new ArithmeticException($"{nameof(TModel)} does not have an Id property or it is not readable (set only)");
            }

            var idType = idProperty.GetMethod.ReturnType;

            Read = model =>
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var id = idProperty.GetMethod.Invoke(model, new object[] { });

                return id;
            };
            IdType = idType;
        }

        public Type IdType { get; set; }

        public Func<TModel, object> Read { get; set; }
    }
}
