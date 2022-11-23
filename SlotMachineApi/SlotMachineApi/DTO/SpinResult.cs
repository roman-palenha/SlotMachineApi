using SlotMachineApi.Entities;

namespace SlotMachineApi.DTO
{
    public class SpinResult
    {
        public int[] Slots { get; set; } 
        Player Player { get; set; }
    }
}
