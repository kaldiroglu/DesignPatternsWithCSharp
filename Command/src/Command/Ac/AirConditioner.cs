namespace dev.kaldiroglu.Command.Ac;

public class AirConditioner
{
    private int _currentTemperature;
    private int _targetTemperature;
    private bool _isOn;

    public AirConditioner(Temperature currentTemperature)
    {
        _currentTemperature = currentTemperature.Value;
    }

    public void TurnOn(Temperature targetTemperature)
    {
        if (!_isOn)
        {
            _isOn = true;
            _targetTemperature = targetTemperature.Value;
            Console.WriteLine("\nFan is turned on. Target temperature is: " + targetTemperature.Value);
            if (_targetTemperature < _currentTemperature)
                TurnOnCooler(targetTemperature);
            else if (_targetTemperature > _currentTemperature)
                TurnOnHeater(targetTemperature);
        }
        else
            Console.WriteLine("\nAirConditioner is already on!");
    }

    public void TurnOff()
    {
        if (_isOn)
        {
            Console.WriteLine("AirConditioner is turned off.\n");
            _isOn = false;
            _currentTemperature = 0;
            _targetTemperature = 0;
        }
        else
            Console.WriteLine("AirConditioner is already off!\n");
    }

    public void TurnOnHeater(Temperature targetTemperature)
    {
        if (_isOn)
        {
            _targetTemperature = targetTemperature.Value;
            if (_targetTemperature > _currentTemperature)
            {
                _currentTemperature = _targetTemperature;
                Console.WriteLine("Heater is turned on. Target temperature is: " + targetTemperature.Value);
            }
        }
        else
            Console.WriteLine("AirConditioner is off, please first turn it on!");
    }

    public void TurnOnCooler(Temperature targetTemperature)
    {
        if (_isOn)
        {
            _targetTemperature = targetTemperature.Value;
            if (_targetTemperature < _currentTemperature)
            {
                _currentTemperature = _targetTemperature;
                Console.WriteLine("Cooler is turned on. Target temperature is: " + targetTemperature.Value);
            }
        }
        else
            Console.WriteLine("AirConditioner is off, please first turn it on!");
    }
}
