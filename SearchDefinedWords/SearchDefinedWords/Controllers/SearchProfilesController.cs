using Microsoft.AspNetCore.Mvc;
using SearchDefinedWords.Models;
using SearchDefinedWords.Services;
using System.Net;

namespace SearchDefinedWords.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SearchProfilesController : ControllerBase {

        private readonly ISearchProfileService searchProfilesService;

        public SearchProfilesController(ISearchProfileService searchProfilesService) {
            this.searchProfilesService = searchProfilesService;
        }

        [HttpPost]
        public ObjectResult Post([FromBody] SearchProfile searchProfile) {
            SearchProfile profile = searchProfilesService.AddProfile(searchProfile);

            return StatusCode((int)HttpStatusCode.OK, profile);
        }
    }
}
