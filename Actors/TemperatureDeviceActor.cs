using demo_iot_actors.Infrastructure;
using Akka.Actor;

namespace demo_iot_actors.Actors;
public class TemperatureDeviceActor : UntypedActor
{
    private IActorRef? _deviceActor;
    public TemperatureDeviceActor()
    {
        _deviceActor = Context.ActorOf(Props.Create(() => new SimulatedTemperatureSensor(Self)), "device");
    }

    //internal state
    private decimal currentTemperature = 0.0m;

    //message handling
    override protected void OnReceive(object message)
    {
        if (message is RespondSensorValue sensor)
        {
            currentTemperature = sensor.Value;
            Console.WriteLine($"TemperatureDeviceActor: {sensor.Value}");
        }
        else if (message is AdjustTemperature adjust)
        {
            _deviceActor?.Tell(adjust);
        }
        else if (message is ReadSensorValue rs)
        {
            Sender.Tell(new RespondSensorValue(currentTemperature));
        }
    }
}