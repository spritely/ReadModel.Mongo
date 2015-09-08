// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStoreException.cs">
//     Copyright (c) 2015. All rights reserved. Licensed under the MIT license. See LICENSE file in
//     the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Mongo
{
    using System;
    using System.Runtime.Serialization;

    public class DataStoreException : Exception
    {
        public DataStoreException()
        {
        }

        public DataStoreException(string message) : base(message)
        {
        }

        public DataStoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DataStoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
