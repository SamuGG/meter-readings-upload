using MeterReadings.Domain.Entities;
using MeterReadings.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace MeterReadings.Application.Tests;

public class DatabaseFixture : IDisposable
{
    public ApplicationDbContext Context { get; init; }

    public DatabaseFixture()
    {
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDbContext(contextOptions);
    }

    public void SetUpAccounts(IEnumerable<AccountEntity> accounts)
    {
        Context.Accounts.AddRange(accounts);
        Context.SaveChanges();
    }

    public void SetUpMeterReadings(IEnumerable<MeterReadingEntity> meterReadings)
    {
        Context.MeterReadings.AddRange(meterReadings);
        Context.SaveChanges();
    }

    public void CleanUp()
    {
        Context.RemoveRange(Context.MeterReadings);
        Context.RemoveRange(Context.Accounts);
        Context.SaveChanges();
    }

    public void Dispose()
    {
        if (Context != null)
            Context.Dispose();
        
        GC.SuppressFinalize(this);
    }
}