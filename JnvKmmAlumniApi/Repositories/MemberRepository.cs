using Dapper;
using JnvKmmAlumniApi.Data;
using JnvKmmAlumniApi.Entities;
using System.Data;

namespace JnvKmmAlumniApi.Repositories
{
    public class MemberRepository
    {
        private readonly DapperContext _context;
        private readonly ILogger<MemberRepository> _logger;

        public MemberRepository(DapperContext context, ILogger<MemberRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Members>> GetMembersAsync(string? id = null)
        {
            try
            {
                using var connection = _context.CreateConnection();
                var members = await connection.QueryAsync<Members>("sp_GetMembers", commandType: CommandType.StoredProcedure);
                return members;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving members.");
                throw;
            }
        }

        public async Task<int> InsertMemberAsync(Members member)
        {
            try
            {
                var parameters = new
                {
                    member.Id,
                    member.Name,
                    member.Surname,
                    member.Mobile,
                    member.Email,
                    member.YearFrom,
                    member.YearTo,
                    member.Batch,
                    member.Profession,
                    member.FilePath,
                    member.Comments,
                    member.JoinedDate,
                    member.ModifiedDate,
                    member.LastTs,
                    member.IsActive,
                    member.Location,
                    member.FileName,
                    member.RoleId
                };

                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync("sp_InsertMember", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while inserting member with Id {MemberId}", member.Id);
                throw;
            }
        }

        public async Task<int> UpdateMemberAsync(Members member)
        {
            try
            {
                var parameters = new
                {
                    member.Id,
                    member.Name,
                    member.Surname,
                    member.Mobile,
                    member.Email,
                    member.YearFrom,
                    member.YearTo,
                    member.Batch,
                    member.Profession,
                    ProfilePhotoPath = member.ProfilePhoto?.FileName,
                    member.Comments,
                    member.JoinedDate,
                    member.ModifiedDate,
                    member.LastTs,
                    member.IsActive,
                    member.Location
                };

                using (var connection = _context.CreateConnection())
                {
                    return await connection.ExecuteAsync("sp_UpdateMember", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating member with Id {MemberId}", member.Id);
                throw;
            }
        }

        // get last identity
        public async Task<int> GetLastMemberId()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.QuerySingleAsync<int>("sp_GetLastMemberIdentity", commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving the last member identity.");
                throw;
            }
        }

        public async Task InsertMemberIdentity(string id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", id, DbType.String);

                    await connection.ExecuteAsync("sp_InsertMemberIdentity", parameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while inserting member identity for Id {MemberId}", id);
                throw;
            }
        }
    }
}
