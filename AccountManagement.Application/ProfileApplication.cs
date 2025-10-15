using System.Security.Claims;
using _01_FrameWork.Application;
using AccountManagment.Application.Contracts.Profile;
using AccountManagment.Contracts.UnitOfWork;
using AccountManagment.Domain.ProfileAgg;
using AccountManagment.Domain.UserAgg;
using Microsoft.AspNetCore.Http;


namespace AccountManagment.Application
{
    public class ProfileApplication : IProfileApplication
    {
        private readonly IProfileRepository _repository;
        private readonly IAccountUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public ProfileApplication(IProfileRepository repository, IAccountUnitOfWork unitOfWork, IUserRepository userRepository )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<OperationResult> CreateProfileAsync(CreateProfileCommand command, ClaimsPrincipal user)
        {
            var operation = new OperationResult();

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return operation.Failed("کاربر لاگین نکرده است.");

            long userId = long.Parse(userIdClaim);
 
            var userExists = await _userRepository.IsExistsAsync(x => x.Id == userId);
            if (!userExists)
                return operation.Failed("کاربر در سیستم وجود ندارد.");


            var exists = await _repository.IsExistsAsync(x => x.UserId == userId);
            if (exists)
                return operation.Failed("برای این کاربر قبلاً پروفایل ثبت شده است.");

            string? avatarPath = null;
            if (command.AvatarFile != null)
                avatarPath = await UploadAvatarAsync(command.AvatarFile);

            var profile = new Profile(
                userId,
                command.FullName,
                command.Email,
                command.Address,
                command.PostalCode,
                avatarPath
            );

            await _repository.CreateAsync(profile);
            await _unitOfWork.CommitAsync();

            return operation.Succedded("پروفایل با موفقیت ایجاد شد.");
        }

        public async Task<OperationResult> EditProfileAsync(EditProfileCommand command, ClaimsPrincipal user)
        {
            var operation = new OperationResult();

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return operation.Failed("کاربر لاگین نکرده است.");

            long userId = long.Parse(userIdClaim);

            var profile = await _repository.GetAsync(x => x.UserId == userId && !x.IsDeleted);
            if (profile == null)
                return operation.Failed("پروفایل یافت نشد.");

            string? avatarPath = profile.AvatarPath;
            if (command.AvatarFile != null)
                avatarPath = await UploadAvatarAsync(command.AvatarFile);

            profile.Edit(command.FullName, command.Email, command.Address, command.PostalCode, avatarPath);

            await _unitOfWork.CommitAsync();
            return operation.Succedded("پروفایل با موفقیت ویرایش شد.");
        }

        public async Task<ProfileViewModel?> GetProfileByUserIdAsync(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return null;

            long userId = long.Parse(userIdClaim);

            var profile = await _repository.GetAsync(x => x.UserId == userId && !x.IsDeleted);
            if (profile == null)
                return null;

            return new ProfileViewModel
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FullName = profile.FullName,
                Email = profile.Email,
                Address = profile.Address,
                PostalCode = profile.PostalCode,
                AvatarPath = profile.AvatarPath
            };
        }

        private string GetAvatarUploadPath()
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        private async Task<string?> UploadAvatarAsync(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                throw new InvalidOperationException("فرمت فایل مجاز نیست.");

            var fileName = $"{Guid.NewGuid()}{ext}";
            var uploadPath = GetAvatarUploadPath();
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Path.Combine("uploads", "avatars", fileName).Replace("\\", "/");
        }


