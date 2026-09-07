namespace LibraryManagement.Domain.Loans;

public class StandardFineCalculator : IFineCalculator
{
    private const decimal DailyFineAmount = 5000m;

    public decimal CalculateFine(DateTime dueDate, DateTime returnDate)
    {
        if (returnDate <= dueDate)
            return 0m;

        var lateDays = (returnDate.Date - dueDate.Date).Days;
        return lateDays * DailyFineAmount;
    }
}