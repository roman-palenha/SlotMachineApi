using Microsoft.AspNetCore.Http;
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
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateBalance(string username, double balance)
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
        [Route("{username}/{bet}")]
        public async Task<IActionResult> Bet(string username, double bet)
        {
            var user = await _playerService.GetByNameAsync(username);
            var newBalance = user.Balance - bet;
            if (newBalance < 0)
            {
                return BadRequest();
            }
            await _playerService.UpdateBalanceAsync(username,newBalance);

            await _machineService.Create(new Machine());

            var resultArray = _machineService.RefreshArray();
        }
    }
}
