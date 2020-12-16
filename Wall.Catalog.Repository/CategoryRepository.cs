using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wall.Catalog.Entity;
using Wall.Catalog.Repository.Interface;
using Wall.Service.DataAccessLayer.DbContexts;
using Wall.Service.Repository;

namespace Wall.Catalog.Repository
{
    public class CategoryRepository : EFRepository, ICategoryRepository
    {
        IServiceScopeFactory _serviceScopeFactory;
        public CategoryRepository(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                return await dbContext.Set<Category>().ToListAsync();
            }              
        }

        public async Task<Category> GetCategoryById(string categoryId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                return await dbContext.Set<Category>().Where(x => x.CategoryId.ToString() == categoryId).FirstOrDefaultAsync();
            }     
        }
    }
}
