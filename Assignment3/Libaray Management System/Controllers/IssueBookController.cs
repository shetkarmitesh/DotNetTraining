using Libaray_Management_System.Interfaces;
using Libaray_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Libaray_Management_System.Controllers
{
    [Route("api/[Controller]/[Action]")]
    [ApiController]
    public class IssueBookController : ControllerBase
    {
        private readonly IIssueBookService _issueBookService;
        public IssueBookController(IIssueBookService issueBookService)
        {
            _issueBookService = issueBookService;
        }

        [HttpPost]
        public async Task<IssueBookModel> IssueBook(IssueBookModel issueBookModel)
        {
            var response = await _issueBookService.IssueBook(issueBookModel);
            return response;
        }
        [HttpGet]
        public async Task<IEnumerable<IssueBookModel>> GetAllIssuedBook()
        {
            var response = await _issueBookService.GetAllIssuedBook();
            return response;
        }

        [HttpPost]
        public async Task<IssueBookModel> ReturnBook(IssueBookModel issueBookModel)
        {
            var response = await _issueBookService.ReturnBook(issueBookModel);
            return response;
        }
        [HttpPost]
        public async Task<IssueBookModel> LostBook(IssueBookModel issueBookModel)
        {
            var response = await _issueBookService.LostBook(issueBookModel);
            return response;
        }


    }
}
