using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models.Produit;

namespace Site.Services
{
    public class ProductService : IProduct
    {
        private readonly SiteContext _context;

        public ProductService(SiteContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> GetAll()
        {
            return _context.Product
                .Include(produit => produit.Categorie);
        }

        public Product GetById(int id)
        {
            return GetAll().FirstOrDefault(produit => produit.Id == id);
        }
    }
}