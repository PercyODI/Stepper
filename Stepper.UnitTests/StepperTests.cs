using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stepper.UnitTests
{
    [TestClass]
    public class StepperTests
    {
        [TestMethod]
        public void OneStep_UsingLambdaFunction_RunsFunctionOnce()
        {
            var runCount = 0;
            var stepper = new Stepper<TestingClass>();
            stepper.AddFirstStep<TestingClass, int>(test =>
            {
                runCount++;
                return StepResult.Success(0);
            });

            stepper.RunJob();

            Assert.AreEqual(1, runCount);
        }

        [TestMethod]
        public void TwoSteps_UsingLambdaFunctions_RunsEachStepOnce()
        {
            var stepOneCount = 0;
            var stepTwoCount = 0;
            var stepper = new Stepper<TestingClass>();
            stepper
                .AddFirstStep<TestingClass, int>(test =>
                {
                    stepOneCount++;
                    return StepResult.Success(1);
                })
                .Then<int, int>(test =>
                {
                    stepTwoCount++;
                    return StepResult.Success(1);
                });

            stepper.RunJob();

            Assert.AreEqual(1, stepOneCount);
            Assert.AreEqual(1, stepTwoCount);
        }

        [TestMethod]
        public void ThreeSteps_UsingLambdaFunctions_RunsEachStepOnce()
        {
            var stepOneCount = 0;
            var stepTwoCount = 0;
            var stepThreeCount = 0;
            var stepper = new Stepper<TestingClass>();
            stepper
                .AddFirstStep<TestingClass, int>(test =>
                {
                    stepOneCount++;
                    return StepResult.Success(1);
                })
                .Then<int, int>(test =>
                {
                    stepTwoCount++;
                    return StepResult.Success(1);
                })
                .Then<int, int>(test =>
                {
                    stepThreeCount++;
                    return StepResult.Success(1);
                });

            stepper.RunJob();

            Assert.AreEqual(1, stepOneCount);
            Assert.AreEqual(1, stepTwoCount);
            Assert.AreEqual(1, stepThreeCount);
        }

        [TestMethod]
        public void ThreeSteps_SecondStepThrowsException_DoesntRunThirdStep()
        {
            var stepOneCount = 0;
            var stepTwoCount = 0;
            var stepThreeCount = 0;
            var stepper = new Stepper<TestingClass>();
            stepper
                .AddFirstStep<TestingClass, int>(test =>
                {
                    stepOneCount++;
                    return StepResult.Success(1);
                })
                .Then<int, int>(test =>
                {
                    stepTwoCount++;
                    throw new Exception("Fake Exception!");
                })
                .Then<int, int>(test =>
                {
                    stepThreeCount++;
                    return StepResult.Success(1);
                });

            stepper.RunJob();

            Assert.AreEqual(1, stepOneCount);
            Assert.AreEqual(1, stepTwoCount);
            Assert.AreEqual(0, stepThreeCount);
        }

        [TestMethod]
        public void ThreeSteps_SecondStepReturnsFailedStepResult_DoesntRunThirdStep()
        {
            var stepOneCount = 0;
            var stepTwoCount = 0;
            var stepThreeCount = 0;
            var stepper = new Stepper<TestingClass>();
            stepper
                .AddFirstStep<TestingClass, int>(test =>
                {
                    stepOneCount++;
                    return StepResult.Success(1);
                })
                .Then<int, int>(test =>
                {
                    stepTwoCount++;
                    return StepResult.Failure();
                })
                .Then<int, int>(test =>
                {
                    stepThreeCount++;
                    return StepResult.Success(1);
                });

            stepper.RunJob();

            Assert.AreEqual(1, stepOneCount);
            Assert.AreEqual(1, stepTwoCount);
            Assert.AreEqual(0, stepThreeCount);
        }

        [TestMethod]
        public void ThreeSteps_FirstStepCreatesString_SecondStepGetsString()
        {
            var testString = "This is a test string";
            var stepper = new Stepper<string>();
            stepper.AddFirstStep<string, string>(_ =>
                {
                    var stringInFirstStep = testString;
                    return StepResult.Success(stringInFirstStep);
                })
                .Then<string, string>(stringUnderTest =>
                {
                    Assert.AreEqual(testString, stringUnderTest);
                    return StepResult.Success(stringUnderTest);
                });
        }
    }

    public class TestingClass
    {
        public string strOne { get; set; }
        public int intOne { get; set; }
    }
}