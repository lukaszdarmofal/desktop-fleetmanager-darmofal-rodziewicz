using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FleetManager.Models;

public class Vehicle : ReactiveObject
{
    [Reactive] public string Name { get; set; }
    [Reactive] public string LicensePlate { get; set; }
    [Reactive] public int FuelLevel { get; set; }
    [Reactive] public string Status { get; set; }
}