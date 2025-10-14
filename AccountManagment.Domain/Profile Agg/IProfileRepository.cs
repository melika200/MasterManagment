using _01_FrameWork.Domain;


namespace AccountManagment.Application.Contracts.Profile
{
    public interface IProfileRepository : IRepository<long, AccountManagment.Domain.ProfileAgg.Profile>
    {

    }
}
