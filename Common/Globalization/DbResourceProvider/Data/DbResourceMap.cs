

namespace Avantech.Common.Globalization.DbResourceProvider.Data
{
    internal class DbResourceMap : EntityTypeConfiguration<DbResource>
    {
        public DbResourceMap()
        {
            // Unique Key
            HasKey(x => new { x.ResourceType, x.ResourceKey, x.CultureCode });

            // Properties
            Property(x => x.ResourceType)
                .IsRequired()
                .HasMaxLength(255);

            Property(x => x.CultureCode)
                .IsRequired()
                .HasMaxLength(20);

            Property(x => x.ResourceKey)
                .IsRequired()
                .HasMaxLength(128);

            Property(x => x.ResourceValue)
                .IsRequired();

            // Table & Column Mappings
            ToTable("DbResources", "dbo");
            Property(x => x.ResourceType).HasColumnName("ResourceType");
            Property(x => x.CultureCode).HasColumnName("CultureCode");
            Property(x => x.ResourceKey).HasColumnName("ResourceKey");
            Property(x => x.ResourceValue).HasColumnName("ResourceValue");
            Property(x => x.LastUpdatedDate).HasColumnName("LastUpdatedDate");
        }
    }
}
