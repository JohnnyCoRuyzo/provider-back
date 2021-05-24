using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static provider_back.Connection.Connection;
using provider_back.Models;
using Microsoft.AspNetCore.Http;
using System.Web.Http.Cors;

namespace provider_back.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        // GET api/provider
        [HttpGet]
        public ActionResult<IEnumerable<ProviderViewModel>> Get()
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            try
            {
                return SelectQuery("SELECT * FROM [dbo].[PI_Provider_Select]");
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        // GET api/provider/5
        [HttpGet("{id}")]
        public ActionResult<ProviderViewModel> Get(int id)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            try 
            { 
                return SelectQuery("SELECT * FROM [dbo].[PI_Provider_Select] WHERE [P_ID] = " + id).FirstOrDefault();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        // POST api/provider
        [HttpPost]
        public ActionResult Post([FromBody] ProviderViewModel viewModel)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            try
            {
                ExecuteStoreProcedure(viewModel, Utilities.EnumAction.Insert);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        // PUT api/provider
        [HttpPut]
        public ActionResult Put([FromBody] ProviderViewModel viewModel)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            try
            {
                ExecuteStoreProcedure(viewModel, Utilities.EnumAction.Update);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        // DELETE api/provider/10
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            try
            {
                ExecuteStoreProcedure(new ProviderViewModel()
                {
                    ProviderID = id
                }, Utilities.EnumAction.Delete);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }
    }
}
