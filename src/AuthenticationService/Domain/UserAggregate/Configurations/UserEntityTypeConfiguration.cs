using Domain.UserAggregate.Entities;
using Domain.UserAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.UserAggregate.Configurations;

/// <summary>
/// Configuration aggregate User and entities of user owners
/// Used for configure database table with Entity Framework Core
/// </summary>
public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    private const string UserTableName = "users";
    private const string DataTableName = "user_data";
    private const string ProfileTableName = "user_profiles";
    private const string UserPrimaryKey = "pk_users";
    private const string ProfilePrimaryKey = "pk_profiles";
    private const string DataPrimaryKey = "pk_users_data";
    private const string UserDataForeignKey = "fk_user_data";
    private const string UserProfileForeignKey = "fk_user_profile";
    private const string UserIdColumnName = "user_id";
    private const string UserIdIndexName = "idx_user_id";
    private const string UsernameIndexName = "idx_username";
    private const string EmailIndexName = "idx_email";
    private const int StatusMaxLength = 20;
    private const int RoleMaxLength = 20;
    private const int NamesMaxLength = 50;
    private const int EmailMaxLength = 50;
    private const int ImageUrlMaxLength = 255;
    private const int GenderMaxLength = 16;
    private const int WebsiteMaxLength = 50;
    private const int LocationMaxLength = 100;


    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(UserTableName);
        builder.HasKey(u => u.Id).HasName(UserPrimaryKey);
        builder.Property(u => u.Id)
            .HasConversion(property => property.Value, value => UserId.Create(value))
            .HasColumnName(UserIdColumnName)
            .ValueGeneratedNever()
            .IsRequired();
        builder.Property(u => u.Status)
            .HasConversion(property => property.Value, value => Status.Create(value))
            .HasMaxLength(StatusMaxLength)
            .IsRequired();
        builder.Property(u => u.Role)
            .HasConversion(property => property.Value, value => Role.Create(value))
            .HasMaxLength(RoleMaxLength)
            .IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.UpdatedAt).IsRequired();
        builder.OwnsOne(u => u.Data, DataConfigure);
        builder.OwnsOne(u => u.Profile, ProfileConfigure);
        builder.HasIndex(u => u.Id, UserIdIndexName);
    }

    private void DataConfigure(OwnedNavigationBuilder<User, Data> builder)
    {
        builder.ToTable(DataTableName);
        builder.WithOwner().HasForeignKey(x => x.Id).HasConstraintName(UserDataForeignKey);
        builder.HasKey(x => x.Id).HasName(DataPrimaryKey);
        builder.Property(u => u.Id)
            .HasConversion(property => property.Value, value => UserId.Create(value))
            .HasColumnName(UserIdColumnName)
            .ValueGeneratedNever()
            .IsRequired();
        builder.Property(u => u.Username)
            .HasConversion(property => property.Value, value => Username.Create(value))
            .HasMaxLength(NamesMaxLength)
            .IsRequired();
        builder.Property(u => u.Email)
            .HasConversion(property => property.Value, value => Email.Create(value))
            .HasMaxLength(EmailMaxLength)
            .IsRequired();
        builder.Property(u => u.Password)
            .HasConversion(property => property.Value, value => Password.Create(value))
            .IsRequired();
        builder.Property(u => u.Salt)
            .HasConversion(property => property.Value, value => Salt.Create(value))
            .IsRequired();

        builder.HasIndex(u => u.Username).HasDatabaseName(UsernameIndexName);
        builder.HasIndex(u => u.Email).HasDatabaseName(EmailIndexName);
    }

    private void ProfileConfigure(OwnedNavigationBuilder<User, Profile> builder)
    {
        builder.ToTable(ProfileTableName);
        builder.WithOwner().HasForeignKey(x => x.Id).HasConstraintName(UserProfileForeignKey);
        builder.HasKey(x => x.Id).HasName(ProfilePrimaryKey);
        builder.Property(u => u.Id)
            .HasConversion(property => property.Value, value => UserId.Create(value))
            .HasColumnName(UserIdColumnName)
            .ValueGeneratedNever()
            .IsRequired();
        builder.Property(u => u.FirstName)
            .HasConversion(property => property.Value, value => FirstName.Create(value))
            .HasMaxLength(NamesMaxLength)
            .IsRequired();
        builder.Property(u => u.LastName)
            .HasConversion(property => property.Value, value => LastName.Create(value))
            .HasMaxLength(NamesMaxLength)
            .IsRequired();
        builder.Property(u => u.ProfilePicture)
            .HasConversion(property => property.Value, value => ProfilePicture.Create(value))
            .HasMaxLength(ImageUrlMaxLength)
            .IsRequired();
        builder.Property(u => u.CoverPicture)
            .HasConversion(property => property.Value, value => CoverPicture.Create(value))
            .HasMaxLength(ImageUrlMaxLength)
            .IsRequired();
        builder.Property(u => u.Gender)
            .HasConversion(property => property.Value, value => Gender.Create(value))
            .HasMaxLength(GenderMaxLength)
            .IsRequired();
        builder.Property(u => u.Website)
            .HasConversion(property => property.Value, value => Website.Create(value))
            .HasMaxLength(WebsiteMaxLength)
            .IsRequired();
        builder.Property(u => u.Location)
            .HasConversion(property => property.Value, value => Location.Create(value))
            .HasMaxLength(LocationMaxLength)
            .IsRequired();
        builder.Property(u => u.Bio)
            .HasConversion(property => property.Value, value => Bio.Create(value))
            .IsRequired();
        builder.Property(u => u.Birthday)
            .HasConversion(property => property.Value, value => Birthday.Create(value))
            .IsRequired();
    }
}