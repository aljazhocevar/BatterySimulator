using System;
using System.Collections.Generic;

namespace BatterySimulatorApp
{
    public class SimulationRow
    {
        public int Second { get; set; }
        public double RequestedMW { get; set; }
        public double AppliedMW { get; set; }
        public double SoC { get; set; }
    }

    public class Simulator
    {
        private readonly BatteryModel _model;

        public Simulator(BatteryModel model)
        {
            _model = model;
        }

        public List<SimulationRow> Run(double[] requestedSequence)
        {
            var rows = new List<SimulationRow>();
            double soc = _model.SoC;
            double cap = _model.CapacityMWh;
            int dt = _model.DtSeconds;

            for (int t = 0; t < requestedSequence.Length; t++)
            {
                double req = requestedSequence[t];
                double applied = Math.Max(Math.Min(req, _model.MaxDischargeMW), _model.MaxChargeMW);

                // energy = appliedPower (MW) * dt (s) / 3600
                double energyMWh = applied * (dt / 3600.0);

                double tentativeSoC = soc - (energyMWh / cap);

                if (tentativeSoC < 0.0)
                {
                    double maxExtractableMWh = soc * cap; // MWh available to discharge

                    if (energyMWh > 0) // discharging
                    {
                        double allowedEnergy = Math.Min(maxExtractableMWh, energyMWh);
                        applied = (allowedEnergy * 3600.0) / dt;
                        energyMWh = allowedEnergy;
                        tentativeSoC = 0.0;
                    }
                    else
                    {
                        tentativeSoC = Math.Max(0.0, tentativeSoC);
                    }
                }
                else if (tentativeSoC > 1.0)
                {
                    double maxAbsorbableMWh = (1.0 - soc) * cap;
                    if (energyMWh < 0) // charging (applied < 0)
                    {
                        double neededEnergy = -energyMWh;
                        double allowedEnergy = Math.Min(maxAbsorbableMWh, neededEnergy);
                        applied = -(allowedEnergy * 3600.0) / dt;
                        energyMWh = -allowedEnergy;
                        tentativeSoC = 1.0;
                    }
                    else
                    {
                        tentativeSoC = Math.Min(1.0, tentativeSoC);
                    }
                }
                soc = soc - (energyMWh / cap);

                // Safety clamp (numeric)
                soc = Math.Max(0.0, Math.Min(1.0, soc));
                applied = Math.Max(Math.Min(applied, _model.MaxDischargeMW), _model.MaxChargeMW);

                rows.Add(new SimulationRow
                {
                    Second = t,
                    RequestedMW = req,
                    AppliedMW = applied,
                    SoC = soc
                });
            }

            return rows;
        }
    }
}