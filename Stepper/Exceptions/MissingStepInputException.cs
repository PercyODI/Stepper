using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper.Exceptions
{
    class MissingStepInputException : Exception
    {
        public MissingStepInputException()
        {
        }

        public MissingStepInputException(string message)
            : base(message)
        {
        }

        public MissingStepInputException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
