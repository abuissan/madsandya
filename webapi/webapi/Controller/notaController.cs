using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using notaModel.Model;

namespace notaController.Controller
{
    public class notaController : ApiController
    {
        private nota notaku = new nota();
        
        public IHttpActionResult Get([FromBody]nota notaku)
        {
            var nota = notaku.GetRow(notaku);

            if (nota.Count > 0)
            {
                return Ok(nota);

            }
            else
            {
                return NotFound();
            }
        }

        // New record
        public IHttpActionResult Put([FromBody]nota notaku)
        {
            var success = notaku.InsertRow(notaku);

            if (!success)
            {
                return BadRequest("Insert new record Fail!");
            }
            else
            {
                return Ok("Insert new record Success!");
            }           
        }

        //Update record
        public IHttpActionResult Post([FromBody]nota notaku)
        {
            var success = notaku.UpdateRow(notaku);

            if (!success)
            {
                return BadRequest("Update record Fail!");
            }
            else
            {
                return Ok("Update record Success!");
            }           

        }

        //Delete record
        public IHttpActionResult Delete(string id)
        {
            var success = notaku.DeleteRow(id);

            if (!success)
            {
                return BadRequest("Delete record Fail!");
            }
            else
            {
                return Ok("Record deleted!");
            }         
        }


    
    }//class
}//namespace
