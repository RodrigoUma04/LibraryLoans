namespace LibraryLoans.Application.Exceptions;

public class ConflictException(string message, Exception? innerException = null) : Exception(message, innerException);