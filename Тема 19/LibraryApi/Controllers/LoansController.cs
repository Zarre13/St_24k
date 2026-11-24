using LibraryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

public class BorrowRequest
{
    public int BookId { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly IBookService _bookService;

    public LoansController(IBookService bookService)
    { _bookService = bookService; }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Borrow([FromBody] BorrowRequest request)
    {
        var borrowerName = User.Identity!.Name!;
        var loan = await _bookService.BorrowBookAsync(request.BookId, borrowerName);
        return Ok(loan);
    }
}