namespace LibraryManagement.Domain.Loans;

public interface IFineCalculator
{
    decimal CalculateFine(DateTime dueDate, DateTime returnDate);
}