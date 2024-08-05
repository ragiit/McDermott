using McHealthCare.Domain.Common.Interfaces;

namespace McHealthCare.Application.Extentions
{
    public class DataService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public DataService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyCreateAsync<T>(T entity) where T : class, INotifiable
        {
            await _hubContext.Clients.All.SendAsync($"Receive{entity.Type}Create", entity.Data);
        }

        public async Task NotifyUpdateAsync<T>(T entity) where T : class, INotifiable
        {
            await _hubContext.Clients.All.SendAsync($"Receive{entity.Type}Update", entity.Data);
        }

        public async Task NotifyDeleteAsync<T>(T entity) where T : class, INotifiable
        {
            await _hubContext.Clients.All.SendAsync($"Receive{entity.Type}Delete", entity.Data);
        }

        public async Task NotifyCreateListAsync<T>(List<T> entities) where T : class, INotifiable
        {
            foreach (var entity in entities)
            {
                await NotifyCreateAsync(entity);
            }
        }

        public async Task NotifyUpdateListAsync<T>(List<T> entities) where T : class, INotifiable
        {
            foreach (var entity in entities)
            {
                await NotifyUpdateAsync(entity);
            }
        }

        public async Task NotifyDeleteListAsync<T>(List<T> entities) where T : class, INotifiable
        {
            foreach (var entity in entities)
            {
                await NotifyDeleteAsync(entity);
            }
        }
    }
}