using System;
using System.Collections.Generic;

namespace BadProject
{
    public abstract class ErrorManagerBase 
    {
        public abstract int ErrorCount { get; set; }
        public abstract void Enqueue(DateTime errorTimeStamp);
        public abstract DateTime Dequeue();
    }

    public class ErrorManager: ErrorManagerBase
    {
        private Queue<DateTime> _errors;
        private int _errorCount;

        public override int ErrorCount {
            get => _errorCount;
            set { _errorCount = value; }
        }
        public ErrorManager() 
        {
            if (_errors == null)
            {
                _errors = new Queue<DateTime>();
            }            
        }

        public Queue<DateTime> Errors { get { return _errors; } }

        public override DateTime Dequeue()
        {
            return _errors.Dequeue();
        }

        public override void Enqueue(DateTime errorTimeStamp)
        {
            _errors.Enqueue(errorTimeStamp);
        }
    }
}
