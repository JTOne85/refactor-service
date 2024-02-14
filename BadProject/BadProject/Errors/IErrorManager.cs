using System;
using System.Collections.Generic;

namespace BadProject.Errors
{
    public interface IErrorManager
    {
        int ErrorCount { get; set; }

        Queue<DateTime> Errors { get; set; }

        void Enqueue(DateTime errorTimeStamp);

        DateTime Dequeue();
    }
}
