using Microsoft.AspNetCore.Mvc;
using SlotMachineApi.DTO;
using SlotMachineApi.Entities;
using SlotMachineApi.Services;

namespace SlotMachineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IMachineService _machineService;

        public PlayerController(IPlayerService playerService, IMachineService machineService)
        {
            _playerService = playerService;
            _machineService = machineService;   
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _playerService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterPlayer registerPlayer)
        {
            try
            {
                var player = new Player
                {
                    UserName = registerPlayer.UserName,
                    Balance = registerPlayer.Balance
                };
                await _playerService.CreateAsync(player);
                return Ok(player);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateBalance(string username, [FromBody]double balance)
        {
            try
            {
                await _playerService.UpdateBalanceAsync(username, balance);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{username}/{machineId}/{bet}")]
        public async Task<IActionResult> Bet([FromRoute] string username, double bet, string machineId)
        {
            var user = await _playerService.GetByNameAsync(username);
            if(user == null)
            {
                return NotFound("User not found");
            }

            var newBalance = user.Balance - bet;
            if (newBalance < 0)
            {
                return BadRequest("Negative balance");
            }

            var machine = await _machineService.GetById(machineId);

            var resultArray = _machineService.ReturnSlotsArray(machine);

            var firstNumFromArray = resultArray[0];

            var win = resultArray
                .TakeWhile(x => x == firstNumFromArray)
                .Sum(x => x) 
                * bet;

            newBalance += win;

            await _playerService.UpdateBalanceAsync(username, newBalance);

            var res = new SpinResult { Slots = resultArray, Balance = newBalance, Win = win };

           return Ok(res);
        }
    }
}
