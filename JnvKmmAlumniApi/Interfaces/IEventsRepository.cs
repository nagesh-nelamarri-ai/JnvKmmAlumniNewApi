using JnvKmmAlumniApi.Entities;

namespace JnvKmmAlumniApi.Interfaces
{
    public interface IEventsRepository
    {
        Task<int> CreateEvent(EventData model);
        Task<IEnumerable<EventData>> GetAllEvents();
    }
}
