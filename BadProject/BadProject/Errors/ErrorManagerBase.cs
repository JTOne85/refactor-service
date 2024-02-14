using System;
using System.Collections.Generic;

namespace BadProject.Errors
{
    public abstract class ErrorManagerBase : IErrorManager
    {
        public abstract int ErrorCount { get; set; }

        public abstract Queue<DateTime> Errors { get; set; }

        public abstract void Enqueue(DateTime errorTimeStamp);

        public abstract DateTime Dequeue();
    }
}
