using System;
using Akka.Actor;

namespace MovieStreamin.Actors
{
    public class PlaybackStatisticsActor : ReceiveActorBase
    {
        private IActorRef _moviePlayCounterActor;
        protected override ConsoleColor SuccessColor { get; set; } = ConsoleColor.White;

        public PlaybackStatisticsActor()
        {
            _moviePlayCounterActor = Context.ActorOf(Props.Create<MoviePlayCounterActor>(), "MoviePlayCounter");
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy((exception) =>
            {
                if (exception is IndexOutOfRangeException)
                {
                    return Directive.Restart;
                }
                if (exception is FormatException)
                {
                    return Directive.Resume;
                }

                return Directive.Restart;
            });
        }
    }
}