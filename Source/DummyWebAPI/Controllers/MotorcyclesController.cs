using DummyWebAPI.Core.Models;
using DummyWebAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class MotorcyclesController : ControllerBase
    {
        private readonly IMotorcycleService motorcycleService;

        public MotorcyclesController(IMotorcycleService motorcycleService)
        {
            this.motorcycleService = motorcycleService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MotorcycleDTO>> GetMotorcycleAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var motorcycle = await motorcycleService.GetMotorcycleAsync(id, cancellationToken);

            if (motorcycle is null)
            {
                return NotFound();
            }

            return Ok(motorcycle);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MotorcycleDTO>>> GetMotorcyclesAsync(
            CancellationToken cancellationToken)
        {
            var motorcycles = await motorcycleService.GetAllMotorcyclesAsync(cancellationToken);

            return Ok(motorcycles);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateMotorcycleAsync(
            MotorcycleDTO motorcycleDTO,
            CancellationToken cancellationToken)
        {
            var result = await motorcycleService.CreateMotorcycleAsync(motorcycleDTO, cancellationToken);

            if (result < 1)
            {
                return BadRequest();
            }

            return CreatedAtRoute(result, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMotorcycleAsync(
            int id,
            MotorcycleDTO motorcycleDTO,
            CancellationToken cancellationToken)
        {
            var actionResult = await GetMotorcycleAsync(id, cancellationToken);

            if ((actionResult.Result as OkObjectResult)?.Value is not MotorcycleDTO)
            {
                return NotFound();
            }

            var updateSuccessful = await motorcycleService.UpdateMotorcycleAsync(id, motorcycleDTO, cancellationToken);

            if (!updateSuccessful)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMotorcycleAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var motorcycleById = await GetMotorcycleAsync(id, cancellationToken);

            if (motorcycleById == null)
            {
                return NotFound();
            }

            var result = await motorcycleService.DeleteMotorcycleAsync(id, cancellationToken);

            if (result < 1)
            {
                return BadRequest();
            }

            return Ok(result);
        }

    }
}
