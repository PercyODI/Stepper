using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public interface IResult
    {
        bool IsSuccess { get; set; }
        Exception Exception { get; set; }
    }
}
