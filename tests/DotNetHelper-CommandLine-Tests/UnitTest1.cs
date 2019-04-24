using DotNetHelper_Contracts.Tools;
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