using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordManager.API.Models;

namespace PasswordManager.API.Data.EntityConfigurations;

public class SocialCredentialConfig : IEntityTypeConfiguration<SocialCredential>
{
    public void Configure(EntityTypeBuilder<SocialCredential> builder)
    {
        builder.Property(c => c.Name)
            .HasConversion<string>();
    }
}
