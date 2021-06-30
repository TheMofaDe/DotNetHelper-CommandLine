using System;
using System.Diagnostics;
#if NETSTANDARD2_0 || NET5_0_OR_GREATER
using System.Runtime.InteropServices;
#endif
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetHelper_CommandLine
{
	/// <summary>
	/// A command-line helper class that makes it easy to run commands.
	/// </summary>
	public class CommandPrompt : IDisposable
	{
		public string RunAsUser { get; private set; }
		private SecureString Password { get; set; }
		/// <summary>
		/// Occurs when the process exits
		/// </summary>
		public event EventHandler Exited;
		/// <summary>
		/// event handler for responses return during the execution of the command
		/// </summary>
		public event DataReceivedEventHandler OutputDataReceived;
		/// <summary>
		/// event handler for error responses return during the execution of the command
		/// </summary>
		public event DataReceivedEventHandler ErrorDataReceived;

		private string CmdLocation
		{
			get
			{
#if NETSTANDARD || NET5_0_OR_GREATER
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return @"/bin/bash";
#endif
				return @"C:\windows\system32\cmd.exe";
			}
		}

		private string Argument
		{
			get
			{
#if NETSTANDARD || NET5_0_OR_GREATER
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return @"-c ";
#endif
				return @"/c ";
			}
		}

		public bool CreateNoWindow { get; set; } = false;


		public CommandPrompt()
		{
			CreateNoWindow = false;
		}

		public CommandPrompt(bool hideWindow)
		{
			CreateNoWindow = hideWindow;
		}

		public CommandPrompt(string runAsUser, string password, bool hideWindow)
		{
			RunAsUser = runAsUser;
			Password = CreateSecurePassword(password);
			CreateNoWindow = hideWindow;
		}


		/// <summary>
		/// Updates the user & password to run commands with
		/// </summary>
		/// <param name="runAsUser"></param>
		/// <param name="password"></param>
		public void UpdateDefaultUserAndPassword(string runAsUser, string password)
		{
			RunAsUser = runAsUser;
			Password = CreateSecurePassword(password);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public SecureString CreateSecurePassword(string password)
		{
			if (password == null)
				return null;
			var securePassword = new SecureString();
			foreach (var c in password.ToCharArray())
			{
				securePassword.AppendChar(c);
			}
			securePassword.MakeReadOnly();
			return securePassword;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="command">the command to run</param>
		/// <param name="workingDirectory">sets the working directory for the command to be run</param>
		/// <returns>a new instance of ProcessStartInfo </returns>
		public ProcessStartInfo CreateStartInfo(string command, string workingDirectory = "./", bool hideWindow = true)
		{
			var argument =  Argument + command;
#if NETSTANDARD || NET5_0_OR_GREATER			
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var escapedArgs = command.Replace("\"", "\\\"");
				argument = $"{Argument} \"{escapedArgs}\"";
            }
#endif

			var info = new ProcessStartInfo(CmdLocation,argument)
			{
				CreateNoWindow = hideWindow,
				UseShellExecute = false,
				WorkingDirectory = workingDirectory, // defaults  $@"./"
				RedirectStandardError = ErrorDataReceived != null,
				RedirectStandardOutput = OutputDataReceived != null,
				UserName = RunAsUser,
				Verb = (string.IsNullOrEmpty(RunAsUser) ? string.Empty : "runas"),
			};
#if NETSTANDARD || NET5_0_OR_GREATER			
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                info.Password = Password;
            }
#else
			 info.Password = Password; // .NET FRAMEWORK HAS TO BE WINDOWS
#endif
			return info;
		}


		/// <summary>
		/// Starts a new instance of a command terminal and runs the specified command
		/// </summary>
		/// <param name="command">the command to run</param>
		/// <param name="workingDirectory">sets the working directory for the command to be run</param>
		/// <returns>the process </returns>
		public Process RunCommand(string command, string workingDirectory = "./")
		{
			var info = CreateStartInfo(command, workingDirectory, CreateNoWindow);
			var process = Process.Start(info);

			if (OutputDataReceived != null)
			{
				if (process != null)
				{
					process.OutputDataReceived += OutputDataReceived;
					process.BeginOutputReadLine();
				}
			}
			if (ErrorDataReceived != null)
			{
				if (process != null)
				{
					process.ErrorDataReceived += ErrorDataReceived;
					process.BeginErrorReadLine();
				}
			}
			if (Exited != null)
			{
				if (process != null)
				{
					process.Exited += Exited;
				}
			}

			return process;
		}

		/// <summary>
		/// Starts a new instance of a command terminal and runs the specified command and wait for it to exit
		/// </summary>
		/// <param name="command">the command to run</param>
		/// <param name="workingDirectory">sets the working directory for the command to be run</param>
		/// <returns>the associated process  and whether or not the process exited</returns>
		public (Process process, bool? didProcessExit) RunCommandAndWaitForExit(string command, string workingDirectory = "./", TimeSpan? timeout = null)
		{
			var info = CreateStartInfo(command, workingDirectory, CreateNoWindow);
			var process = Process.Start(info);

			if (OutputDataReceived != null)
			{
				if (process != null)
				{
					process.OutputDataReceived += OutputDataReceived;
					process.BeginOutputReadLine();
				}
			}
			if (ErrorDataReceived != null)
			{
				if (process != null)
				{
					process.ErrorDataReceived += ErrorDataReceived;
					process.BeginErrorReadLine();
				}
			}
			if (Exited != null)
			{
				if (process != null)
				{
					process.Exited += Exited;
				}
			}

			if (timeout is null)
			{
				process.WaitForExit();
				return (process, true);
			}
			else
			{
				var didProcessExit = process?.WaitForExit(int.Parse(timeout.Value.TotalMilliseconds.ToString()));
				return (process, didProcessExit);
			}


		}

#if NET5_0_OR_GREATER
		/// <summary>
		/// Starts a new instance of a command terminal and runs the specified command
		/// </summary>
		/// <param name="command">the command to run</param>
		/// <param name="workingDirectory">sets the working directory for the command to be run</param>
		/// <param name="cancellationToken">cancellation token for how long the process should wait to be exited</param>
		/// <returns>the process </returns>
		public async Task<Process> RunCommandAndWaitForExitAsync(string command, string workingDirectory = "./",
			CancellationToken cancellationToken = default)
		{
			var info = CreateStartInfo(command, workingDirectory, CreateNoWindow);
			var process = Process.Start(info);
			if (OutputDataReceived != null)
			{
				if (process != null)
				{
					process.OutputDataReceived += OutputDataReceived;
					process.BeginOutputReadLine();
				}
			}

			if (ErrorDataReceived != null)
			{
				if (process != null)
				{
					process.ErrorDataReceived += ErrorDataReceived;
					process.BeginErrorReadLine();
				}
			}

			if (Exited != null)
			{
				if (process != null)
				{
					process.Exited += Exited;
				}
			}

			if (process != null)
				 await process?.WaitForExitAsync(cancellationToken);
			return (process);
		}
#endif

		public void Dispose()
		{
			RunAsUser = null;
			Password?.Clear();
			ErrorDataReceived = null;
			OutputDataReceived = null;
			Exited = null;
		}
	}
}