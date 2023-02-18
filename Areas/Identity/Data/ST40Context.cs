using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Site.Models.ApplicationUser;
using Site.Models.Categorie;
using Site.Models.FAQ;
using Site.Models.Order;
using Site.Models.Panier;
using Site.Models.Produit;
using Site.Models.Shipping;

namespace Site.Data
{
    public class SiteContext : IdentityDbContext<Client>
    {
        public SiteContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<EveryProduct> EveryProduct { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CartItem> ItemPanier { get; set; }
        public DbSet<Commande> Commande { get; set; }
        public DbSet<CommandeDetail> CommandeDetails { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Livraison> Livraison { get; set; }
        public DbSet<EveryLivraison> EveryLivraisons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .HasOne(f => f.Categorie)
                .WithMany(c => c.Produits)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<CartItem>()
                .HasOne(c => c.Produit);
                
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
