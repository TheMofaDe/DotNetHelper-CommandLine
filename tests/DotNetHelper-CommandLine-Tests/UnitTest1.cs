using System;
using System.Diagnostics;
using DotNetHelper_CommandLine;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]

    public class CommandLineTestFixture 
    {

        public CommandPrompt CommandPrompt { get; } = new CommandPrompt();
        public CommandLineTestFixture() 
        {


        }

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {

        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        
        }



        [SetUp]
        public void Init()
        {

        }

        [TearDown]
        public void Cleanup()
        {
 
        }




    }
}