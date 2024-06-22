using Libaray_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Libaray_Management_System.Interfaces
{
    public interface IIssueBookService
    {
        Task<IssueBookModel> IssueBook(IssueBookModel issueBookModel);
        Task<IEnumerable<IssueBookModel>> GetAllIssuedBook();
        /*        Task<IssueBookModel> UpdateIssuedBook(IssueBookModel issueBookModel);*/
        Task<IssueBookModel> ReturnBook(IssueBookModel issueBookModel);
        Task<IssueBookModel> LostBook(IssueBookModel issueBookModel);


    }
}
