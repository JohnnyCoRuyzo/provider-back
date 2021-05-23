using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static provider_back.Connection.Connection;
using provider_back.Models;

namespace provider_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        // GET api/provider
        [HttpGet]
        public ActionResult<IEnumerable<ProviderViewModel>> Get()
        {
            return SelectQuery("SELECT * FROM [dbo].[PI_Provider_Select]") ;
        }

        // GET api/provider/5
        [HttpGet("{id}")]
        public ActionResult<ProviderViewModel> Get(int id)
        {
            return SelectQuery("SELECT * FROM [dbo].[PI_Provider_Select]");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
