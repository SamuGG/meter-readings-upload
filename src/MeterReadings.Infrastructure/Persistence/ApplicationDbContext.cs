using MeterReadings.Application.Interfaces;
using MeterReadings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MeterReadings.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<AccountEntity> Accounts => Set<AccountEntity>();
    public DbSet<MeterReadingEntity> MeterReadings => Set<MeterReadingEntity>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}