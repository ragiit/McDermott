using McDermott.Telemedic.Model;
using static TelemedicService;

namespace McDermott.Telemedic.Service
{
    public interface ITelemedicService
    {
        Task<TelemedicResult> SearchTelemedicAsync(string number, int serviceId);
    }
}
