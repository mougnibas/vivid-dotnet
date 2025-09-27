// Version 3, 19 November 2007
//
// Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
// Everyone is permitted to copy and distribute verbatim copies
// of this license document, but changing it is not allowed.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vivid.Kernel.Service;

namespace VividTest.Kernel.Service
{
    /// <summary>
    /// Unit tests of ``Customer`` struct.
    /// </summary>
    [TestClass]
    [TestCategory("Unit")]
    public sealed class VividTestKernelCustomer
    {
        [DataTestMethod]
        [DataRow("")]
        [DataRow("my-awesome-id")]
        public void FullConstructorWithThisGivenIdShouldHaveTheSameId(string id)
        {
            // Arrange.
            string expected = id;
            VividKernelCustomer customer = new VividKernelCustomer { Id = id, Secret = "secret" };

            // Act.
            string actual = customer.Id;

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("my-awesome-secret")]
        public void FullConstructorWithThisGivenSecretShouldHaveTheSameSecret(string secret)
        {
            // Arrange.
            string expected = secret;
            VividKernelCustomer customer = new VividKernelCustomer { Id = "id", Secret = secret };

            // Act.
            string actual = customer.Secret;

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("", "", "", "")]
        [DataRow("my-awesome-id", "my-awesome-secret", "my-awesome-id", "my-awesome-secret")]
        public void CustomersWithSameFieldsAreEqual(string id1, string secret1, string id2, string secret2)
        {
            // Arrange.
            VividKernelCustomer customerOne = new VividKernelCustomer { Id = id1, Secret = secret1 };
            VividKernelCustomer customerTwo = new VividKernelCustomer { Id = id2, Secret = secret2 };

            // Act.
            bool actual = customerOne.Equals(customerTwo);

            // Assert.
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void ComparingCustomerAndSomethingElseShouldReturnFalse()
        {
            // Arrange.
            VividKernelCustomer customer = new VividKernelCustomer { Id = "my-id", Secret = "my-secret" };
            string notACustomer = "I'm not a customer";

            // Act.
            bool actual = customer.Equals(notACustomer);

            // Assert.
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ComparingCustomerAndAnotherCustomerCastedAsObjectShouldReturnTrue()
        {
            // Arrange.
            VividKernelCustomer customerOne = new VividKernelCustomer { Id = "my-id", Secret = "my-secret" };
            VividKernelCustomer customerTwo = new VividKernelCustomer { Id = "my-id", Secret = "my-secret" };
            object anotherCustomer = customerTwo;

            // Act.
            bool actual = customerOne.Equals(anotherCustomer);

            // Assert.
            Assert.IsTrue(actual);
        }

        [DataTestMethod]
        [DataRow("", "", "", "")]
        [DataRow("my-awesome-id", "my-awesome-secret", "my-awesome-id", "my-awesome-secret")]
        public void OperatorEqualsReturnsTrueForEqualCustomers(string id1, string secret1, string id2, string secret2)
        {
            // Arrange.
            VividKernelCustomer customerOne = new VividKernelCustomer { Id = id1, Secret = secret1 };
            VividKernelCustomer customerTwo = new VividKernelCustomer { Id = id2, Secret = secret2 };

            // Act.
            bool actual = customerOne == customerTwo;

            // Assert.
            Assert.IsTrue(actual);
        }

        [DataTestMethod]
        [DataRow("a", "c", "b", "d")]
        [DataRow("my-awesome-id-1", "my-awesome-secret-1", "my-awesome-id-2", "my-awesome-secret-2")]
        public void CustomersWithDifferentFieldsAreNotEqual(string id1, string secret1, string id2, string secret2)
        {
            // Arrange.
            VividKernelCustomer customerOne = new VividKernelCustomer { Id = id1, Secret = secret1 };
            VividKernelCustomer customerTwo = new VividKernelCustomer { Id = id2, Secret = secret2 };

            // Act.
            bool actual = customerOne.Equals(customerTwo);

            // Assert.
            Assert.IsFalse(actual);
        }

        [DataTestMethod]
        [DataRow("a", "c", "b", "d")]
        [DataRow("my-awesome-id-1", "my-awesome-secret-1", "my-awesome-id-2", "my-awesome-secret-2")]
        public void OperatorNotEqualsReturnsTrueForDifferentCustomers(string id1, string secret1, string id2, string secret2)
        {
            // Arrange.
            VividKernelCustomer customerOne = new VividKernelCustomer { Id = id1, Secret = secret1 };
            VividKernelCustomer customerTwo = new VividKernelCustomer { Id = id2, Secret = secret2 };

            // Act.
            bool actual = customerOne != customerTwo;

            // Assert.
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void ToStringShouldReturnFormattedString()
        {
            // Arrange.
            VividKernelCustomer customer = new VividKernelCustomer { Id = "my-id", Secret = "my-secret" };
            string expected = "VividKernelCustomer(Id='my-id', Secret='my-secret')";

            // Act.
            string actual = customer.ToString();

            // Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetHashCodeShouldReturnHashCodeBasedOnFields()
        {
            // Arrange: hash code is based on the Id and Secret fields.
            VividKernelCustomer customer = new VividKernelCustomer { Id = "my-id", Secret = "my-secret" };
            int expected = HashCode.Combine("my-id", "my-secret");

            // Act.
            int actual = customer.GetHashCode();

            // Assert.
            Assert.AreEqual(expected, actual);
        }
    }
}
