using Dapper;
using JnvKmmAlumniApi.Data;
using JnvKmmAlumniApi.Entities;
using JnvKmmAlumniApi.Interfaces;
using System.Data;

namespace JnvKmmAlumniApi.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly DapperContext _context;
        private readonly ILogger<EventsRepository> _logger;


        public EventsRepository(DapperContext context, ILogger<EventsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> CreateEvent(EventData model)
        {
            // Executes the insert stored procedure and returns new ID
            try
            {
                var parameters = new
                {
                    model.Title,
                    model.Description,
                    model.EventDateTime,
                    model.FilePath,
                    model.Location,
                    model.FileName
                };
                using (var connection = _context.CreateConnection())
                {

                    var result = await connection.ExecuteAsync("sp_InsertEvent", parameters, commandType: CommandType.StoredProcedure);
                    return result;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving events.");
                throw;
            }

        }

        public async Task<IEnumerable<EventData>> GetAllEvents()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var members = await connection.QueryAsync<EventData>("sp_GetAllEvents", commandType: CommandType.StoredProcedure);
                    return members;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving members.");
                throw;
            }
        }

    }
}
