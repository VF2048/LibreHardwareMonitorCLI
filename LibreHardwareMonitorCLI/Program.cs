// See https://aka.ms/new-console-template for more information
using LibreHardwareMonitor.Hardware;


void Monitor()
{
    Computer computer = new Computer
    {
        IsCpuEnabled = true,
        IsGpuEnabled = true,
        IsMemoryEnabled = true,
        IsMotherboardEnabled = true,
        IsControllerEnabled = true,
        IsNetworkEnabled = true,
        IsStorageEnabled = true,
        IsBatteryEnabled = true,
        IsPsuEnabled = true,
    };

    computer.Open();
    computer.Accept(new UpdateVisitor());

    foreach (IHardware hardware in computer.Hardware)
    {
        Console.WriteLine("{0}: {1}", hardware.HardwareType, hardware.Name);

        foreach (IHardware subhardware in hardware.SubHardware)
        {
            Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

            foreach (ISensor sensor in subhardware.Sensors)
            {
                Console.WriteLine("\t{0}: {1} {2}", sensor.Name, sensor.Value, sensor.SensorType);
            }
        }

        foreach (ISensor sensor in hardware.Sensors)
        {
            Console.WriteLine("\t{0}: {1} {2}", sensor.Name, sensor.Value, sensor.SensorType);
        }
    }

    computer.Close();
}

Monitor();

public class UpdateVisitor : IVisitor
{
    public void VisitComputer(IComputer computer)
    {
        computer.Traverse(this);
    }
    public void VisitHardware(IHardware hardware)
    {
        hardware.Update();
        foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
    }
    public void VisitSensor(ISensor sensor) { }
    public void VisitParameter(IParameter parameter) { }
}