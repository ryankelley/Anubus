﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nimbus.IntegrationTests.InfrastructureContracts;
using NUnit.Framework;

namespace Nimbus.IntegrationTests
{
    [TestFixture]
    [Timeout(15*1000)]
    public abstract class TestForAllBuses
    {
        protected Bus Bus { get; private set; }

        [SetUp]
        public void SetUp()
        {
            //Console.WriteLine("Sleeping (allowing R#'s test runner to sort itelf out...)");
            //Thread.Sleep(TimeSpan.FromSeconds(2));

            MethodCallCounter.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine();
            Console.WriteLine();
            Bus.Dispose();
        }

        public virtual async Task Given(ITestHarnessBusFactory busFactory)
        {
            Bus = (Bus) busFactory.Create();

            Console.WriteLine();
            Console.WriteLine();
        }

        public abstract Task When();

        public IEnumerable<TestCaseData> AllBusesTestCases
        {
            get
            {
                // ReSharper disable LoopCanBeConvertedToQuery
                var testFixtureType = GetType();
                var busFactoryEnumerator = new BusFactoryEnumerator(testFixtureType);

                foreach (var factory in busFactoryEnumerator.GetBusFactories())
                {
                    yield return new TestCaseData(factory)
                        .SetName(factory.MessageBrokerName)
                        ;
                }
                // ReSharper restore LoopCanBeConvertedToQuery
            }
        }
    }
}