using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper.Exceptions
{
    class WrongInputTypeException : Exception
    {
        public WrongInputTypeException()
        {
        }

        public WrongInputTypeException(string message)
            : base(message)
        {
        }

        public WrongInputTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
