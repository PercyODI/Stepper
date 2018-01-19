using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stepper.Exceptions;

namespace Stepper
{
    public abstract class Step
    {
        protected Delegate StepFunc { get; set; }
        protected Step NextStep { get; set; }

        protected Step()
        {
        }

        public Step(Delegate stepFunc)
        {
            StepFunc = stepFunc;
        }

        public abstract void RunStep(JobResult jobResult);

        public Step Then(Step nextStep)
        {
            NextStep = nextStep;
            return NextStep;
        }


    }
}