using AutoMapper;
using Libaray_Management_System.Data;
using Libaray_Management_System.Entities;
using Libaray_Management_System.Interfaces;
using Libaray_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libaray_Management_System.Services
{
    public class BookService:IBookService
    {
        private readonly BookDBContext _bookDBContext;
        private readonly IMapper _mapper;

        public BookService(BookDBContext bookDBContext,IMapper mapper)

        {
            _bookDBContext = bookDBContext;
        }

        public async Task<ActionResult<BookModel>> AddBook(BookModel bookModel)
        {
            var existingBook= await GetBookByISBN(bookModel.ISBN);
            if (existingBook != null)
            {
                throw new InvalidOperationException("A book already exists.");
            }

            BookEntity bookEntity = new BookEntity();
            bookEntity = _mapper.Map<BookEntity>(bookModel);
            bookEntity.Initialize(true, "Admin");
            //use mapper
            _bookDBContext.BookEntity.Add(bookEntity);
            var response = _bookDBContext.SaveChanges();

            return _mapper.Map<BookModel>(response);

         
        }

        public async Task<IEnumerable<BookModel>> GetAllBooks()
        {
            var response = await _bookDBContext.BookEntity.Where(q => q.Active && !q.Archived).ToListAsync();
            var bookModels = new List<BookModel>();
            foreach (var book in response)
            {
                BookModel bookModel = new BookModel();
              
                bookModel = _mapper.Map<BookModel>(book);
                bookModels.Add(bookModel);
            }
            return bookModels;
        }

        public async Task<BookModel> GetBookByBookId(int bookId)
        {
            var book = await _bookDBContext.BookEntity.Where(q => q.BookId == bookId && q.Active && !q.Archived).FirstOrDefaultAsync();
            if (book == null)
            {
                return null;
            }
            BookModel bookModel = new BookModel();
            bookModel=_mapper.Map<BookModel>(book);
          
            return bookModel;
        } public async Task<BookModel> GetBookByISBN(string ISBN)
        {
            var book = await _bookDBContext.BookEntity.Where(q => q.ISBN == ISBN && q.Active && !q.Archived).FirstOrDefaultAsync();
            if (book == null)
            {
                return null;
            }
            BookModel bookModel = new BookModel();
            bookModel=_mapper.Map<BookModel>(book);
          
            return bookModel;
        }
        
        public async Task<ActionResult<BookModel>> UpdateBook(BookModel bookModel)
        {
            var existingBook = await _bookDBContext.BookEntity.Where(q => q.ISBN == bookModel.ISBN && q.Active && !q.Archived).FirstOrDefaultAsync();
            if (existingBook != null)
            {
                throw new InvalidOperationException("A book not found.");
            }
            existingBook.Active = false;
            existingBook.Archived = true;
            await _bookDBContext.SaveChangesAsync();

            existingBook.Initialize(false, "Admin");
            //check 
            existingBook= _mapper.Map<BookEntity>(bookModel);

            _bookDBContext.BookEntity.Add(existingBook);
            _bookDBContext.SaveChanges();
            return _mapper.Map<BookModel>(existingBook);
        }
        public async Task<BookModel> DeleteBook(int bookId)
        {
            var bookToDelete = await _bookDBContext.BookEntity.Where(q => q.BookId == bookId && q.Active && !q.Archived).FirstOrDefaultAsync();

            if (bookToDelete != null)
            {
                throw new InvalidOperationException("A member not found.");
            }
            bookToDelete.Active = false;
            bookToDelete.Archived = true;
            await _bookDBContext.SaveChangesAsync();
            bookToDelete.Initialize(false, "Admin");
            bookToDelete.Active = false;
            bookToDelete.Archived = true;
            _bookDBContext.BookEntity.Add(bookToDelete);
            _bookDBContext.SaveChanges();

            return _mapper.Map<BookModel>(bookToDelete);
        }
    }
}
