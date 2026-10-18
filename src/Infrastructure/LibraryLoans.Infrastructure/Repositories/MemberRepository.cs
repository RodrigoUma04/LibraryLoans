using LibraryLoans.Application.Contracts.Persistence;
using LibraryLoans.Domain;

namespace LibraryLoans.Infrastructure.Repositories;

public class MemberRepository(LibraryLoansDbContext dbContext) : GenericRepository<Member>(dbContext), IMemberRepository;