using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Site.Data;
using Site.Models.Categorie;
using Site.Models.Shipping;
using System.Collections.Generic;
using System.Linq;

namespace Site.Models
{
    public class ModelBuilderExtensions
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                SiteContext context = serviceScope.ServiceProvider.GetService<SiteContext>();

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(Categories.Select(c => c.Value));
                    foreach (Category categorieseed in categories.Values)
                    {
                        context.Categories.Add(categorieseed);
                    }
                }
                if(!context.Livraison.Any())
                {
                    context.Livraison.AddRange(Livraisons.Select(c => c.Value));
                    foreach (Livraison livraisonseed in livraisons.Values)
                    {
                        context.Livraison.Add(livraisonseed);
                    }
                }
                if(!context.EveryLivraisons.Any())
                {
                    context.EveryLivraisons.AddRange(EveryLivraisonsLaFonction.Select(c => c.Value));
                    foreach (EveryLivraison everylivraisonseed in everylivraisons.Values)
                    {
                        context.EveryLivraisons.Add(everylivraisonseed);
                    }
                }
                context.SaveChanges();
            }
        }
        private static Dictionary<string, Category> categories;
        public static Dictionary<string, Category> Categories
        {
            get
            {
                if (categories == null)
                {
                    var genreList = new Category[]
                    {
                        new Category
                        {
                            Name = "Terre et Montagne",
                            Description = "Pour les produits de Mer et de Lac"
                        },
                        new Category
                        {
                            Name = "Nautique",
                            Description = "Pour les produits de Montagne"
                        }
                    };
                    categories = new Dictionary<string, Category>();

                    foreach (Category genre in genreList)
                    {
                        categories.Add(genre.Name, genre);
                    }
                }
                return categories;
            }
        }
        private static Dictionary<string, Livraison> livraisons;
        public static Dictionary<string, Livraison> Livraisons
        {
            get
            {
                if (livraisons == null)
                {
                    var genreList = new Livraison[]
                    {
                        new Livraison
                        {
                            Name = "Sur place",
                            Description = "Votre commande est à venir chercher chez nous. Aucun frais supplémentaire.",
                            Prix = 0.00M
                        },
                        new Livraison
                        {
                            Name = "Livraison normale",
                            Description = "Livraison à l'adresse indiqué dans un délai d'une semaine. 87€ de frais supplémentaires.",
                            Prix = 87.00M
                        },
                        new Livraison
                        {
                            Name = "Livraison expresse",
                            Description = "Livraison à l'adresse indiqué dans un délai de 3 jours. 135€ de frais supplémentaires.",
                            Prix = 135.00M
                        }
                    };
                    livraisons = new Dictionary<string, Livraison>();

                    foreach (Livraison genre in genreList)
                    {
                        livraisons.Add(genre.Name, genre);
                    }
                }
                return livraisons;
            }
        }

        private static Dictionary<string, EveryLivraison> everylivraisons;
        public static Dictionary<string, EveryLivraison> EveryLivraisonsLaFonction
        {
            get
            {
                if (everylivraisons == null)
                {
                    var genreList = new EveryLivraison[]
                    {
                        new EveryLivraison
                        {
                            Id = livraisons.ElementAt(0).Value.Id,
                            Name = livraisons.ElementAt(0).Value.Name,
                            Description = livraisons.ElementAt(0).Value.Description,
                            Prix = livraisons.ElementAt(0).Value.Prix
                        },
                        new EveryLivraison
                        {
                            Id = livraisons.ElementAt(1).Value.Id,
                            Name = livraisons.ElementAt(1).Value.Name,
                            Description = livraisons.ElementAt(1).Value.Description,
                            Prix = livraisons.ElementAt(1).Value.Prix
                        },
                        new EveryLivraison
                        {
                            Id = livraisons.ElementAt(2).Value.Id,
                            Name = livraisons.ElementAt(2).Value.Name,
                            Description = livraisons.ElementAt(2).Value.Description,
                            Prix = livraisons.ElementAt(2).Value.Prix
                        }
                    };
                    everylivraisons = new Dictionary<string, EveryLivraison>();

                    foreach (EveryLivraison genre in genreList)
                    {
                        everylivraisons.Add(genre.Name, genre);
                    }
                }
                return everylivraisons;
            }
        }
    }
}