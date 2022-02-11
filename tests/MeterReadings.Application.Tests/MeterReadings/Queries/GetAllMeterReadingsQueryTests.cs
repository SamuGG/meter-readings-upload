using MeterReadings.Application.MeterReadings.Queries;
using MeterReadings.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MeterReadings.Application.Tests.MeterReadings.Queries
{
    public class GetAllMeterReadingsQueryTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public GetAllMeterReadingsQueryTests(DatabaseFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task GivenSomeMeterReadingsStored_WhenRunningQuery_ThenAllResultsAreReturned()
        {
            // Arrange
            var accounts = new AccountEntity[]
            {
                new AccountEntity { Id = 1, FirstName = "Adam", LastName = "Arlington" },
                new AccountEntity { Id = 2, FirstName = "Beth", LastName = "Boyle" },
                new AccountEntity { Id = 3, FirstName = "Charles", LastName = "Carlson" }
            };

            var meterReadings = new MeterReadingEntity[]
            {
                new MeterReadingEntity { Id = 1, AccountId = accounts[0].Id, ReadingDateTime = new DateTime(2022, 2, 10, 12, 10, 27), ReadingValue = 18291 },
                new MeterReadingEntity { Id = 2, AccountId = accounts[1].Id, ReadingDateTime = new DateTime(2022, 2, 9, 9, 54, 31), ReadingValue = 7582 },
                new MeterReadingEntity { Id = 3, AccountId = accounts[2].Id, ReadingDateTime = new DateTime(2022, 2, 9, 8, 7, 2), ReadingValue = 143 }
            };

            _fixture.SetUpAccounts(accounts);
            _fixture.SetUpMeterReadings(meterReadings);

            var queryHandler = new GetAllMeterReadingsRequestHandler(_fixture.Context);

            // Act
            var queryResult = await queryHandler.Handle(new GetAllMeterReadingsRequest(), CancellationToken.None)
                .ConfigureAwait(false);

            // Assert
            Assert.Collection(queryResult,
                elem => Assert.Equal(MapEntitiesToModel(accounts[0], meterReadings[0]), elem),
                elem => Assert.Equal(MapEntitiesToModel(accounts[1], meterReadings[1]), elem),
                elem => Assert.Equal(MapEntitiesToModel(accounts[2], meterReadings[2]), elem));
        }

        private static QueryMeterReadingModel MapEntitiesToModel(AccountEntity account, MeterReadingEntity meterReading) =>
            new(meterReading.AccountId, 
                account.FirstName, 
                account.LastName, 
                meterReading.ReadingDateTime, 
                meterReading.ReadingValue);

        public void Dispose()
        {
            _fixture.CleanUp();
            GC.SuppressFinalize(this);
        }
    }
}