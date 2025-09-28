using _01_FrameWork.Application;
using _01_FrameWork.Infrastructure;
using AccountManagment.Contracts;
using AccountManagment.Domain.RolesTypesAgg;
using AccountManagment.Domain.UserAgg;
using AutoMapper;

namespace AccountManagement.Application;

public class UserApplication : IUserApplication
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uniteOfWork;
    public UserApplication(IUserRepository userRepository, IMapper mapper, IUnitOfWork uniteOfWork)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _uniteOfWork = uniteOfWork;
    }

    public async Task<OperationResult> ChangePassword(ChangePasswordCommand command)
    {

        var operationResult = new OperationResult();


        var user = await _userRepository.GetAsync(command.Id);
        if (user == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);


        if (!command.Password.Equals(command.PasswordConfirm))
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

        var role = RolesType.AllTypes.FirstOrDefault(r => r.Id == command.RoleId);
        if (role == null)
            return operationResult.Failed("نقش مورد نظر یافت نشد.");

        var user = new User(
            command.Fullname?.Normalize_FullPersianTextAndNumbers(),
            command.Username,
            AccountUtils.HashPassword(command.Password),
            command.Mobile?.Normalize_PersianNumbers(),
            role.Id);
        await _userRepository.AddAsync(user);
        await _uniteOfWork.CommitAsync();
        return operationResult.Succedded();
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
