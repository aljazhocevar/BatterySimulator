using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BatterySimulatorApp
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            Console.WriteLine("Battery storage simulator (.NET 8)");

            string inputPath = args.Length > 0 ? args[0] : "activation_60s.json";
            string outputPath = args.Length > 1 ? args[1] : "output.json";
            // INPUT
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return 2;
            }

            double[] requested;
            try
            {
                var json = await File.ReadAllTextAsync(inputPath);
                requested = JsonSerializer.Deserialize<double[]>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[INPUT ERROR] Failed to read/parse input JSON: {ex.Message}");
                return 3;
            }

            if (requested.Length != 60)
            {
                Console.Error.WriteLine($"[INPUT ERROR] Input must contain exactly 60 values (one per second). Found: {requested.Length}");
                return 4;
            }

            var model = new BatteryModel
            {
                CapacityMWh = 5.0,
                SoC = 0.50,
                MaxChargeMW = -1.0,
                MaxDischargeMW = 1.0,
                Efficiency = 1.0,
                DtSeconds = 1
            };

            // SIMULATION
            var simulator = new Simulator(model);
            var rows = simulator.Run(requested);

            // OUTPUT
            var options = new JsonSerializerOptions {WriteIndented = true};
            string jsonString = JsonSerializer.Serialize(rows, options);
            File.WriteAllText(outputPath, jsonString);

            Console.WriteLine($"Battery simulation finished. Wrote {rows.Count} rows to {outputPath}");
            return 0;
        }
    }
}