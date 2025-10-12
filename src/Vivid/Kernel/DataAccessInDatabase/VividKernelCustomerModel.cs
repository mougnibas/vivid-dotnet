// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;

namespace Vivid.Kernel.DataAccessInDatabase
{
    /// <summary>
    /// A representation of a customer (Entity Framework Core model).
    /// </summary>
    public class VividKernelCustomerModel
    {
        /// <summary>
        /// Unique identifier of the customer.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Secret of the customer.
        /// </summary>
        public string Secret { get; set; }

        public VividKernelCustomerModel(string id, string secret)
        {
            Id = id;
            Secret = secret;
        }
    }
}
