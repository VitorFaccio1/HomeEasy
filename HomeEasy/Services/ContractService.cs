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

    public async Task<List<Contract>?> GetUserNotCompletedApprovedContractsAsync(string userId)
    {
        var contracts = await _context.Contracts
            .Include(contract => contract.Contractee)
            .Include(contract => contract.Contractor)
            .Include(contract => contract.Ad)
            .Where(contract =>
                !contract.Completed && contract.Approved &&
                (contract.Contractor.Id.ToString() == userId ||
                contract.Contractee.Id.ToString() == userId))
            .ToListAsync();

        return contracts;
    }

    public async Task<List<Contract>?> GetUserNotCompletedPendingContractsAsync(string userId)
    {
        var contracts = await _context.Contracts
            .Include(contract => contract.Contractee)
            .Include(contract => contract.Contractor)
            .Include(contract => contract.Ad)
            .Where(contract =>
                !contract.Completed && !contract.Approved &&
                (contract.Contractor.Id.ToString() == userId ||
                contract.Contractee.Id.ToString() == userId))
            .ToListAsync();

        return contracts;
    }

    public async Task<List<Contract>?> GetUserCompletedContractsAsync(string userId)
    {
        var contracts = await _context.Contracts
            .Include(contract => contract.Contractee)
            .Include(contract => contract.Contractor)
            .Include(contract => contract.Ad)
            .Where(contract =>
                contract.Completed &&
                (contract.Contractor.Id.ToString() == userId ||
                contract.Contractee.Id.ToString() == userId))
            .ToListAsync();

        return contracts;
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