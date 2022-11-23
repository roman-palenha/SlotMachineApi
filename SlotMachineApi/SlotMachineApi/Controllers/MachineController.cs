using Microsoft.AspNetCore.Mvc;
using SlotMachineApi.Entities;
using SlotMachineApi.Services;

namespace SlotMachineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController: ControllerBase
    {
        private readonly IMachineService _machineService;
        public MachineController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        [HttpPut]
        public async Task<IActionResult> ChangeMachine([FromBody]string id, int newSize)
        {
            await _machineService.RefreshArray(id, newSize);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMachine(int SlotsSize)
        {
            var res = await _machineService.Create(new Machine { SlotsSize = SlotsSize });
            return Ok(res);
        }
    }
}
