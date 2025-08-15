// GNU AFFERO GENERAL PUBLIC LICENSE
// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;
using System.Globalization;

namespace VividKernelService;

/// <summary>
/// A representation of a customer.
/// </summary>
public struct Customer : IEquatable<Customer>
{
    /// /// <summary>
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
    public bool Equals(Customer other)
    {
        return string.Equals(Id, other.Id, StringComparison.Ordinal)
            && string.Equals(Secret, other.Secret, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Customer other && Equals(other);
    }

    /// <inheritdoc/>
    public static bool operator ==(Customer left, Customer right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc/>
    public static bool operator !=(Customer left, Customer right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Secret);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "Customer(Id='{0}', Secret='{1}')", Id, Secret);
    }
}
