using System.Threading.Tasks;

namespace Wall.Integration.Messages
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
