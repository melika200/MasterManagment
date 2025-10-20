using MasterManagement.Domain.SupportAgg;
using MasterManagement.Domain.SupportStatusTypesAgg;
using MasterManagment.Application.Contracts.Support;
using MasterManagment.Application.Contracts.UnitOfWork;
using _01_FrameWork.Application;
using _01_FrameWork.Application.Exceptions;
using System.Security.Claims;

namespace MasterManagment.Application;

public class SupportApplication : ISupportApplication
{
    private readonly ISupportRepository _repository;
    private readonly IMasterUnitOfWork _unitOfWork;

    public SupportApplication(ISupportRepository repository, IMasterUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> CreateAsync(CreateSupportCommand command, long accountId)
    {
        var result = new OperationResult();

        var support = new Support(accountId, command.FullName, command.Email, command.PhoneNumber, command.Subject, command.Message);
        //support.ChangeStatus(SupportStatusType.Open);
        support.ChangeStatus(SupportStatusType.Open);

        await _repository.CreateAsync(support);
        await _unitOfWork.CommitAsync();

        return result.Succedded();
    }

    public async Task<OperationResult> EditAsync(EditSupportViewModel model)
    {
        var result = new OperationResult();
        var support = await _repository.GetAsync(model.Id) ?? throw new EntityNotFoundException(nameof(Support), model.Id);

        support.Edit(model.FullName, model.Email, model.PhoneNumber, model.Subject, model.Message);
        await _repository.UpdateAsync(support);
        await _unitOfWork.CommitAsync();

        return result.Succedded();
    }

    public async Task<OperationResult> DeleteAsync(long id)
    {
        var result = new OperationResult();
        var support = await _repository.GetAsync(id) ?? throw new EntityNotFoundException(nameof(Support), id);

        support.SoftDelete();
        await _repository.UpdateAsync(support);
        await _unitOfWork.CommitAsync();

        return result.Succedded();
    }

    public async Task<OperationResult> ChangeStatusAsync(ChangeSupportStatusCommand command)
    {
        var result = new OperationResult();
        var support = await _repository.GetAsync(command.Id) ?? throw new EntityNotFoundException(nameof(Support), command.Id);

        var newStatus = SupportStatusType.AllTypes.FirstOrDefault(x => x.Name == command.Status);
        if (newStatus == null) return result.Failed("Invalid status.");

        support.ChangeStatus(newStatus);
        await _repository.UpdateAsync(support);
        await _unitOfWork.CommitAsync();

        return result.Succedded();
    }

    public async Task<SupportViewModel?> GetDetails(long id)
    {
        var support = await _repository.GetAsync(id);
        if (support == null) return null;

        return new SupportViewModel
        {
            Id = support.Id,
            AccountId = support.AccountId,
            FullName = support.FullName,
            Email = support.Email,
            PhoneNumber = support.PhoneNumber,
            Subject = support.Subject,
            Message = support.Message,
            Status = support.Status?.Name ?? string.Empty,
            CreationDate = support.CreationDate,
            IsReplied = support.IsReplied,     
            ReplyMessage = support.ReplyMessage
        };
    }

    public async Task<List<SupportViewModel>> GetUserTickets(long accountId)
    {
        var list = await _repository.GetByAccountIdAsync(accountId);
        return list.Select(x => new SupportViewModel
        {
            Id = x.Id,
            AccountId = x.AccountId,
            FullName = x.FullName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            Subject = x.Subject,
            Message = x.Message,
            Status = x.Status?.Name ?? string.Empty,
            CreationDate = x.CreationDate,
            IsReplied = x.IsReplied,       
            ReplyMessage = x.ReplyMessage
        }).ToList();
    }

    //public async Task<List<SupportViewModel>> SearchAsync(SupportSearchCriteria searchModel)
    //{
    //    var list = await _repository.GetAllSupport(searchModel.Keyword, searchModel.Status, searchModel.AccountId);
    //    return list.Select(x => new SupportViewModel
    //    {
    //        Id = x.Id,
    //        AccountId = x.AccountId,
    //        FullName = x.FullName,
    //        Email = x.Email,
    //        PhoneNumber = x.PhoneNumber,
    //        Subject = x.Subject,
    //        Message = x.Message,
    //        Status = x.Status?.Name ?? string.Empty,
    //        CreationDate = x.CreationDate
    //    }).ToList();
    //}

    public async Task<List<SupportViewModel>> GetAllAsync()
    {
        var list = await _repository.GetAllSupport();
        return list.Select(x => new SupportViewModel
        {
            Id = x.Id,
            AccountId = x.AccountId,
            FullName = x.FullName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            Subject = x.Subject,
            Message = x.Message,
            Status = x.Status?.Name ?? string.Empty,
            CreationDate = x.CreationDate
        }).ToList();
    }


    public async Task<OperationResult> MarkAsRepliedAsync(long id)
    {
        var result = new OperationResult();
        var support = await _repository.GetAsync(id) ?? throw new EntityNotFoundException(nameof(Support), id);

        support.MarkAsReplied();
        await _repository.UpdateAsync(support);
        await _unitOfWork.CommitAsync();

        return result.Succedded();
    }
    public async Task<OperationResult> EditUserTicketAsync(EditUserSupportCommand model, ClaimsPrincipal user)
    {
        var result = new OperationResult();
        var support = await _repository.GetAsync(model.Id) ?? throw new EntityNotFoundException(nameof(Support), model.Id);

        var accountIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (accountIdClaim == null || support.AccountId != long.Parse(accountIdClaim))
            return result.Failed("You cannot edit this ticket.");

        support.Edit(model.FullName, model.Email, model.PhoneNumber, model.Subject, model.Message);
        await _repository.UpdateAsync(support);
        await _unitOfWork.CommitAsync();

        return result.Succedded();
    }
    public async Task<OperationResult> EditAdminTicketAsync(EditAdminSupportCommand command)
    {
        var result = new OperationResult();

        var support = await _repository.GetAsync(command.Id)
            ?? throw new EntityNotFoundException(nameof(Support), command.Id);

        if (!string.IsNullOrEmpty(command.ReplyMessage))
            support.Reply(command.ReplyMessage);

        if (!string.IsNullOrEmpty(command.Status))
        {
            
            if (!int.TryParse(command.Status, out int statusId))
                return result.Failed("Invalid status format.");

         
            var status = SupportStatusType.AllTypes.FirstOrDefault(x => x.Id == statusId);
            if (status == null)
                return result.Failed($"Invalid status id: {statusId}");

            support.ChangeStatus(status);
        }

        await _repository.UpdateAsync(support);
        await _unitOfWork.CommitAsync();

        return result.Succedded();
    }



}
