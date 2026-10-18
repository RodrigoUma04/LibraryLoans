using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Application.DTOs.Book;
using LibraryLoans.Application.Validators;
using LibraryLoans.Domain;
using LibraryLoans.Domain.Common;

namespace LibraryLoans.Application.Services;

public class BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyCollection<GetBookDto>> GetBooksAsync(CancellationToken cancellationToken = default) 
    {
        var books = await bookRepository.GetAllAsync(cancellationToken);

        return [.. books.Select(GetBookDto.From)];
    }

    public async Task<Result<GetBookDto>> GetBookAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = await bookRepository.GetByIdAsync(id, cancellationToken);

        return book is null ? Result<GetBookDto>.Fail($"Book with id {id} not found", ErrorType.NotFound) : Result<GetBookDto>.Ok(GetBookDto.From(book));
    }

    public async Task<Result<GetBookDto>> AddBookAsync(CreateBookDto dto, CancellationToken cancellationToken = default)
    {
        var validation = CreateBookDtoValidator.Validate(dto);
        if(validation.IsFailure)
            return Result<GetBookDto>.Fail(validation.Errors, ErrorType.Validation);
        
        if(await bookRepository.IsbnExistsAsync(dto.Isbn, cancellationToken))
            return Result<GetBookDto>.Fail("A book with the same ISBN already exists", ErrorType.Conflict);

        var book = new Book
        {
            Author = dto.Author,
            Title = dto.Title,
            Isbn = dto.Isbn
        };
        
        bookRepository.Add(book);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<GetBookDto>.Ok(GetBookDto.From(book));
    }
}