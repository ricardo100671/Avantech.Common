

namespace Avantech.Common.Globalization.DbResourceProvider.Data
{
    internal class DbResourceContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbResourceContext"/> class.
        /// </summary>
        public DbResourceContext(): base("name=DbResourceProvider"){}

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DbResourceContext>(null);
            modelBuilder.Configurations.Add(new DbResourceMap());
            modelBuilder.Configurations.Add(new DbResourcePrimerMap());
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        public DbSet<DbResource> Resources { get; set; }

        /// <summary>
        /// Gets the resource primers.
        /// </summary>
        public DbSet<DbResourcePrimer> ResourcePrimers { get; set; }
    }
}
