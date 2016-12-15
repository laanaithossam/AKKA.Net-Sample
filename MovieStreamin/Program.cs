using System;
using Akka.Actor;
using Akka.Actor.Dsl;
using MovieStreamin.Actors;
using MovieStreamin.Messages;

namespace MovieStreamin
{
    internal class Program
    {
        private static ActorSystem _movieStreamingActorSystem;

        private static IActorRef _playbackActor;

        private static void Main(string[] args)
        {
            //Ininitialize
            _movieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");
            Console.WriteLine("Actor System cerated");

            //Create UserActor
            _playbackActor = _movieStreamingActorSystem.ActorOf(Props.Create<PlaybackActor>(), "Playback");

            do
            {
                var cmd = Console.ReadLine();

                if (cmd.StartsWith("play"))
                    PlayCommand(cmd);
                if (cmd.StartsWith("stop"))
                    StopCommand(cmd);
                if (cmd.StartsWith("exit"))
                {
                    ExitCommand();
                    break;
                }
            } while (true);

        }

        private static void ExitCommand()
        {
            _playbackActor.GracefulStop(TimeSpan.FromSeconds(2));
            _movieStreamingActorSystem.Terminate().Wait();
            Console.ReadLine();
            Environment.Exit(1);
        }

        private static void PlayCommand(string cmd)
        {
            int userId = int.Parse(cmd.Split(',')[1]);
            string movieTitle = cmd.Split(',')[2];

            var message = new PlayMovieMessage(movieTitle, userId);
            _movieStreamingActorSystem.ActorSelection("user/Playback/UserCoordinator").Tell(message);
        }

        private static void StopCommand(string cmd)
        {
            int userId = int.Parse(cmd.Split(',')[1]);
            var message = new StopMovieMessage(userId);
            _movieStreamingActorSystem.ActorSelection("user/Playback/UserCoordinator").Tell(message);
        }

        private static void Test()
        {
            //Ininitialize
            _movieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");
            Console.WriteLine("Actor System cerated");

            //Create UserActor
            Props userActorProps = Props.Create<UserActor>();
            IActorRef userActorRef = _movieStreamingActorSystem.ActorOf(userActorProps, "UserActor");
            userActorRef.Tell(new PlayMovieMessage("The Akka Movie", 30), userActorRef);
            Console.ReadKey();
            //Send PlayMovieMessage
            userActorRef.Tell(new PlayMovieMessage("The Actor", 30), userActorRef);

            Console.ReadKey();

            //Send StopMovieMessage
            Console.WriteLine("Sending Stop Movie Message");
            userActorRef.Tell(new StopMovieMessage(30), userActorRef);
            //Send PlayMovieMessage
            userActorRef.Tell(new PlayMovieMessage("The Actor", 30), userActorRef);
            Console.ReadKey();
            //Send StopMovieMessage
            Console.WriteLine("Sending another Stop Movie Message");
            userActorRef.Tell(new StopMovieMessage(30), userActorRef);

            //Shutdown Actor
            ActorRefImplicitSenderExtensions.Tell(userActorRef, PoisonPill.Instance);

            Console.ReadKey();

            //hutdown Actor system
            _movieStreamingActorSystem.Terminate();

            _movieStreamingActorSystem.WhenTerminated.ContinueWith((t) => Console.WriteLine("Actor system stopped"));

            Console.ReadLine();
        }


    }
}