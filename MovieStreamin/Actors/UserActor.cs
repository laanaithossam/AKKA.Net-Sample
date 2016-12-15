using System;
using Akka.Actor;
using MovieStreamin.Messages;

namespace MovieStreamin.Actors
{
    public class UserActor : ReceiveActorBase
    {
        private readonly int _userId;
        private string _currentlyWatching;
        private Predicate<PlayMovieMessage> _predicate = m => m.UserId == 32;
        public UserActor(int userId)
        {
            _userId = userId;
            Console.WriteLine(nameof(UserActor) + _userId + "  created");
            Stopped();
        }

        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => OnPlayMovie(message));
            Receive<StopMovieMessage>(message => Console.WriteLine("Nothing to stop !"));

            Console.WriteLine($"UserActor{_userId} has now become stopped");
        }

        private void OnStopMovie()
        {
            Console.WriteLine($"User {_userId} stopped the movie {_currentlyWatching}");
            _currentlyWatching = null;
            Become(Stopped);
        }

        private void Playing()
        {
            Receive<PlayMovieMessage>(message => Console.WriteLine("Cannot start playing anoter movie, before stopping existing one !"));
            Receive<StopMovieMessage>(message => OnStopMovie());
            _currentlyWatching = null;
            Console.WriteLine($"UserActor{_userId} has now become played");
        }

        private void OnPlayMovie(PlayMovieMessage message)
        {
            _currentlyWatching = message.Title;
            Console.WriteLine($"User {_userId} is playing movie : {message.Title} ");
            Context.ActorSelection("/user/Playback/PlaybackStatistics/MoviePlayCounter").Tell(message);
            Become(Playing);
        }

        protected override ConsoleColor SuccessColor { get; set; } = ConsoleColor.Yellow;
    }
}