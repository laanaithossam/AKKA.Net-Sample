using System;
using System.Collections.Generic;
using MovieStreamin.Messages;

namespace MovieStreamin.Actors
{
    public class MoviePlayCounterActor : ReceiveActorBase
    {
        private Dictionary<string, int> _moviePlayCount;
        protected override ConsoleColor SuccessColor { get; set; } = ConsoleColor.Magenta;

        public MoviePlayCounterActor()
        {
            _moviePlayCount = new Dictionary<string, int>();

            Receive<PlayMovieMessage>(message => OnIncrementCount(message));
        }

        private void OnIncrementCount(PlayMovieMessage message)
        {
            if (message.Title == "Buggy")
                throw new FormatException();

            if (!_moviePlayCount.ContainsKey(message.Title))
                _moviePlayCount.Add(message.Title, 0);
            _moviePlayCount[message.Title]++;

            if (_moviePlayCount[message.Title] > 3)
                throw new IndexOutOfRangeException();


        }
    }
}