using Libaray_Management_System.Data;
using Libaray_Management_System.Interfaces;
using Libaray_Management_System.Models;
using Libaray_Management_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Libaray_Management_System.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookDBContext  _dbContext;
        private readonly IBookService _bookService;
        public BookController(BookDBContext dbContext, IBookService bookService)
        {
            _dbContext = dbContext;
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IEnumerable<BookModel>> GetAllBooks()
        {
            return await _bookService.GetAllBooks();
        }

        [HttpGet]
        public async Task<BookModel> GetBookByUId(int uId)
        {
            return await _bookService.GetBookByBookId(uId);
        }

        [HttpPost]
        public async Task<ActionResult<BookModel>> AddBook(BookModel bookModel)
        {
            var response = await _bookService.AddBook(bookModel);
            return response;
        }
        [HttpPut]
        public async Task<ActionResult<BookModel>> UpdateBook(BookModel bookModel)
        {
            var response = await _bookService.UpdateBook(bookModel);
            return response;
        }

        [HttpDelete]
        public async Task<BookModel> DeleteBook(int uId)
        {
            var response = await _bookService.DeleteBook(uId);
            return response;
        }
    }
}
