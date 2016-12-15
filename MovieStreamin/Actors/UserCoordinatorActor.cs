using System;
using System.Collections.Generic;
using Akka.Actor;
using MovieStreamin.Messages;

namespace MovieStreamin.Actors
{
    public class UserCoordinatorActor : ReceiveActorBase
    {
        private readonly Dictionary<int, IActorRef> _users;

        public UserCoordinatorActor()
        {
            _users = new Dictionary<int, IActorRef>();

            Receive<PlayMovieMessage>(message =>
            {
                CreateChildIfNotExists(message);
                _users[message.UserId].Tell(message);
            });

            Receive<StopMovieMessage>(message =>
            {
                _users[message.UserId].Tell(message);
            });
        }

        protected override ConsoleColor SuccessColor { get; set; } = ConsoleColor.Cyan;

        private void CreateChildIfNotExists(PlayMovieMessage message)
        {
            if (!_users.ContainsKey(message.UserId))
                _users.Add(message.UserId,
                    Context.ActorOf(Props.Create(() => new UserActor(message.UserId)), $"User{message.UserId}"));
        }
    }
}