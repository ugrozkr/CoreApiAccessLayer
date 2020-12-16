using System.ComponentModel.Composition;
using Wall.Catalog.Repository.Interface;
using Wall.Service.DependencyResolver.Interface;

namespace Wall.Catalog.Repository
{
    [Export(typeof(IDependencyResolver))]
    public class CatalogResolver : IDependencyResolver
    {
        public void SetUp(IDependencyRegister dependencyRegister)
        {
            dependencyRegister.AddScoped<ICategoryRepository, CategoryRepository>();
            dependencyRegister.AddScoped<IEventRepository, EventRepository>();
        }
    }
}
