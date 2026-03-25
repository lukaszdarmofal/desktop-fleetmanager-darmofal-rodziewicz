using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Text.Json;
using FleetManager.Models;

namespace FleetManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private const string FilePath = "Assets/vehicles.json";
    private static readonly JsonSerializerOptions _options = new() {WriteIndented = true};
    
    public ObservableCollection<Vehicle> Vehicles { get; } = [];

    
    public MainWindowViewModel()
    {
       LoadVehicles(); 
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