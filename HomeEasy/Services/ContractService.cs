using HomeEasy.Data;
using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEasy.Services;

public class ContractService : IContractService
{
    private readonly HomeEasyContext _context;

    public ContractService(HomeEasyContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Contract contract)
    {
        _context.Contracts.Add(contract);

        await _context.SaveChangesAsync();
    }

    public async Task<(List<Contract> Contracts, int TotalCount)> GetUserContractsFilteredAsync(string userId, int page = 1, int size = 6, bool completed = false, bool approved = false)
    {
        var query = _context.Contracts
            .Include(contract => contract.Contractee)
            .Include(contract => contract.Contractor)
            .Include(contract => contract.Ad)
            .Where(contract => contract.Contractor.Id.ToString() == userId || contract.Contractee.Id.ToString() == userId);

        if (completed)
        {
            query = query.Where(contract => contract.Completed)
                .OrderBy(contract => contract.Contractee.Id.ToString() == userId ? contract.ContracteeReviewed : contract.ContractorReviewed);
        }
        else if (approved)
        {
            query = query.Where(contract => contract.Approved && !contract.Completed)
                .OrderBy(contract => contract.Date);
        }
        else
        {
            query = query.Where(contract => !contract.Approved && !contract.Completed)
                .OrderBy(contract => contract.Date);
        }

        var totalCount = await query.CountAsync();

        var contracts = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (contracts, totalCount);
    }

    public async Task<Contract?> GetContractById(Guid id) =>
        await _context.Contracts
            .Include(contract => contract.Contractee)
            .Include(contract => contract.Contractor)
            .Include(contract => contract.Ad)
            .FirstOrDefaultAsync(contract => contract.Id == id);

    public async Task ApproveContractAsync(Guid id)
    {
        var contract = _context.Contracts.FirstOrDefault(contract => contract.Id == id);

        contract.Approved = true;

        await _context.SaveChangesAsync();
    }

    public async Task CompleteContractAsync(Guid id)
    {
        var contract = _context.Contracts.FirstOrDefault(contract => contract.Id == id);

        contract.Completed = true;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteContractAsync(Guid id)
    {
        var contract = _context.Contracts.FirstOrDefault(contract => contract.Id == id);

        _context.Contracts.Remove(contract);

        await _context.SaveChangesAsync();
    }
}