using Akka.Actor;

namespace demo_iot_actors.Infrastructure;

public class SimulatedTemperatureSensor : UntypedActor
{
    private readonly IActorRef _readerActor;
    public SimulatedTemperatureSensor(IActorRef readerActor)
    {
        _readerActor = readerActor;
    }

    private decimal _temperatureBase = 20.0m;

    override protected void PreStart()
    {
        Context.System.Scheduler.ScheduleTellRepeatedly(
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(1),
            Self,
            new ReadSensorValue(),
            Self
        );
    }
    override protected void OnReceive(object message)
    {
        if (message is ReadSensorValue)
        {
            var r = new Random();
            var value = r.Next(0, 5) + _temperatureBase;
            _readerActor.Tell(new RespondSensorValue(value));
        }
        else if (message is AdjustTemperature adjust)
        {
            _temperatureBase += adjust.TemperatureAdjustment;
            Self.Tell(new ReadSensorValue(), Self);
        }
    }
}
