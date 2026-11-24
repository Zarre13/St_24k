using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

public class BorrowRequest
{
    public int BookId { get; set; }
    public string BorrowerName { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly IBookService _bookService;

    public LoansController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<IActionResult> Borrow([FromBody] BorrowRequest request)
    {
        var result = await _bookService.BorrowBookAsync(
            request.BookId,
            request.BorrowerName);

        return result.Success
            ? Ok(result.Loan)
            : BadRequest(result.ErrorMessage);
    }
}
