using LibraryLoans.API.Extensions;
using LibraryLoans.Application.DTOs.Book;
using LibraryLoans.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryLoans.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(BookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<GetBookDto>>> GetBooks(CancellationToken cancellationToken) =>
        Ok(await bookService.GetBooksAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetBookDto>> GetBook(int id, CancellationToken cancellationToken)
    {
        var result = await bookService.GetBookAsync(id, cancellationToken);
        if (result.IsFailure)
            return result.ToProblem();
        
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<GetBookDto>> PostBook([FromBody] CreateBookDto dto,
        CancellationToken cancellationToken)
    {
        var result = await bookService.AddBookAsync(dto, cancellationToken);

        if (result.IsFailure)
            return result.ToProblem();
        
        return CreatedAtAction(nameof(GetBook), new { id = result.Value!.Id }, result.Value);
    }
}