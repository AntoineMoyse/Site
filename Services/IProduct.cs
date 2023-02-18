using System.Collections.Generic;
using Site.Models.Produit;

namespace Site.Services
{
    public interface IProduct
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
    }
}