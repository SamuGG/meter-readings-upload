using MediatR;
using MeterReadings.Application.Exceptions;
using MeterReadings.Application.MeterReadings;
using MeterReadings.Application.MeterReadings.Commands;
using MeterReadings.Application.Models;
using MeterReadings.Domain.Entities;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MeterReadings.Application.Tests.MeterReadings;

public class ImportServiceTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;

    public ImportServiceTests(DatabaseFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task GivenNewUserMeterReadingForAccount_WhenImportRuns_ThenCountsSuccessfulRecord()
    {
        // Arrange
        const int accountId = 1;

        _fixture.SetUpAccounts(new AccountEntity[]
        {
            new AccountEntity() { Id = accountId, FirstName = "Account", LastName = "Test" }
        });

        var models = new MeterReadingModel[]
        {
            new MeterReadingModel() { AccountId = accountId, ReadingDateTime = new DateTime(2022, 1, 12), ReadingValue = "727"}
        };

        var senderMock = new Mock<ISender>(MockBehavior.Strict);

        senderMock
            .Setup(x => x.Send(It.IsAny<CreateNewRecordRequest>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>(CreateNewRecordRequestCallback)
            .ReturnsAsync(Unit.Value);

        senderMock
            .Setup(x => x.Send(It.IsAny<UpdateRecordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var service = new ImportService(_fixture.Context, senderMock.Object);

        // Act
        var result = await service.ImportAsync(models, CancellationToken.None);

        // Assert
        Assert.Equal(1, result.SuccessfulRecordCount);
        Assert.Equal(0, result.FailedRecordCount);
    }
    
    [Fact]
    public async Task GivenNewUserMeterReadingWithoutAccount_WhenImportRuns_ThenCountsFailedRecord()
    {
        // Arrange
        var models = new MeterReadingModel[]
        {
            new MeterReadingModel() { AccountId = 1, ReadingDateTime = new DateTime(2022, 1, 12), ReadingValue = "727"}
        };

        var senderMock = new Mock<ISender>(MockBehavior.Strict);

        senderMock
            .Setup(x => x.Send(It.IsAny<CreateNewRecordRequest>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>(CreateNewRecordRequestCallback)
            .ReturnsAsync(Unit.Value);

        senderMock
            .Setup(x => x.Send(It.IsAny<UpdateRecordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var service = new ImportService(_fixture.Context, senderMock.Object);

        // Act
        var result = await service.ImportAsync(models, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.SuccessfulRecordCount);
        Assert.Equal(1, result.FailedRecordCount);
    }

    [Fact]
    public async Task GivenBadUserMeterReading_WhenImportRuns_ThenCountsFailedRecord()
    {
        // Arrange
        const int accountId = 1;

        _fixture.SetUpAccounts(new AccountEntity[]
        {
            new AccountEntity() { Id = accountId, FirstName = "Account", LastName = "Test" }
        });

        var models = new MeterReadingModel[]
        {
            new MeterReadingModel() { AccountId = accountId, ReadingDateTime = new DateTime(2022, 1, 12), ReadingValue = "void"}
        };

        var senderMock = new Mock<ISender>(MockBehavior.Strict);

        senderMock
            .Setup(x => x.Send(It.IsAny<CreateNewRecordRequest>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>(CreateNewRecordRequestCallback)
            .ReturnsAsync(Unit.Value);

        senderMock
            .Setup(x => x.Send(It.IsAny<UpdateRecordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var service = new ImportService(_fixture.Context, senderMock.Object);

        // Act
        var result = await service.ImportAsync(models, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.SuccessfulRecordCount);
        Assert.Equal(1, result.FailedRecordCount);
    }

    [Fact]
    public async Task GivenAnUpdatedUserMeterReading_WhenImportRuns_ThenCountsSuccessfulRecord()
    {
        // Arrange
        const int accountId = 1;

        _fixture.SetUpAccounts(new AccountEntity[]
        {
            new AccountEntity() { Id = accountId, FirstName = "Account", LastName = "Test" }
        });

        var models = new MeterReadingModel[]
        {
            new MeterReadingModel() { AccountId = accountId, ReadingDateTime = new DateTime(2022, 1, 12), ReadingValue = "727"},
            new MeterReadingModel() { AccountId = accountId, ReadingDateTime = new DateTime(2022, 1, 13), ReadingValue = "730"}
        };

        var senderMock = new Mock<ISender>(MockBehavior.Strict);

        senderMock
            .Setup(x => x.Send(It.IsAny<CreateNewRecordRequest>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>(CreateNewRecordRequestCallback)
            .ReturnsAsync(Unit.Value);

        senderMock
            .Setup(x => x.Send(It.IsAny<UpdateRecordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var service = new ImportService(_fixture.Context, senderMock.Object);

        // Act
        var result = await service.ImportAsync(models, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.SuccessfulRecordCount);
        Assert.Equal(0, result.FailedRecordCount);
    }

    [Fact]
    public async Task GivenAnOutdatedUserMeterReading_WhenImportRuns_ThenCountsFailedRecord()
    {
        // Arrange
        const int accountId = 1;

        _fixture.SetUpAccounts(new AccountEntity[]
        {
            new AccountEntity() { Id = accountId, FirstName = "Account", LastName = "Test" }
        });

        var models = new MeterReadingModel[]
        {
            new MeterReadingModel() { AccountId = accountId, ReadingDateTime = new DateTime(2022, 1, 12), ReadingValue = "727"},
            new MeterReadingModel() { AccountId = accountId, ReadingDateTime = new DateTime(2022, 1, 11), ReadingValue = "725"}
        };

        var senderMock = new Mock<ISender>(MockBehavior.Strict);

        senderMock
            .Setup(x => x.Send(It.IsAny<CreateNewRecordRequest>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>(CreateNewRecordRequestCallback)
            .ReturnsAsync(Unit.Value);

        senderMock
            .Setup(x => x.Send(It.IsAny<UpdateRecordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var service = new ImportService(_fixture.Context, senderMock.Object);

        // Act
        var result = await service.ImportAsync(models, CancellationToken.None);

        // Assert
        Assert.Equal(1, result.SuccessfulRecordCount);
        Assert.Equal(1, result.FailedRecordCount);
    }

    [Fact]
    public async Task GivenSomeUserMeterReadings_WhenImportRuns_ThenReturnsSuccessfulAndFailedCount()
    {
        // Arrange
        _fixture.SetUpAccounts(
            Enumerable.Range(1, 3)
            .Select(id => new AccountEntity()
            {
                Id = id,
                FirstName = $"Account{id}",
                LastName = "Test"
            }));

        var models = new MeterReadingModel[]
        {
            new MeterReadingModel() { AccountId = 1, ReadingDateTime = new DateTime(2022, 1, 12), ReadingValue = "727"},   // Expected: success (new record)
            new MeterReadingModel() { AccountId = 2, ReadingDateTime = new DateTime(2022, 1, 13), ReadingValue = "1552"},  // Expected: success (new record)
            new MeterReadingModel() { AccountId = 3, ReadingDateTime = new DateTime(2022, 1, 14), ReadingValue = "645"},   // Expected: success (new record)
            new MeterReadingModel() { AccountId = 2, ReadingDateTime = new DateTime(2022, 1, 12), ReadingValue = "1550"},  // Expected: failure (outdated)
            new MeterReadingModel() { AccountId = 1, ReadingDateTime = new DateTime(2022, 1, 13), ReadingValue = "730"},   // Expected: success (update)
            new MeterReadingModel() { AccountId = 4, ReadingDateTime = new DateTime(2022, 1, 12), ReadingValue = "1"},     // Expected: failure (account not found)
            new MeterReadingModel() { AccountId = 2, ReadingDateTime = new DateTime(2022, 1, 14), ReadingValue = "void"}   // Expected: failure (bad reading)
        };

        var senderMock = new Mock<ISender>(MockBehavior.Strict);

        senderMock
            .Setup(x => x.Send(It.IsAny<CreateNewRecordRequest>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>(CreateNewRecordRequestCallback)
            .ReturnsAsync(Unit.Value);

        senderMock
            .Setup(x => x.Send(It.IsAny<UpdateRecordRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var service = new ImportService(_fixture.Context, senderMock.Object);

        // Act
        var result = await service.ImportAsync(models, CancellationToken.None);

        // Assert
        Assert.Equal(4, result.SuccessfulRecordCount);
        Assert.Equal(3, result.FailedRecordCount);
    }

    private void CreateNewRecordRequestCallback(IRequest<Unit> unitRequest, CancellationToken _)
    {
        var request = (CreateNewRecordRequest)unitRequest;

        if (!_fixture.Context.Accounts.Any(account => account.Id == request.MeterReadingModel.AccountId))
            throw new AccountNotFoundException(request.MeterReadingModel.AccountId);

        _fixture.Context.MeterReadings.Add(
            new MeterReadingEntity()
            {
                AccountId = request.MeterReadingModel.AccountId,
                ReadingDateTime = request.MeterReadingModel.ReadingDateTime
            });

        _fixture.Context.SaveChanges();
    }

    public void Dispose()
    {
        _fixture.CleanUp();
        GC.SuppressFinalize(this);
    }
}