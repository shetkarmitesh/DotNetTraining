using AutoMapper;
using Libaray_Management_System.Data;
using Libaray_Management_System.Entities;
using Libaray_Management_System.Interfaces;
using Libaray_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Libaray_Management_System.Services
{
    public class IssueBookService:IIssueBookService
    {
        private readonly IssueBookDBContext _issueBookDBContext;
        Status bookStatus;

        private readonly BookDBContext _bookDBContext;
        private readonly IMapper _mapper;
        public IssueBookService(IssueBookDBContext issueBookDBContext, IMapper mapper)
        {
            _mapper = mapper;
            _issueBookDBContext = issueBookDBContext;
        }

        public async Task<IssueBookModel> IssueBook(IssueBookModel issueBookModel)
        {

            var existingIssuedBook = await _bookDBContext.BookEntity.Where(q => q.BookId == issueBookModel.BookId && q.Active && !q.Archived).FirstOrDefaultAsync();
            if (existingIssuedBook != null && existingIssuedBook.BookStatus == Status.Issued  )
            {
                throw new InvalidOperationException("The book is already issued.");
            }
            IssueBookEntity issueBookEntity = new IssueBookEntity();

            issueBookEntity=_mapper.Map<IssueBookEntity>(issueBookModel);
            issueBookEntity.Initialize(true, "Admin");
            issueBookEntity.IssuedDate = DateTime.Now;
            issueBookEntity.ExpectedDate = DateTime.Now.AddDays(7);
            existingIssuedBook.BookStatus = Status.Issued;

            /*_bookDBContext.BookEntity.Add(existingIssuedBook);
            _issueBookDBContext.IssueBookEntity.Add(issueBookEntity);*/

             _issueBookDBContext.SaveChanges();
             _bookDBContext.SaveChanges();
            return issueBookModel;

        }

        public async Task<IEnumerable<IssueBookModel>> GetAllIssuedBook()
        {
            var response = await _issueBookDBContext.IssueBookEntity.Where(q =>q.BookStatus==Status.Issued && q.Active && !q.Archived).ToListAsync();
            var issueBookModels = new List<IssueBookModel>();
            foreach (var book in response)
            {
                IssueBookModel issuebookModel = new IssueBookModel();

                issuebookModel = _mapper.Map<IssueBookModel>(book);
                issueBookModels.Add(issuebookModel);
            }
            return issueBookModels;
        }

       
        public async Task<IssueBookModel> ReturnBook(IssueBookModel issueBookModel)
        {
            var existingIssuedBook = await _issueBookDBContext.IssueBookEntity.Where(q => q.BookStatus== Status.Issued  && q.MemberId == issueBookModel.MemberId && q.BookId == issueBookModel.BookId && q.Active && !q.Archived).FirstOrDefaultAsync();
            if (existingIssuedBook == null || existingIssuedBook.BookStatus != Status.Issued)
            {
                throw new InvalidOperationException("The book is not currently issued.");
            }

            existingIssuedBook.BookStatus = Status.Available;
            existingIssuedBook.ActualReturnDate = DateTime.Now;
            if (existingIssuedBook.ExpectedDate < existingIssuedBook.ActualReturnDate)
            {
                var dailyPenaltyRate = 5;
                var daysOverdue = Math.Max(0, (existingIssuedBook.ActualReturnDate - existingIssuedBook.ExpectedDate).Days);
                var penalty = daysOverdue * dailyPenaltyRate;
                existingIssuedBook.MemberDetails.Penalty=penalty;
            }
            existingIssuedBook.BookDetails.BookStatus= Status.Available;
            await _issueBookDBContext.SaveChangesAsync();
           

            return issueBookModel;
        }

        public async Task<IssueBookModel> LostBook(IssueBookModel issueBookModel)
        {
            var issuedBook = await _issueBookDBContext.IssueBookEntity.Where(q => q.BookStatus == Status.Issued && q.MemberId == issueBookModel.MemberId && q.BookId == issueBookModel.BookId && q.Active && !q.Archived).FirstOrDefaultAsync();
            if (issuedBook == null || issuedBook.BookStatus != Status.Issued)
            {
                throw new InvalidOperationException("The book is not currently issued.");
            }
            issuedBook.BookStatus = Status.Lost;
            issuedBook.ActualReturnDate = DateTime.Now;
            var bookPrice = issuedBook.BookDetails.Price;
            var penalty = 0;
            if (issuedBook.ExpectedDate < issuedBook.ActualReturnDate)
            {
                var dailyPenaltyRate = 5;
                var daysOverdue = Math.Max(0, (issuedBook.ActualReturnDate - issuedBook.ExpectedDate).Days);
                 penalty = daysOverdue * dailyPenaltyRate;
            }
            issuedBook.MemberDetails.Penalty = penalty+bookPrice;
            issuedBook.BookDetails.BookStatus = Status.Lost;
            await _issueBookDBContext.SaveChangesAsync();


            return issueBookModel;
        }
    }
}
