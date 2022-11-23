using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlotMachineApi.DTO;
using SlotMachineApi.Entities;
using SlotMachineApi.Services;
using System.Drawing;
using System.Reflection.PortableExecutable;
using System;
using Machine = SlotMachineApi.Entities.Machine;

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
                return NotFound();
            }
            var newBalance = user.Balance - bet;
            if (newBalance < 0)
            {
                return BadRequest();
            }

            var machine = await _machineService.GetById(machineId);
            var resultArray = _machineService.ReturnSlotsArray(machine);
            var firstNumFromArray = resultArray[0];
            var consecutiveIdenticalDigits = resultArray.TakeWhile(x => x == firstNumFromArray);
            var win  = consecutiveIdenticalDigits.Sum(x => x) * bet;
            newBalance += win;
            await _playerService.UpdateBalanceAsync(username, newBalance);

            var res = new SpinResult { Slots = resultArray, Balance = newBalance, Win = win };

           return Ok(res);

        }
//        The bet should be deducted from the requesting players balance
//· The result array of the slot machine should be randomly selected as a single digit integer(0-9)
//for each array cell.the length of the array(size of the slot machine) is configurable and the configuration value is stored in the database.
//· The win should be calculated as the game bet multiplied by the sum of consecutive identical digits starting from position zero. for example, 3,3,3,4,5 = 9 | 2,3,2 = 2 | 7,7,5,3,1,2,3 = 14
//· The Win should be added to the player balance.
    }

}
