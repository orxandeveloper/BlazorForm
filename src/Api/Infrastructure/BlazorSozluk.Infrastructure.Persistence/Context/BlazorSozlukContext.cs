using BlazorSozluk.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Infrastructure.Persistence.Context
{
    public class BlazorSozlukContext:DbContext
    {
        public BlazorSozlukContext()
        {

        }

        public const string DEFAULT_SCHEMA = "dbo";
        public BlazorSozlukContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<EntryVote> EntryVotes { get; set; }
        public DbSet<EntryFavorite> EntryFavorites { get; set; }

        public DbSet<EntryComment> EntryComments { get; set; }
        public DbSet<EntryCommentVote> EntryCommentVote { get; set; }
        public DbSet<EntryCommentFavorite> EntryCommentFavorites { get; set; }

        public DbSet<EmailConfirmation> EmailConfirmation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               
      
                optionsBuilder.UseSqlServer("data source=localhost;initial catalog=BlazorSozluk;persist security info=True;Integrated Security=SSPI;", opt =>
                {
                    opt.EnableRetryOnFailure();
                });

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            onBeforeSave();

            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            onBeforeSave();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            onBeforeSave();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            onBeforeSave();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void onBeforeSave() { 
            
            var addedEntities=ChangeTracker.Entries()
                .Where(i=>i.State==EntityState.Added)
                .Select(i=>(BaseEntity)i.Entity);

            PrepareAddedEntities(addedEntities);
        }
        private void PrepareAddedEntities(IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreateDate = DateTime.Now;
            }
        }
    }
}
