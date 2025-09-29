namespace BatterySimulatorApp
{
    public class BatteryModel
    {
        // Battery parameters
        public double CapacityMWh { get; set; } = 5.0;
        public double SoC { get; set; } = 0.5;
        public double MaxChargeMW { get; set; } = -1.0;    // charging
        public double MaxDischargeMW { get; set; } = 1.0;  // discharging
        public double Efficiency { get; set; } = 1.0; // this is ignored
        public int DtSeconds { get; set; } = 1;
    }
}