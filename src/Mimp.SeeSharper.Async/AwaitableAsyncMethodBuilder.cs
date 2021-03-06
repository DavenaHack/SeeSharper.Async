using Mimp.SeeSharper.Async.Abstraction;
using System;
using System.Runtime.CompilerServices;

namespace Mimp.SeeSharper.Async
{
    public struct AwaitableAsyncMethodBuilder
    {

        public static AwaitableAsyncMethodBuilder Create() =>
            new AwaitableAsyncMethodBuilder();


        public IAwaitable Task => new TaskAwaitable(_builder.Task);


        private AsyncTaskMethodBuilder _builder;


        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            _builder = AsyncTaskMethodBuilder.Create();
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

    public struct AwaitableAsyncMethodBuilder<T>
    {

        public static AwaitableAsyncMethodBuilder<T> Create() =>
            new AwaitableAsyncMethodBuilder<T>();


        public IAwaitable<T> Task => new TaskAwaitable<T>(_builder.Task);


        private AsyncTaskMethodBuilder<T> _builder;


        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            _builder = AsyncTaskMethodBuilder<T>.Create();
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
