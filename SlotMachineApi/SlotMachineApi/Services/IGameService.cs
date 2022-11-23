using SlotMachineApi.Entities;

namespace SlotMachineApi.Services
{
    public interface IGameService
    {
         Task Update(Game game, int newSize);
         int[] ReturnSlotsArray(Game game);

    }
}