        public async Task<ProfileViewModel?> GetProfileByUserIdInAdminAsync(long userId)
        {
            var profile = await _repository.GetAsync(x => x.UserId == userId && !x.IsDeleted);

            if (profile == null)
                return null;

            return new ProfileViewModel
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FullName = profile.FullName,
                Email = profile.Email,
                Address = profile.Address,
                PostalCode = profile.PostalCode,
                AvatarPath = profile.AvatarPath
            };
        }
        public async Task<IEnumerable<ProfileViewModel>> GetAllProfileAsync()
        {
            var profiles = await _repository.GetManyAsync(x => !x.IsDeleted);
            return profiles.Select(profile => new ProfileViewModel
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FullName = profile.FullName,
                Email = profile.Email,
                Address = profile.Address,
                PostalCode = profile.PostalCode,
                AvatarPath = profile.AvatarPath
            });
        }

        public async Task<ProfileViewModel?> GetProfileByIdAsync(long id)
        {
            var profile = await _repository.GetAsync(id);
            if (profile == null || profile.IsDeleted)
                return null;

            return new ProfileViewModel
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FullName = profile.FullName,
                Email = profile.Email,
                Address = profile.Address,
                PostalCode = profile.PostalCode,
                AvatarPath = profile.AvatarPath
            };
        }

        public async Task<OperationResult> DeleteProfileAsync(long id)
        {
            var operation = new OperationResult();
            var profile = await _repository.GetAsync(id);
            if (profile == null)
                return operation.Failed("پروفایل یافت نشد.");

            profile.SoftDelete();
            await _unitOfWork.CommitAsync();
            return operation.Succedded("پروفایل با موفقیت حذف شد.");
        }

       
    }
}


        //private string GetAvatarUploadPath()
        //{
        //    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
        //    if (!Directory.Exists(folder))
        //        Directory.CreateDirectory(folder);
        //    return folder;
        //}

      
        //private async Task<string?> UploadAvatarAsync(IFormFile? file)
        //{
        //    if (file == null || file.Length == 0) return null;

        //    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        //    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        //    if (!allowedExtensions.Contains(ext))
        //        throw new InvalidOperationException("فرمت فایل مجاز نیست.");

        //    var fileName = $"{Guid.NewGuid()}{ext}";
        //    var uploadPath = GetAvatarUploadPath();
        //    var filePath = Path.Combine(uploadPath, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return Path.Combine("uploads", "avatars", fileName).Replace("\\", "/");
        //}
