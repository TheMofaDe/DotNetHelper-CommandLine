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

        [Test]
        public void Test_RunCommand_ReturnsMessage()
        {
            var cmd = new CommandPrompt(false);
            var command = $"echo BRO";
            var hasReceivedMessage = false;
            cmd.RunCommand(command
                , delegate (object sender, EventArgs args)
                {
                    Console.WriteLine("Exit");
                    Assert.IsTrue(hasReceivedMessage, "Didn't Receive message from running command.");
                }
                , delegate (object sender, DataReceivedEventArgs args)
                {
                    Console.WriteLine(args.Data);
                    if (args.Data?.Equals("BRO") == true)
                        hasReceivedMessage = true;
                }
                , delegate(object sender, DataReceivedEventArgs args)
                {
                    Console.WriteLine(args.Data);
                }
            );
          
 
        }


        [Test]
        public void Test_RunCommand_Overload_CommandOnly()
        {
            var cmd = new CommandPrompt(false);
            var command = $"echo BRO";
            var hasReceivedMessage = false;
            var processCode = cmd.RunCommand("echo Today");
            Assert.AreEqual(0,processCode);
        }



        [Test]
        public void Test_RunCommand_Ensure_Failure_ExitCode()
        {
            var cmd = new CommandPrompt(false);
            var command = $"echo BRO";
            var hasReceivedMessage = false;
            var processCode = cmd.RunCommand("IAMNOTVALIDSYNTAX");
            Assert.AreEqual(1, processCode);
        }




        [Test]
        public void Test_GetProcess_ReturnsMessage()
        {
            var cmd = new CommandPrompt(false);
            var command = $"echo BRO";
            var hasReceivedMessage = false;
            var process = cmd.GetProcess(command
                , delegate (object sender, EventArgs args)
                {
                    Console.WriteLine("Exit");
                    Assert.IsTrue(hasReceivedMessage, "Didn't Receive message from running command.");
                }
                , delegate (object sender, DataReceivedEventArgs args)
                {
                    Console.WriteLine(args.Data);
                    if (args.Data?.Equals("BRO") == true)
                        hasReceivedMessage = true;
                }
                , delegate (object sender, DataReceivedEventArgs args)
                {
                    Console.WriteLine(args.Data);
                }
            );
            Assert.IsNotNull(process);

        }


        [Test]
        public void Test_GetProcess_Overload_CommandOnly()
        {
            var cmd = new CommandPrompt(false);
            var command = $"echo BRO";
            var process = cmd.GetProcess("echo Today");
            Assert.AreEqual(0, process.ExitCode);
        }







        [Test]
        [Ignore("// MUST HAVE LOCAL ACCOUNT WITH THIS USERNAME AND PASSWORD")]
        public void Test_RunCommand_WithUsernameAndPassword()
        {
            var cmd = new CommandPrompt("Administrator1","Password@123",false); 
            var command = $"echo BRO";
            var hasReceivedMessage = false;
            cmd.RunCommand(command
                , delegate (object sender, EventArgs args)
                {
                    Console.WriteLine("Exit");
                    Assert.IsTrue(hasReceivedMessage, "Didn't Receive message from running command.");
                }
                , delegate (object sender, DataReceivedEventArgs args)
                {
                    Console.WriteLine(args.Data);
                    if (args.Data?.Equals("BRO") == true)
                        hasReceivedMessage = true;
                }
                , delegate (object sender, DataReceivedEventArgs args)
                {
                    Console.WriteLine(args.Data);
                }
            );

        }

       



    }
}