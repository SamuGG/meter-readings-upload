using MeterReadings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeterReadings.Infrastructure.Persistence.Configurations;

public class AccountEntityConfiguration : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.Property(account => account.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(account => account.FirstName)
            .HasMaxLength(200)
            .IsRequired();
           
        builder.Property(account => account.LastName)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasKey(account => account.Id);
    }
}