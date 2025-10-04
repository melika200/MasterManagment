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



    public async Task<OperationResult> DeleteAsync(long id)
    {
        var operationResult = new OperationResult();
        var user = await _userRepository.GetAsync(id);
        if (user == null)
            return operationResult.Failed("کاربر یافت نشد.");

        user.IsDeleted = true;
        await _userRepository.UpdateAsync(user);
        await _uniteOfWork.CommitAsync();

        return operationResult.Succedded();
    }

    public async Task<OperationResult> ActivateAsync(long id)
    {
        var operationResult = new OperationResult();
        var user = await _userRepository.GetAsync(id);
        if (user == null)
            return operationResult.Failed("کاربر یافت نشد.");

        user.Activate();
        await _userRepository.UpdateAsync(user);
        await _uniteOfWork.CommitAsync();

        return operationResult.Succedded();
    }

    public async Task<OperationResult> DeactivateAsync(long id)
    {
        var operationResult = new OperationResult();
        var user = await _userRepository.GetAsync(id);
        if (user == null)
            return operationResult.Failed("کاربر یافت نشد.");

        user.Deactivate();
        await _userRepository.UpdateAsync(user);
        await _uniteOfWork.CommitAsync();

        return operationResult.Succedded();
    }

    public async Task<OperationResult> ChangeRoleAsync(ChangeUserRoleCommand command)
    {
        var operationResult = new OperationResult();

        var user = await _userRepository.GetAsync(command.Id);
        if (user == null)
            return operationResult.Failed("کاربر یافت نشد.");

        var role = RolesType.AllTypes.FirstOrDefault(r => r.Id == command.NewRoleId);
        if (role == null)
            return operationResult.Failed("نقش انتخاب شده معتبر نیست.");

        user.ChangeRole(role);  
        await _userRepository.UpdateAsync(user);
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

   


    public async Task<bool> IsExistsBy(string username)
    {
        return await _userRepository.IsExistsAsync(x => x.Username == username && x.IsDeleted == false);
    }

    public async Task<OperationResult> Edit(EditUserCommand command)
    {
        var operationResult = new OperationResult();

        var user = await _userRepository.GetAsync(command.Id);
        if (user == null)
            return operationResult.Failed("کاربر یافت نشد.");

        if (string.IsNullOrEmpty(command.Username))
            return operationResult.Failed("نام کاربری نمی‌تواند خالی باشد.");

        // چک برای نام کاربری تکراری به جز کاربر فعلی
        bool isDuplicate = await _userRepository.IsExistsAsync(x => x.Username == command.Username && x.Id != command.Id && !x.IsDeleted);
        if (isDuplicate)
            return operationResult.Failed("نام کاربری تکراری است.");

        user.Edit(command.Fullname, command.Username);

        // اگر نیاز به تغییر نقش هست، متد ChangeRole هم صدا زده شود
        if (command.RoleId > 0 && command.RoleId != user.RoleId)
        {
            var role = RolesType.AllTypes.FirstOrDefault(r => r.Id == command.RoleId);
            if (role == null)
                return operationResult.Failed("نقش انتخاب شده معتبر نیست.");

            user.ChangeRole(role);
        }

        await _userRepository.UpdateAsync(user);
        await _uniteOfWork.CommitAsync();

        return operationResult.Succedded();
    }

    public long GetUserId(string? name)
    {
        // فرض بر این است که این متد باید آی دی کاربر بر اساس نام کاربری (Username) را برگرداند
        var user = _userRepository.GetAsync(x => x.Username == name).Result;
        if (user != null)
            return user.Id;

        return 0;
    }

    //public List<UserViewModel> GetAccountsByIds(List<long> accountIds)
    //{
    //    // گرفتن کاربران بر اساس لیست آیدی‌ها
    //    var users = _userRepository.GetManyAsync(x => accountIds.Contains(x.Id)).Result;
    //    if (users == null)
    //        return new List<UserViewModel>();

    //    return users.Select(u => _mapper.Map<UserViewModel>(u)).ToList();
    //}

    public EditUserViewModel? GetForEdit(long id)
    {
        var user = _userRepository.GetAsync(id).Result;
        if (user == null)
            return null;

        var model = _mapper.Map<EditUserViewModel>(user);
        return model;
    }

    public List<UserViewModel>? Search(UserSearchCriteria criteria)
    {
        var query = _userRepository.GetManyAsync(x =>
            (!string.IsNullOrEmpty(criteria.Username) ? x.Username.Contains(criteria.Username) : true)
            && (!criteria.IsActive.HasValue || x.IsActive == criteria.IsActive.Value)
            && !x.IsDeleted).Result;

        if (query == null)
            return new List<UserViewModel>();

        return query.Select(u => _mapper.Map<UserViewModel>(u)).ToList();
    }

    public async Task<List<UserViewModel>> GetAccountsByIds(List<long> accountIds)
    {
        var users = await _userRepository.GetManyAsync(x => accountIds.Contains(x.Id) && !x.IsDeleted);
        if (users == null)
            return new List<UserViewModel>();

        return users.Select(u => new UserViewModel
        {
            Id = u.Id,
            Username = u.Username,
            Fullname = u.Fullname,
            Address = u.Address,         
            PhoneNumber = u.PhoneNumber,   
            PostalCode = u.PostalCode      
                                          
        }).ToList();
    }

}
