using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wall.Catalog.Entity;
using Wall.Service.Repository;

namespace Wall.Catalog.Repository.Interface
{
    public interface ICategoryRepository : IGenericRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(string categoryId);
    }
}
