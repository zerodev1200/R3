﻿namespace R3
{
    public static partial class EventFactory
    {
        public static CompletableEvent<TMessage, Unit> Empty<TMessage>()
        {
            return R3.Factories.Empty<TMessage>.Instance;
        }

        public static CompletableEvent<TMessage, Unit> Empty<TMessage>(TimeProvider timeProvider)
        {
            return ReturnOnCompleted<TMessage, Unit>(default, timeProvider);
        }

        public static CompletableEvent<TMessage, Unit> Empty<TMessage>(TimeSpan dueTime, TimeProvider timeProvider)
        {
            return ReturnOnCompleted<TMessage, Unit>(default, dueTime, timeProvider);
        }
    }
}

namespace R3.Factories
{
    internal sealed class Empty<TMessage> : CompletableEvent<TMessage, Unit>
    {
        // singleton
        public static readonly Empty<TMessage> Instance = new Empty<TMessage>();

        protected override IDisposable SubscribeCore(Subscriber<TMessage, Unit> subscriber)
        {
            subscriber.OnCompleted(default);
            return Disposable.Empty;
        }

        Empty()
        {

        }
    }
}