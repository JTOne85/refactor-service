using System;
using System.Collections.Generic;

namespace BadProject.Errors
{

    public class ErrorManager : ErrorManagerBase
    {
        private Queue<DateTime> _errors;
        private int _errorCount;
        public override Queue<DateTime> Errors
        {
            get
            {
                if (Errors == null)
                {
                    _errors = new Queue<DateTime>();
                    return _errors;
                }
                else return Errors;
            }
            set
            {
                _errors = value;
            }
        }

        public override int ErrorCount
        {
            get => _errorCount;
            set { _errorCount = value; }
        }

        public ErrorManager()
        {
            if (Errors == null)
            {
                Errors = new Queue<DateTime>();
            }
        }

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
