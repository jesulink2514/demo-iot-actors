using Akka.Actor;
using demo_iot_actors.Actors;
using demo_iot_actors.Infrastructure;

var system = ActorSystem.Create("baufest");

// create top-level actors within the actor system
Props temperatureProps = Props.Create<TemperatureDeviceActor>();
IActorRef temperatureActor = system.ActorOf(temperatureProps, "consoleReaderActor");

var answer = string.Empty;
do
{
    PrintInstructions();
    answer = Console.ReadLine();
    switch (answer)
    {
        case "a":
            temperatureActor.Tell(new AdjustTemperature(5.0m));
            break;
        case "d":
            temperatureActor.Tell(new AdjustTemperature(-5.0m));
            break;
        case "r":
            var temperature = await temperatureActor.Ask<RespondSensorValue>(new ReadSensorValue());
            Console.WriteLine($"Temperature: {temperature.Value}");
            break;
        case "q":
            await system.Terminate();
            break;
        default:
            Console.WriteLine("Invalid option");
            break;
    }
}
while (answer != "q");

// blocks the main thread from exiting until the actor system is shut down
system.WhenTerminated.Wait();

void PrintInstructions()
{
    Console.WriteLine("Press 'a' to increase the temperature");
    Console.WriteLine("Press 'd' to decrease the temperature");
    Console.WriteLine("Press 'r' to query the temperature");
    Console.WriteLine("Press 'q' to quit");
}

