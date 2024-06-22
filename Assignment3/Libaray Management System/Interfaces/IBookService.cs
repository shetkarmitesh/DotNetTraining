using Libaray_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Libaray_Management_System.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookModel>> GetAllBooks();
        Task<BookModel> GetBookByBookId(int bookId);
        Task<BookModel> GetBookByISBN(string ISBN);
        Task<ActionResult<BookModel>> AddBook(BookModel bookModel);
        Task<ActionResult<BookModel>> UpdateBook(BookModel bookModel);
        Task<BookModel> DeleteBook(int bookId);
    }
}
