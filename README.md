# Battery Storage Simulator (.NET 8)

This is a simple .NET 8 console application that simulates a battery storage system for 60 seconds.

## Contents
- `Program.cs` - entry point, reads input JSON and writes JSON output.
- `Simulator.cs` - simulation logic.
- `BatteryModel.cs` - battery parameters.
- `activation_60s.json` - example input with 60 values (MW).
- `output.json` - generated after running (not included by default).

## How to build & run
Requires .NET 8 SDK installed.

```bash
navigate to: BatterySimulator/src/
dotnet build
dotnet run BatterySimulator.csproj -- ../activation_60s.json ../output.json
```

If you omit args and onlz dotnet run is used, defaults are `../activation_60s.json` and `../output.json` in the working directory.

## How to build & run tests
```bash
navigate to: BatterySimulator/tests/
dotnet build
dotnet test
```

## Nice to have / Future improvements

The following ideas would improve usability, robustness, and realism of the simulator.

1. Configurable battery parameters
Allow setting capacity, max charge/discharge power, initial SoC, etc. via a JSON config file or CLI arguments.

Example:
```bash
dotnet run --project src/BatterySimulator/BatterySimulator.csproj -- activation_60s.json output.json --capacity=10 --soc=0.8
```

2. Summary report at the end of simulation
Print or export key metrics such as final SoC, total charged/discharged energy, and number of seconds at power or SoC limits.

3. Extended test coverage
Add integration tests with real input profiles, edge cases (0 % SoC + discharge, 100 % SoC + charge), and statistical checks to ensure SoC stays within limits for the entire run.

4. Visualization
Generate simple graphs (for example with ScottPlot or Plotly.NET) to show SoC and power over time.
Alternatively, instruct users to import the output CSV into Excel or another plotting tool.

7. Longer simulation support
Allow input sequences longer than 60 seconds to handle hour- or day-long battery operation profiles.

8. Continuous Integration (CI)
Integrate tests with GitHub Actions or another CI system to ensure correctness on each commit.
