using LibraryApi.Exceptions;
using LibraryApi.Models;
using LibraryApi.Repositories;

namespace LibraryApi.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _books;
    private readonly ILoanRepository _loans;
    private readonly ILogger <BookService> _logger;

    public BookService(IBookRepository books, ILoanRepository loans,
    ILogger<BookService> logger)
        {
            _books = books; _loans = loans;_logger=logger;
    }
    public async Task<Loan> BorrowBookAsync(int bookId, string borrowerName)
    {
        var book = await _books.GetByIdAsync(bookId);
        if (book == null)
            throw new NotFoundException("Книга не найдена");
            
        if (!book.IsAvailable)
            throw new BusinessRuleException("Книга уже выдана");

        book.IsAvailable = false;
        await _books.UpdateAsync(book);

        var loan = await _loans.AddAsync(new Loan
        {
            BookId = bookId,
            BorrowerName = borrowerName,
            LoanDate = DateTime.UtcNow,
        });
        _logger.LogInformation(
            "Книга {BookId} выдана пользователю {BorrowerName}",
             bookId,borrowerName );
        return loan ;
    }
}