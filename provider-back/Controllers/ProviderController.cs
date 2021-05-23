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
            return SelectQuery("SELECT * FROM [dbo].[PI_Provider_Select] WHERE [P_ID] = " + id).FirstOrDefault();
        }

        // POST api/provider
        [HttpPost]
        public void Post([FromBody] ProviderViewModel viewModel)
        {
            ExecuteStoreProcedure(viewModel, Utilities.EnumAction.Insert);
        }

        // PUT api/provider
        [HttpPut]
        public void Put([FromBody] ProviderViewModel viewModel)
        {
            ExecuteStoreProcedure(viewModel, Utilities.EnumAction.Update);
        }

        // DELETE api/provider
        [HttpDelete]
        public void Delete(ProviderViewModel viewModel)
        {
            ExecuteStoreProcedure(viewModel, Utilities.EnumAction.Delete);
        }
    }
}
