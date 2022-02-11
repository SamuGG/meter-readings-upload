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
    public class CreateNewRecordCommandTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public CreateNewRecordCommandTests(DatabaseFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task GivenAModelForAnAccount_WhenCommandRuns_ThenNewMeterReadingIsRecorded()
        {
            // Arrange
            var accounts = new AccountEntity[]
            {
                new AccountEntity { Id = 12, FirstName = "Adam", LastName = "Arlington" }
            };
            _fixture.SetUpAccounts(accounts);

            var queryHandler = new CreateNewRecordRequestHandler(_fixture.Context);
            var model = new MeterReadingModel()
            {
                AccountId = accounts[0].Id,
                ReadingDateTime = new DateTime(2022, 2, 5, 8, 41, 23),
                ReadingValue = "12345"
            };

            // Act
            await queryHandler.Handle(new CreateNewRecordRequest(model), CancellationToken.None).ConfigureAwait(false);

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
            var queryHandler = new CreateNewRecordRequestHandler(_fixture.Context);
            var model = new MeterReadingModel()
            {
                AccountId = 33,
                ReadingDateTime = new DateTime(2022, 2, 5, 8, 41, 23),
                ReadingValue = "235"
            };

            // Act + Assert
            await Assert.ThrowsAsync<AccountNotFoundException>(() => 
                queryHandler.Handle(new CreateNewRecordRequest(model), CancellationToken.None))
            .ConfigureAwait(false);
        }

        public void Dispose()
        {
            _fixture.CleanUp();
            GC.SuppressFinalize(this);
        }
    }
}