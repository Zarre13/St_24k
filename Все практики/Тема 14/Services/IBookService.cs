using LibraryApi.Models;

namespace LibraryApi.Services;

public interface IBookService
{
    Task<BorrowResult> BorrowBookAsync(int bookId, string borrowerName);
}

public class BorrowResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public Loan? Loan { get; set; }
}
