using LibraryManagement.Application;
using LibraryManagement.Domain.Common;
using LibraryManagement.Infrastructure;
using LibraryManagement.Domain.Loans;


var bookRepository = new InMemoryBookRepository();
var memberRepository = new InMemoryMemberRepository();
var loanRepository = new InMemoryLoanRepository();
var fineCalculator = new StandardFineCalculator();

var libraryService = new LibraryService(bookRepository, memberRepository, loanRepository, fineCalculator);

try
{
    var book = await libraryService.AddBookAsync(
        title: "کیمیاگر",
        author: "پائولو کوئیلو",
        copies: 2);

    var member = await libraryService.RegisterMemberAsync(
        fullName: "علی رضایی",
        email: "ali.rezaei@example.com");

    Console.WriteLine($"کتاب ثبت شد: {book.Title} ({book.AvailableCopies} نسخه در دسترس)");
    Console.WriteLine($"عضو ثبت شد: {member.FullName}");

    var loan = await libraryService.BorrowBookAsync(book.Id, member.Id, DateTime.Today);
    Console.WriteLine($"امانت ثبت شد. تاریخ سررسید: {loan.DueDate:yyyy-MM-dd}");

    var returnedLoan = await libraryService.ReturnBookAsync(loan.Id, DateTime.Today.AddDays(20));
    Console.WriteLine($"کتاب بازگردانده شد. جریمه تاخیر: {returnedLoan.Fine:N0} تومان");
}
catch (DomainException ex)
{
    Console.WriteLine($"خطا: {ex.Message}");
}