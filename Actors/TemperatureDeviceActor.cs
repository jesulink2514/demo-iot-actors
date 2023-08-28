using demo_iot_actors.Infrastructure;
using Akka.Actor;

namespace demo_iot_actors.Actors;

public record ForbiddenAction(string Reason);

public class TemperatureDeviceActor : ReceiveActor
{
    private decimal currentTemperature = 0.0m;

    private IActorRef _deviceActor;
    public TemperatureDeviceActor()
    {
        _deviceActor = Context.ActorOf(Props.Create(() => new SimulatedTemperatureSensor(Self)), "device");

        Idle();
    }

    private void Adjusting()
    {
        Receive<AdjustTemperature>(adjust =>
        {
            Console.WriteLine("[Forbidden] Already adjusting temperature...");
        });

        Receive<RespondSensorValue>(rs =>
        {
            Become(Idle);
            Self.Tell(rs);
        });

        Receive<ReadSensorValue>(rs => Sender.Tell(new RespondSensorValue(currentTemperature)));
    }

    private void Idle()
    {
        Receive<RespondSensorValue>(sensor =>
        {
            currentTemperature = sensor.Value;
            //Console.WriteLine($"TemperatureDeviceActor: {sensor.Value} C°");
        });

        Receive<AdjustTemperature>(adjust =>
        {
            Become(Adjusting);
            _deviceActor?.Tell(adjust);
        });

        Receive<ReadSensorValue>(rs =>
        {
            Sender.Tell(new RespondSensorValue(currentTemperature));
        });
    }
}