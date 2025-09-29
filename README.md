# Battery Storage Simulator (.NET 8)

This is a simple .NET 8 console application that simulates a battery storage system for 60 seconds.

## Contents
- `Program.cs` - entry point, reads input JSON and writes JSON output.
- `Simulator.cs` - simulation logic.
- `BatteryModel.cs` - battery parameters.
- `activation_60s.json` - example input with 60 values (MW).
- `output.csv` - generated after running (not included by default).

## How to build & run
Requires .NET 8 SDK installed.

```bash
dotnet build
dotnet run --project BatterySimulator.csproj -- activation_60s.json output.json
```

If you omit args, defaults are `activation_60s.json` and `output.json` in the working directory.