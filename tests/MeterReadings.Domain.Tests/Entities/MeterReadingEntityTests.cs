using MeterReadings.Domain.Entities;
using System;
using Xunit;

namespace MeterReadings.Domain.Tests.Entities
{
    public class MeterReadingEntityTests
    {
        private static readonly DateTime PresentDateTime = new(2022, 2, 10, 9, 45, 25);
        private static readonly DateTime PreviousDateTime = new(2022, 1, 30, 23, 1, 58);

        public static TheoryData<DateTime, DateTime, bool> IsNewerThan_TestData => new()
        {
            { PresentDateTime, PresentDateTime, false },
            { PreviousDateTime, PresentDateTime, false },
            { PresentDateTime, PreviousDateTime, true }
        };

        [Theory]
        [MemberData(nameof(IsNewerThan_TestData))]
        public void GivenADateOlderThanReadingDate_WhenIsNewerThanRuns_ThenReturnsTrue(DateTime readingDateTime, DateTime comparisonDateTime, bool expected) =>
            Assert.Equal(expected, new MeterReadingEntity() { ReadingDateTime = readingDateTime }.IsNewerThan(comparisonDateTime));
    }
}