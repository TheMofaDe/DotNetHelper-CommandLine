using System;
using System.Diagnostics;
using DotNetHelper_CommandLine;

namespace ConsoleSample
{
	class Program
	{
		static void Main(string[] args)
		{
			var cmd = new CommandPrompt() { };
			cmd.OutputDataReceived += OnDataReceived;
			cmd.Exited += Exited;
			cmd.ErrorDataReceived += ErrorDataReceived;

			var process = cmd.RunCommand("ping www.google.com");
			// Or if you need wait until the process 
			var processButExited = cmd.RunCommandAndWaitForExit("ping www.youtube.com","./",TimeSpan.FromMilliseconds(100));
			Console.ReadKey();
		}


		private static void Exited(object sender, EventArgs e)
		{
			Console.WriteLine("command has exited.");
		}

		private static void ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			Console.WriteLine("error : " + e.Data);
		}

		private static void OnDataReceived(object sender, DataReceivedEventArgs args)
		{
			Console.WriteLine("received data : " + args.Data);
		}

	}
}