//private string GetAvatarUploadPath()
//{
//    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
//    if (!Directory.Exists(folder))
//        Directory.CreateDirectory(folder);
//    return folder;
//}
        //public async Task<ProfileViewModel?> GetProfileByUserIdAsync(ClaimsPrincipal user)
        //{
        //    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (userIdClaim == null)
        //        return null;

        //    long userId = long.Parse(userIdClaim);

        //    var profile = await _repository.GetAsync(x => x.UserId == userId && !x.IsDeleted);
        //    if (profile == null)
        //        return null;

        //    return new ProfileViewModel
        //    {
        //        Id = profile.Id,
        //        UserId = profile.UserId,
        //        FullName = profile.FullName,
        //        Email = profile.Email,
        //        Address = profile.Address,
        //        PostalCode = profile.PostalCode,
        //        AvatarPath = profile.AvatarPath
        //    };
        //}
        //public async Task<OperationResult> CreateProfileAsync(CreateProfileCommand command, ClaimsPrincipal user)
        //{
        //    var operation = new OperationResult();

        //    // --- استخراج امن userId از ClaimsPrincipal ورودی ---
        //    // fallback روی چند نام claim مختلف تا اگر mapping متفاوت بود هم کار کند
        //    var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        //                      ?? user?.FindFirst("nameid")?.Value
        //                      ?? user?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value
        //                      ?? user?.FindFirst("sub")?.Value;

        //    // لاگ موقت (برای دیباگ) — بعد از تست حذف یا تبدیل به ILogger کن
        //    // Console.WriteLine($"[DEBUG] Incoming claims: {string.Join(", ", user?.Claims.Select(c => $"{c.Type}={c.Value}") ?? Enumerable.Empty<string>())}");

        //    if (string.IsNullOrWhiteSpace(userIdClaim))
        //        return operation.Failed("کاربر لاگین نکرده است یا شناسه کاربر در توکن موجود نیست.");

        //    if (!long.TryParse(userIdClaim, out var userId))
        //        return operation.Failed("شناسه کاربر نامعتبر است.");

        //    // --- اطمینان از وجود کاربر در DB ---
        //    var userExists = await _userRepository.IsExistsAsync(x => x.Id == userId && !x.IsDeleted);
        //    if (!userExists)
        //        return operation.Failed("کاربر در سیستم وجود ندارد.");

        //    // --- بررسی تکراری نبودن پروفایل ---
        //    var exists = await _repository.IsExistsAsync(x => x.UserId == userId);
        //    if (exists)
        //        return operation.Failed("برای این کاربر قبلاً پروفایل ثبت شده است.");

        //    string? avatarPath = null;
        //    if (command.AvatarFile != null)
        //        avatarPath = await UploadAvatarAsync(command.AvatarFile);

        //    var profile = new Profile(
        //        userId,
        //        command.FullName,
        //        command.Email,
        //        command.Address,
        //        command.PostalCode,
        //        avatarPath
        //    );

        //    await _repository.CreateAsync(profile);
        //    await _unitOfWork.CommitAsync();

        //    return operation.Succedded("پروفایل با موفقیت ایجاد شد.");
        //}

        //public async Task<OperationResult> EditProfileAsync(EditProfileCommand command, ClaimsPrincipal user)
        //{
        //    var operation = new OperationResult();

        //    var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        //                      ?? user?.FindFirst("nameid")?.Value
        //                      ?? user?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

        //    if (string.IsNullOrWhiteSpace(userIdClaim))
        //        return operation.Failed("کاربر لاگین نکرده است.");

        //    if (!long.TryParse(userIdClaim, out var userId))
        //        return operation.Failed("شناسه کاربر نامعتبر است.");

        //    var profile = await _repository.GetAsync(x => x.UserId == userId && !x.IsDeleted);
        //    if (profile == null)
        //        return operation.Failed("پروفایل یافت نشد.");

        //    string? avatarPath = profile.AvatarPath;
        //    if (command.AvatarFile != null)
        //        avatarPath = await UploadAvatarAsync(command.AvatarFile);

        //    profile.Edit(command.FullName, command.Email, command.Address, command.PostalCode, avatarPath);

        //    await _unitOfWork.CommitAsync();
        //    return operation.Succedded("پروفایل با موفقیت ویرایش شد.");
        //}

        //public async Task<ProfileViewModel?> GetProfileByUserIdAsync(ClaimsPrincipal user)
        //{
        //    var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        //                      ?? user?.FindFirst("nameid")?.Value
        //                      ?? user?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

        //    if (string.IsNullOrWhiteSpace(userIdClaim))
        //        return null;

        //    if (!long.TryParse(userIdClaim, out var userId))
        //        return null;

        //    var profile = await _repository.GetAsync(x => x.UserId == userId && !x.IsDeleted);
        //    if (profile == null)
        //        return null;

        //    return new ProfileViewModel
        //    {
        //        Id = profile.Id,
        //        UserId = profile.UserId,
        //        FullName = profile.FullName,
        //        Email = profile.Email,
        //        Address = profile.Address,
        //        PostalCode = profile.PostalCode,
        //        AvatarPath = profile.AvatarPath
        //    };
        //}

        //private string GetAvatarUploadPath()
        //{
        //    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
        //    if (!Directory.Exists(folder))
        //        Directory.CreateDirectory(folder);
        //    return folder;
        //}

        //private async Task<string?> UploadAvatarAsync(IFormFile file)
        //{
        //    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        //    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        //    if (!allowedExtensions.Contains(ext))
        //        throw new InvalidOperationException("فرمت فایل مجاز نیست.");

        //    var fileName = $"{Guid.NewGuid()}{ext}";
        //    var uploadPath = GetAvatarUploadPath();
        //    var filePath = Path.Combine(uploadPath, fileName);

        //    using var stream = new FileStream(filePath, FileMode.Create);
        //    await file.CopyToAsync(stream);

        //    return Path.Combine("uploads", "avatars", fileName).Replace("\\", "/");
        //}