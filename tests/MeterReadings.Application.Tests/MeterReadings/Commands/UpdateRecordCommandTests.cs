using MeterReadings.Application.Exceptions;
using MeterReadings.Application.MeterReadings.Commands;
using MeterReadings.Application.Models;
using MeterReadings.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MeterReadings.Application.Tests.MeterReadings.Queries
{
    public class UpdateRecordCommandTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public UpdateRecordCommandTests(DatabaseFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task GivenAModelForAnAccount_WhenCommandRuns_ThenMeterReadingIsUpdated()
        {
            // Arrange
            var accounts = new AccountEntity[]
            {
                new AccountEntity { Id = 12, FirstName = "Adam", LastName = "Arlington" }
            };
            
            var meterReadings = new MeterReadingEntity[]
            {
                new MeterReadingEntity { Id = 15, AccountId = accounts[0].Id, ReadingDateTime = new DateTime(2022, 2, 10, 12, 10, 27), ReadingValue = 18291 }
            };

            _fixture.SetUpAccounts(accounts);
            _fixture.SetUpMeterReadings(meterReadings);

            var queryHandler = new UpdateRecordRequestHandler(_fixture.Context);
            var model = new MeterReadingModel()
            {
                AccountId = accounts[0].Id,
                ReadingDateTime = meterReadings[0].ReadingDateTime.AddMinutes(5),
                ReadingValue = "18292"
            };

            // Act
            await queryHandler.Handle(new UpdateRecordRequest(meterReadings[0], model), CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.NotNull(_fixture.Context.MeterReadings.Single(meterReading =>
                meterReading.AccountId == model.AccountId
                && meterReading.ReadingDateTime == model.ReadingDateTime
                && meterReading.ReadingValue == model.GetNumericReadingValue()));
        }


        [Fact]
        public async Task GivenAModelWithoutAccount_WhenCommandRuns_ThenExceptionIsRaised()
        {
            // Arrange
            var queryHandler = new UpdateRecordRequestHandler(_fixture.Context);
            var model = new MeterReadingModel()
            {
                AccountId = 33,
                ReadingDateTime = new DateTime(2022, 2, 5, 8, 41, 23),
                ReadingValue = "235"
            };

            // Act + Assert
            await Assert.ThrowsAsync<AccountNotFoundException>(() => 
                queryHandler.Handle(new UpdateRecordRequest(new MeterReadingEntity(), model), CancellationToken.None))
            .ConfigureAwait(false);
        }

        public void Dispose()
        {
            _fixture.CleanUp();
            GC.SuppressFinalize(this);
        }
    }
}