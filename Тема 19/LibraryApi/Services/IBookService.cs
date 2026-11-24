using LibraryApi.Models;

namespace LibraryApi.Services;

public interface IBookService
{
    Task<Loan> BorrowBookAsync(int bookId, string borrowerName);
}

