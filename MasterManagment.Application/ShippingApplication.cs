using MasterManagement.Domain.ShippingAgg;
using MasterManagement.Domain.ShippingStatusesTypeAgg;
using MasterManagment.Application.Contracts.Shipping;
using MasterManagment.Application.Contracts.UnitOfWork;

namespace MasterManagment.Application;

public class ShippingApplication : IShippingApplication
{
    private readonly IShippingRepository _shippingRepository;
    private readonly IMasterUnitOfWork _unitOfWork;

    public ShippingApplication(IShippingRepository shippingRepository, IMasterUnitOfWork unitOfWork = null)
    {
        _shippingRepository = shippingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> CreateAsync(CreateShippingCommand command)
    {
        var shipping = new Shipping(
            command.CartId,
            command.FullName,
            command.PhoneNumber,
            command.Address,
        command.Description
        );

        await _shippingRepository.CreateAsync(shipping);
        await _unitOfWork.CommitAsync();
        return shipping.Id;
    }


    public Task<List<ShippingStatusViewModel>> GetAll()
    {
        var result = ShippingStatusesType.AllStatuses
            .Select(s => new ShippingStatusViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

        return Task.FromResult(result);
    }


    public async Task<ShippingViewModel?> GetByCartIdAsync(long cartId)
    {
        var shipping = await _shippingRepository.GetByCartIdAsync(cartId);
        if (shipping == null) return null;

        return new ShippingViewModel
        {
            Id = shipping.Id,
            FullName = shipping.FullName,
            PhoneNumber = shipping.PhoneNumber,
            Address = shipping.Address,
            Description = shipping.Description,
            ShippingStatusId = shipping.ShippingStatusId,
            ShippingStatusName = shipping.ShippingStatus?.Name ?? "نامشخص"

        };
    }
}
