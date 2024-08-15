using HomeEasy.Models;

namespace HomeEasy.Interfaces;

public interface IContractService
{
    Task CreateAsync(Contract contract);

    Task<List<Contract>?> GetUserNotCompletedApprovedContractsAsync(string userId);

    Task<List<Contract>?> GetUserNotCompletedPendingContractsAsync(string userId);

    Task<List<Contract>?> GetUserCompletedContractsAsync(string userId);

    Task<Contract?> GetContractById(Guid id);

    Task ApproveContractAsync(Guid id);

    Task CompleteContractAsync(Guid id);

    Task DeleteContractAsync(Guid id);
}