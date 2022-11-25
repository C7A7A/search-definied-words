using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchDefinedWords.Models;
using SearchDefinedWords.Services;

namespace SearchDefinedWords.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SearchTextesController : ControllerBase {

        private readonly ISearchTextService searchTextService;

        public SearchTextesController(ISearchTextService searchTextService) {
            this.searchTextService = searchTextService;
        }

        [HttpPost]
        public List<string> SearchText([FromBody] SearchText searchText) {
            return searchTextService.searchText();
        }
    }
}
