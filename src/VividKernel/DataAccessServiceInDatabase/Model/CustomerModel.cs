// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;
using System.Globalization;

namespace VividKernelService.Model;

/// <summary>
/// A representation of a customer (Entity Framework Core model).
/// </summary>
public class CustomerModel
{
    /// <summary>
    /// Unique identifier of the customer.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Secret of the customer.
    /// </summary>
    public string Secret { get; set; }

    public CustomerModel(string id, string secret)
    {
        Id = id;
        Secret = secret;
    }
}
