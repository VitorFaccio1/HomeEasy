using HomeEasy.Models;

namespace HomeEasy.Interfaces;

public interface IContractService
{
    Task CreateAsync(Contract contract);

    Task<(List<Contract> Contracts, int TotalCount)> GetUserContractsFilteredAsync(string userId, int page = 1, int size = 6, bool completed = false, bool approved = false);

    Task<Contract?> GetContractById(Guid id);

    Task ApproveContractAsync(Guid id);

    Task CompleteContractAsync(Guid id);

    Task DeleteContractAsync(Guid id);
}