using System;
using System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public struct ValueTaskAwaitableAsyncMethodBuilder
    {

        public static ValueTaskAwaitableAsyncMethodBuilder Create() =>
            new ValueTaskAwaitableAsyncMethodBuilder();


        public ValueTaskAwaitable Task => new ValueTaskAwaitable(_builder.Task);


        private AsyncValueTaskMethodBuilder _builder;


        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            _builder = AsyncValueTaskMethodBuilder.Create();
            _builder.Start(ref stateMachine);
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine) =>
            _builder.SetStateMachine(stateMachine);

        public void SetException(Exception exception) =>
            _builder.SetException(exception);

        public void SetResult() =>
            _builder.SetResult();


        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine =>
            _builder.AwaitOnCompleted(ref awaiter, ref stateMachine);

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine =>
            _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);


    }

    public struct ValueTaskAwaitableAsyncMethodBuilder<T>
    {

        public static ValueTaskAwaitableAsyncMethodBuilder<T> Create() =>
            new ValueTaskAwaitableAsyncMethodBuilder<T>();


        public ValueTaskAwaitable<T> Task => new ValueTaskAwaitable<T>(_builder.Task);


        private AsyncValueTaskMethodBuilder<T> _builder;


        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            _builder = AsyncValueTaskMethodBuilder<T>.Create();
            _builder.Start(ref stateMachine);
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine) =>
            _builder.SetStateMachine(stateMachine);

        public void SetException(Exception exception) =>
            _builder.SetException(exception);

        public void SetResult(T result) =>
            _builder.SetResult(result);


        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine =>
            _builder.AwaitOnCompleted(ref awaiter, ref stateMachine);

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine =>
            _builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);


    }
}
