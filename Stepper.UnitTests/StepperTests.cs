﻿using System;
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
            var stepper = new Stepper();
            stepper.AddFirstStep(() =>
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
            var stepper = new Stepper();
            stepper
                .AddFirstStep(() =>
                {
                    stepOneCount++;
                    return StepResult.Success(1);
                })
                .Then(test =>
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
            var stepper = new Stepper();
            stepper
                .AddFirstStep(() =>
                {
                    stepOneCount++;
                    return StepResult.Success("ghj");
                })
                .Then(test =>
                {
                    stepTwoCount++;
                    return StepResult.Success(1);
                })
                .Then(test =>
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
            var stepper = new Stepper();
            stepper
                .AddFirstStep(() =>
                {
                    stepOneCount++;
                    return StepResult.Success(1);
                })
                .Then<int>(test =>
                {
                    stepTwoCount++;
                    throw new Exception("Fake Exception!");
                })
                .Then(test =>
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
            var stepper = new Stepper();
            stepper
                .AddFirstStep(() =>
                {
                    stepOneCount++;
                    return StepResult.Success(1);
                })
                .Then(test =>
                {
                    stepTwoCount++;
                    return StepResult.Failure<int>();
                })
                .Then(test =>
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
            var stepper = new Stepper();
            stepper.AddFirstStep(() =>
                {
                    var stringInFirstStep = testString;
                    return StepResult.Success(stringInFirstStep);
                })
                .Then(stringUnderTest =>
                {
                    Assert.AreEqual(testString, stringUnderTest);
                    return StepResult.Success(stringUnderTest);
                });
        }

        [TestMethod]
        public void OneStep_HasBothSuccessAndFailStepResult_DoesntThrowException()
        {
            var stepper = new Stepper();
            stepper.AddFirstStep(() =>
                {
                    if (true)
                    {
                        return StepResult.Success(1);
                    }
                    return StepResult.Failure<int>();
                })
                .Then(test =>
                {

                    Console.WriteLine(test);
                    return StepResult.Success();
                });

            var jobResult = stepper.RunJob();
            Assert.AreEqual(false, jobResult.HasFailed);
        }

        [TestMethod]
        public void ThreeSteps_FromPrivateMethodGroups_ShouldRunAllThreeStepsOnce()
        {
            var stepCount = 0;
            var stepper = new Stepper();
            stepper
                .AddFirstStep<int, int>(TestStepOne)
                .Then<int>(TestStepTwo)
                .Then<int>(TestStepThree);

            var result = stepper.RunJob(stepCount);
            Assert.AreEqual(3, stepCount);
            
        }

        private StepResult<int> TestStepOne(int stepOneCount)
        {
            stepOneCount++;
            return StepResult.Success(0);
        }

        private StepResult<int> TestStepTwo(int stepTwoCount)
        {
            stepTwoCount++;
            return StepResult.Success(0);
        }

        private StepResult<int> TestStepThree(int stepThreeCount)
        {
            stepThreeCount++;
            return StepResult.Success(0);
        }
    }

    public class TestingClass
    {
        public string strOne { get; set; }
        public int intOne { get; set; }
    }
}