using Akka.Actor;

var system = ActorSystem.Create("baufest");

// blocks the main thread from exiting until the actor system is shut down
system.WhenTerminated.Wait();
