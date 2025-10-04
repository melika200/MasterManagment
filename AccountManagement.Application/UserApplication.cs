using _01_FrameWork.Application;
using AccountManagment.Contracts.UnitOfWork;
using AccountManagment.Contracts.UserContracts;
using AccountManagment.Domain.RolesTypesAgg;
using AccountManagment.Domain.UserAgg;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AccountManagement.Application;

public class UserApplication : IUserApplication
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IAccountUnitOfWork _uniteOfWork;
    private readonly ILogger<UserApplication> _logger;

    public UserApplication(IUserRepository userRepository, IMapper mapper, IAccountUnitOfWork uniteOfWork, ILogger<UserApplication> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _uniteOfWork = uniteOfWork;
        _logger = logger;
    }

    public async Task<OperationResult> ChangePassword(ChangePasswordCommand command)
    {

        var operationResult = new OperationResult();


        var user = await _userRepository.GetAsync(command.Id);
        if (user == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);


        if (command.Password!.Equals(command.PasswordConfirm))
            return operationResult.Failed(ApplicationMessages.PasswordConfirmNotMatch);
        var hashedPassword = AccountUtils.HashPassword(command.Password);
        if (hashedPassword == null)
            return operationResult.Failed("رمز عبور معتبر نیست.");


        user.ChangePassword(hashedPassword);


        await _uniteOfWork.CommitAsync();

        return operationResult.Succedded();

    }

  

    public async Task<OperationResult> Create(CreateUserCommand command)
    {
        var operationResult = new OperationResult();

        if (string.IsNullOrEmpty(command.Username))
            return operationResult.Failed("نام کاربری نمی‌تواند خالی باشد.");

        if (await _userRepository.IsExistsAsync(x => x.Username == command.Username))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        var roleIdToUse = command.RoleId > 0 ? command.RoleId : RolesType.User.Id;

        var role = RolesType.AllTypes.FirstOrDefault(r => r.Id == roleIdToUse);
        if (role == null)
            return operationResult.Failed("نقش مورد نظر یافت نشد.");

        var user = new User(command.Username, role.Id);
        await _userRepository.CreateAsync(user);
        await _uniteOfWork.CommitAsync();
        return operationResult.Succedded();
    }

    public async Task<User?> GetUserWithRoleByUsernameAsync(string username)
    {
        //return await _userRepository.GetAsync(u => u.Username == username);
        return await _userRepository.GetUserWithRoleAsync(username);
    }

    public async Task<OperationResult> Edit(EditUserCommand command)
    {
         throw new NotImplementedException();
    }

    public long GetUserId(string? name)
    {
        throw new NotImplementedException();
    }

    public List<UserViewModel> GetAccountsByIds(List<long> accountIds)
    {
        throw new NotImplementedException();
    }

    public EditUserViewModel? GetForEdit(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsExistsBy(string username)
    {
        return await _userRepository.IsExistsAsync(x => x.Username == username && x.IsDeleted == false);
    }

    public List<UserViewModel>? Search(UserSearchCriteria criteria)
    {
        throw new NotImplementedException();
    }

}
