using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class StepResult<T> : IResult
    {
        public bool EndJob { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
        private T PassingObj { get; set; }
    }
}
