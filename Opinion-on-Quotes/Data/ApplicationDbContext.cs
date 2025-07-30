using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Opinion_on_Quotes.Models;

namespace Opinion_on_Quotes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<QuoteMood>()
        .HasKey(qm => new { qm.quote_id, qm.mood_id }); // Composite PK

            modelBuilder.Entity<QuoteMood>()
                .HasOne(qm => qm.Quote)
                .WithMany(q => q.QuoteMoods)
                .HasForeignKey(qm => qm.quote_id);

            modelBuilder.Entity<QuoteMood>()
                .HasOne(qm => qm.Mood)
                .WithMany(m => m.QuoteMoods)
                .HasForeignKey(qm => qm.mood_id);
        }
        //Set Drama Object as a Table
        public DbSet<Drama> Dramas { get; set; }

        //Set Mood Object as a Table
        public DbSet<Mood> Moods { get; set; }
        //Set Quote Object as a Table
        public DbSet<Quote> Quotes { get; set; }
        //Set QuoteMood Object as a Table
        public DbSet<QuoteMood> QuoteMoods { get; set; }

        public DbSet<Comment> Comments { get; set; }

    }

}
