namespace Wall.Service.DependencyResolver.Interface
{
    public interface IDependencyResolver
    {
        void SetUp(IDependencyRegister dependencyRegister);
    }
}
