using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices.Swift;
using System.Text.Json;
using FleetManager.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FleetManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private const string FilePath = "Assets/vehicles.json";
    private static readonly JsonSerializerOptions _options = new() {WriteIndented = true};
    
    public ObservableCollection<Vehicle> Vehicles { get; } = [];

    public ReactiveCommand<Vehicle, Unit> RefuelCommand { get; }
    public ReactiveCommand<Vehicle, Unit> SendToServiceCommand { get; }

    public ReactiveCommand<Vehicle, Unit> SendBackCommand { get; }
    public ReactiveCommand<Vehicle, Unit> SendOnRoadCommand { get; }
    
    [Reactive] public string Name { get; set; }
    [Reactive] public string LicensePlate { get; set; }
    [Reactive] public int FuelLevel { get; set; }
    [Reactive] public string Status { get; set; }
    
    public MainWindowViewModel()
    {
       LoadVehicles(); 
       
       RefuelCommand = ReactiveCommand.Create<Vehicle>(vehicle =>
       {
           vehicle.FuelLevel = 100;
           Console.WriteLine("Fuel Level: " + vehicle.FuelLevel);
       });

       SendToServiceCommand = ReactiveCommand.Create<Vehicle>(vehicle =>
       {
           if (vehicle.Status != "Service")
           {
               vehicle.Status = "Service";
               Console.WriteLine("Status: " + vehicle.Status);
           }
       });

       SendOnRoadCommand = ReactiveCommand.Create<Vehicle>(vehicle => 
       {
           if (vehicle.Status == "Available" )
           {               
               if (vehicle.FuelLevel - 20 < 0)
               {
                   vehicle.FuelLevel = 0;
                   Console.WriteLine("Za mało paliwa");
               }
               else
               {
                   vehicle.Status = "InRoute";
                   vehicle.FuelLevel -= 20;
                   Console.WriteLine("Status: " + vehicle.Status);
                   Console.WriteLine("FuelLevel: " + vehicle.FuelLevel);
               }
               
           }
       });

       SendBackCommand = ReactiveCommand.Create<Vehicle>(vehicle =>
       {
           if (vehicle.Status != "Available")
           {
               vehicle.Status = "Available";
           }
           else
           {
               Console.WriteLine("Jest już w bazie");
           }
       });

    }

    private void LoadVehicles()
    {
        if (!File.Exists(FilePath))
        {
            Console.WriteLine("Nie znaleziono pliku");
            return;
        }
        
        var jsonData = File.ReadAllText(FilePath);
        var list = JsonSerializer.Deserialize<List<Vehicle>>(jsonData, _options);
        Vehicles.Clear();

        if (list == null) return;
        foreach (var vehicle in list)
        {
            Vehicles.Add(vehicle);
        }
    }
}