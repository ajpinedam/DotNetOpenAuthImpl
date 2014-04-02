using System.Web.Http;
using AuthServerImpl.Attribute;

namespace AuthServerImpl.Controllers
{
    public class DemoController : ApiController
    {

        [OAuth22("http://localhost/demo")]
        [Route("Demo")]
        [HttpGet]
        public string Demo()
        {
            return "Hello World From My Closed World";
        }

    }
}
