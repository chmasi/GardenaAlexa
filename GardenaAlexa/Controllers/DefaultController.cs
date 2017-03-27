using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GardenaAlexa.Controllers
{
    public class DefaultController : ApiController
    {
        [Route("echo/GardenaCMD")]
        [HttpPost]
        public HttpResponseMessage PostMowerCommand()
        {
            var rasenmaeher = new RasenmaeherSpeechlet();
            return rasenmaeher.GetResponse(Request);
        }


    }
}
