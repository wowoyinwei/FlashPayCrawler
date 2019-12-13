using FlashPayCrawler.Apis;
using Microsoft.AspNetCore.Mvc;


namespace FlashPayCrawler.Controllers
{
    [Route("[controller]")]
    public class ManageController : BaseController
    {
        protected override BaseApi api { get { return new ManageApi("Manage"); } }
    }
}
