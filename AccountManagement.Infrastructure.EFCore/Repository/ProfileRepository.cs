using AccountManagment.Domain.ProfileAgg;
using Microsoft.EntityFrameworkCore;
using _01_FrameWork.Infrastructure;
using AccountManagement.Infrastructure.EFCore.Context;
using AccountManagment.Application.Contracts.Profile;

namespace AccountManagment.Infrastructure.EFCore.Repository
{
    public class ProfileRepository : RepositoryBase<long, Profile>, IProfileRepository
    {
        private readonly AccountContext _context;

        public ProfileRepository(AccountContext context) : base(context)
        {
            _context = context;
        }

        
    }
}
