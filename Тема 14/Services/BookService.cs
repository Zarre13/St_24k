using LibraryApi.Models;
using LibraryApi.Repositories;

namespace LibraryApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _books;
    private readonly ILoanRepository _loans;

    public BookService(IBookRepository books, ILoanRepository loans)
    {
        _books = books;
        _loans = loans;
    }

    public async Task<BorrowResult> BorrowBookAsync(int bookId, string borrowerName)
    {
        var book = await _books.GetByIdAsync(bookId);
        if (book is null)
        {
            return new BorrowResult { ErrorMessage = "Книга не найдена" };
        }

        if (!book.IsAvailable)
        {
            return new BorrowResult { ErrorMessage = "Книга уже выдана" };
        }

        book.IsAvailable = false;
        await _books.UpdateAsync(book);

        var loan = await _loans.AddAsync(new Loan
        {
            BookId = bookId,
            BorrowerName = borrowerName,
            LoanDate = DateTime.UtcNow
        });

        return new BorrowResult { Success = true, Loan = loan };
    }
}
