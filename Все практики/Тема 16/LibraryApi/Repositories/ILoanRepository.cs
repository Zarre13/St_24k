using LibraryApi.Models;

namespace LibraryApi.Repositories;

public interface ILoanRepository
{
    Task<Loan> AddAsync(Loan loan);
}