using System;
using System.Runtime.CompilerServices;
using Akka.Actor;

namespace MovieStreamin.Actors
{
    public abstract class ReceiveActorBase : ReceiveActor
    {
        protected abstract ConsoleColor SuccessColor { get; set; }

        protected override void PreStart()
        {
            Console.ForegroundColor = SuccessColor;
            Console.WriteLine($"{this.GetType().Name} PreStart");
            Console.ForegroundColor=ConsoleColor.White;
            base.PreStart();
        }

        protected override void PostStop()
        {
            Console.ForegroundColor = SuccessColor;
            Console.WriteLine($"{this.GetType().Name} PostStop");
            Console.ForegroundColor = ConsoleColor.White;
            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.ForegroundColor = SuccessColor;
            Console.WriteLine($"{this.GetType().Name} PostRestart");
            Console.ForegroundColor = ConsoleColor.White;
            base.PreRestart(reason, message);
        }


        protected override void PostRestart(Exception reason)
        {
            Console.ForegroundColor = SuccessColor;
            Console.WriteLine($"{this.GetType().Name} PostRestart");
            Console.ForegroundColor = ConsoleColor.White;
            base.PostRestart(reason);
        }
    }
}