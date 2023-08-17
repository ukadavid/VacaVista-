using System;
using Microsoft.AspNetCore.Mvc;
using VecaVista.Data;
using System.Linq;
using VecaVista.Model.Dto;

namespace VecaVista.Controllers
{
	[Route("api/createveca")]
	[ApiController]
	public class CreateVecaAPIController : ControllerBase
	{
		[HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VecaDto>> GetVecas()
		{
			return Ok(DataStore.vecaList);

        }

        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VecaDto> GetVeca(int id)
        {
            try
            {
                var veca = DataStore.vecaList.FirstOrDefault(u => u.Id == id);

                if (veca == null)
                {
                    return NotFound(); // Return 404 Not Found status
                }

                return veca; // Return 200 OK status
            }
            catch (Exception)
            {
                // Handle other exceptions here
                return StatusCode(500, "An error occurred."); // Return 500 Internal Server Error status
            }

            
        }
    }
}

