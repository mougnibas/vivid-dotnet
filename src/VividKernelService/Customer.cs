// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;

namespace VividKernelService;

/// <summary>
/// A representation of a customer.
/// </summary>
public struct Customer : IEquatable<Customer>
{
    /// <summary>
    /// Unique identifier of the customer.
    /// </summary>
    /// <returns>Unique identifier of the customer.</returns>
    public string Id { get; set; }

    /// <summary>
    /// Secret of the customer.
    /// </summary>
    /// <returns>Secret of the customer.</returns>
    public string Secret { get; set; }

    /// <inheritdoc/>
    bool IEquatable<Customer>.Equals(Customer other)
    {
        return Id == other.Id && Secret == other.Secret;
    }
}
