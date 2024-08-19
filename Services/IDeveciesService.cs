using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.Services
{
    public interface IDeveciesService
    {
        IEnumerable<SelectListItem> GetDevices();

    }
}
