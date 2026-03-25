using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FleetManager.ViewModels;

public class VehicleItemViewModel : ReactiveObject
{
    private int _fuelLevel;
    private string _status;

    public string Name { get; }
    public string LicensePlate { get; }
    
    public int FuelLevel
    {
        get => _fuelLevel;
        set => this.RaiseAndSetIfChanged(ref _fuelLevel, value);
    }

    public string Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    public ICommand RefuelCommand { get; }
    public ICommand SendToServiceCommand { get; }
    
    public ICommand SendOnRoadCommand { get; }

    public VehicleItemViewModel()
    {
        RefuelCommand = ReactiveCommand.Create(() =>
        {
            FuelLevel = 100;
        });

        SendToServiceCommand = ReactiveCommand.Create(() =>
        {
            if (Status != "Serwis")
            {
                Status = "Serwis";
            }
        });

        SendOnRoadCommand = ReactiveCommand.Create(() => 
        {
            if (Status != "W trasie" || FuelLevel > 15)
            {
                Status = "W trasie";
                FuelLevel -= 20;
            }
        });
    }
}