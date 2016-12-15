using System;
using Akka.Actor;

namespace MovieStreamin.Actors
{
    public class PlaybackActor : ReceiveActorBase
    {
        protected override ConsoleColor SuccessColor { get; set; } = ConsoleColor.Green;

        public PlaybackActor()
        {
            Context.ActorOf(Props.Create<UserCoordinatorActor>(), "UserCoordinator");
            Context.ActorOf(Props.Create<PlaybackStatisticsActor>(), "PlaybackStatistics");
        }

        
    }
}