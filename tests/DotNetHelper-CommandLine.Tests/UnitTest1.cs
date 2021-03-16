using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DotNetHelper_CommandLine;
using Xunit;


namespace Tests
{
	public class CommandPromptTests
	{
		public MemoryStream MemoryStream = new();
		public StreamWriter StreamWriter { get; }
		public CommandPromptTests()
		{
			//StreamWriter = new StreamWriter(MemoryStream) {AutoFlush = true};
			//Console.SetOut(StreamWriter);
		}

#if NET5_0_OR_GREATER
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task RunCommandAndWaitForExitAsync_WithInvalidCommand_ShouldReturnBadExitCode(bool hideWindow)
		{
			// Arrange
			var cmd = new CommandPrompt(hideWindow);
			var actualErrorValue = string.Empty;
			int? exitCode = null;
			var command = $"ec";

			cmd.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs args)
			{
				if (args.Data is not null)
					actualErrorValue = args.Data;
			};
			// Act
			var exception = await Record.ExceptionAsync(async () =>
			{
				var process = await cmd.RunCommandAndWaitForExitAsync(command);
				exitCode = process.ExitCode;
			});


			//Assert
			Assert.Null(exception);
			Assert.Equal(1, exitCode);
			Assert.NotEmpty(actualErrorValue.ToCharArray());
		}


		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async Task RunCommandAndWaitForExitAsync_WithEchoAsCommand_ShouldEcho(bool hideWindow)
		{
			// Arrange
			var cmd = new CommandPrompt(hideWindow);
			var actualValue = string.Empty;
			var expectedValue = "myname";
			int? exitCode = null;
			var command = $"echo {expectedValue}";

			cmd.OutputDataReceived += delegate(object sender, DataReceivedEventArgs args)
			{
				if(args.Data is not null)
					actualValue = args.Data;
			};

			// Act
			var exception = await Record.ExceptionAsync(async () =>
			{
				var process = await cmd.RunCommandAndWaitForExitAsync(command);
				exitCode = process.ExitCode;
			});
			
			
			//Assert
			Assert.Null(exception);
			Assert.Equal(0,exitCode);
			Assert.Equal(expectedValue, actualValue);
		}
#endif

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void RunCommand_WithEchoAsCommand_ShouldEcho(bool hideWindow)
		{
			// Arrange
			var cmd = new CommandPrompt(hideWindow);
			var actualValue = string.Empty;
			int? exitCode = null;
			var expectedValue = "myname";
			var command = $"echo {expectedValue}";

			cmd.OutputDataReceived += delegate (object sender, DataReceivedEventArgs args)
			{
				if (args.Data is not null)
					actualValue = args.Data;
			};

			// Act
			var exception = Record.Exception(() =>
			{
				var process = cmd.RunCommand(command);
				process.WaitForExit();
				exitCode = process.ExitCode;
			});


			//Assert
			Assert.Null(exception);
			Assert.Equal(0, exitCode);
			Assert.Equal(expectedValue, actualValue);
		}


		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void RunCommandAndWaitForExit_WithEchoAsCommand_ShouldEcho(bool hideWindow)
		{
			// Arrange
			var cmd = new CommandPrompt(hideWindow);
			var actualValue = string.Empty;
			int? exitCode = null;
			var expectedValue = "myname";
			var command = $"echo {expectedValue}";

			cmd.OutputDataReceived += delegate (object sender, DataReceivedEventArgs args)
			{
				if (args.Data is not null)
					actualValue = args.Data;
			};

			// Act
			var exception = Record.Exception(() =>
			{
				(Process process, var didProcessExit) = cmd.RunCommandAndWaitForExit(command);
				exitCode = process.ExitCode;
			});


			//Assert
			Assert.Null(exception);
			Assert.Equal(0, exitCode);
			Assert.Equal(expectedValue, actualValue);
		}


		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void RunCommandAndWaitForExit_ShouldExitEarly_WhenTimeoutIsSet(bool hideWindow)
		{
			// Arrange
			var cmd = new CommandPrompt(hideWindow);
			var actualValue = string.Empty;
			int? exitCode = null;
			var expectedValue = "myname";
			var command = $"echo {expectedValue}";

			cmd.OutputDataReceived += delegate (object sender, DataReceivedEventArgs args)
			{
				if (args.Data is not null)
					actualValue = args.Data;
			};

			// Act
			var exception = Record.Exception(() =>
			{
				(Process process, var didProcessExit) = cmd.RunCommandAndWaitForExit(command, timeout: TimeSpan.FromMilliseconds(1));
				exitCode = process.ExitCode;
			});


			//Assert
			Assert.NotNull(exception);
			Assert.Equal(null, exitCode);
			Assert.Equal(string.Empty, actualValue);
		}

	}


}