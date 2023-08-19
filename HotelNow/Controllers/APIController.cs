using System;
using Microsoft.AspNetCore.Mvc;
using VecaVista.Data;
using System.Linq;
using VecaVista.Model.Dto;
using Microsoft.AspNetCore.JsonPatch;

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

        [HttpGet("id", Name = "GetVecas")]
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VecaDto> CreateVeca([FromBody] VecaDto vecaDto)
        {
            if (DataStore.vecaList.FirstOrDefault(u => u.Name == vecaDto.Name) != null)
            {
                ModelState.AddModelError("nameError", "Name already exist");
                return BadRequest(ModelState);
            }
            if (vecaDto == null)
            {
                return BadRequest(vecaDto);
            }

            if (vecaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            vecaDto.Id = DataStore.vecaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;

            DataStore.vecaList.Add(vecaDto);

            return CreatedAtRoute("GetVecas", new { id = vecaDto.Id }, vecaDto);
        }

        [HttpDelete("{id:int}", Name = "DeleteVeca")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVeca(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var vecaToDelete = DataStore.vecaList.FirstOrDefault(u => u.Id == id);

            if (vecaToDelete != null)
            {
                DataStore.vecaList.Remove(vecaToDelete);
                return NoContent();
            }

            return NotFound();
        }

        [HttpPut]

        public ActionResult<VecaDto> UpdateVeca([FromBody] VecaDto vecaDto)
        {
            var vecaValue = DataStore.vecaList.FirstOrDefault(u => u.Id == vecaDto.Id);
            if (vecaDto.Id == 0)
            {
                return BadRequest();
            }

            if (vecaValue?.Id != null)
            {
                vecaValue.Name = vecaDto.Name;

                return Ok(vecaValue);
            }

            return NotFound();

        }
        [HttpPatch("{id:int}")]
        public ActionResult<VecaDto> PatchVeca(int id, [FromBody] JsonPatchDocument<VecaDto> patchDto)
        {
            var vecaValue = DataStore.vecaList.FirstOrDefault(u => u.Id == id);
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            if (vecaValue?.Id != null)
            {
                patchDto.ApplyTo(vecaValue, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}

