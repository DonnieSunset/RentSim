﻿using FinanceMathService;
using FinanceMathService.DTOs;
using NUnit.Framework;

namespace FinanceMathService_Tests
{
    [TestFixture]
    internal class FinanceMath_uTests
    {
        private FinanceMath financeMath = new FinanceMath();

        [TestCase(999, 1000, 1000, 0)]
        [TestCase(1000, 1000, 1000, 0)]
        [TestCase(4000, 300, 100, 0)]
        [TestCase(4000, 1000, 1000, -263.75)]
        [TestCase(4000, 1000, 3000, -263.75*2)]
        public void CalculateStocksTaxes_ValidInput_CorrectTaxes(
            decimal withdrawalAmount, 
            decimal referenceAmountFromDeposits, 
            decimal referenceAmountFromInterests,
            decimal expectedTaxes)
        {
            CAmount totalAmount = new CAmount { FromDeposits = referenceAmountFromDeposits, FromInterests = referenceAmountFromInterests };

            var taxes = financeMath.CalculateStocksTaxes(withdrawalAmount, totalAmount);
            
            Assert.That(taxes, Is.EqualTo(expectedTaxes).Within(FinanceMath.Precision));
        }
    }
}
