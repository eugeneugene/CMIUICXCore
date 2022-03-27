using System;
using System.Threading;
using System.Threading.Tasks;

namespace CMIUICXCore.Code
{
    /// <summary>
    /// Defines the <see cref="AsyncHelper" />.
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        /// Defines the _taskFactory.
        /// </summary>
        private static readonly TaskFactory _taskFactory = new(CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);

        /// <summary>
        /// The RunSync.
        /// </summary>
        /// <typeparam name="TResult">.</typeparam>
        /// <param name="func">The func<see cref="Func{Task}"/>.</param>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
            => _taskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        /// <summary>
        /// The RunSync.
        /// </summary>
        /// <param name="func">The func<see cref="Func{Task}"/>.</param>
        public static void RunSync(Func<Task> func)
            => _taskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
    }
}
