using System;

namespace DotNetHelper_CommandLine
{
	/// <summary>
	/// Helper class for common commands  
	/// </summary>
	public static class UnixCommands
	{
		/// <summary>
		/// Copy one directory to another 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static string CopyDirectory(string from, string to)
		{
			if (from.Contains(" "))
			{
				from = $"\"{from}\"";
			}
			if (to.Contains(" "))
			{
				to = $"\"{to}\"";
			}
			return $"xcopy {from} {to}";
		}


		/// <summary>
		/// returns command for tailing the specified files
		/// </summary>
		/// <param name="fullFilePath"></param>
		/// <param name="outputLastLine"></param>
		/// <returns></returns>
		public static string TailFile(string fullFilePath, int outputLastLine = 10)
		{
			var cmd = $"tail -f {fullFilePath} -n {outputLastLine}";
			return cmd;
		}


		/// <summary>
		/// return command for getting system info
		/// </summary>
		/// <returns></returns>
		public static string GetSystemInfo()
		{
			return "systeminfo";
		}

		/// <summary>
		/// return the command to get the Operating System version
		/// </summary>
		/// <returns></returns>
		public static string GetOsVersion()
		{
			return "ver";
		}

		/// <summary>
		/// returns the command get the task list
		/// </summary>
		/// <returns></returns>
		public static string GetTaskList()
		{
			return "tasklist";
		}

		/// <summary>
		/// returns the command to get ping the specified host
		/// </summary>
		/// <param name="host"></param>
		/// <returns></returns>
		public static string Ping(string host)
		{
			return $"ping {host}";
		}

		/// <summary>
		/// returns the command to create the specified directory 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string CreateDirectory(string path)
		{
			return $"mkdir {path}";
		}

		/// <summary>
		/// returns the commands for moving the specified file
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static string MoveFile(string from, string to)
		{
			if (from.Contains(" "))
			{
				from = $"\"{from}\"";
			}
			if (to.Contains(" "))
			{
				to = $"\"{to}\"";
			}
			return $"move {from} {to}";
		}


		/// <summary>
		/// Sets ownership on files and directories
		/// </summary>
		/// <param name="fileOrPath"></param>
		/// <returns></returns>
		public static string GetAttribute(string fileOrPath)
		{
			if (fileOrPath.Contains(" "))
			{
				fileOrPath = $"\"{fileOrPath}\"";
			}

			return $"attrib {fileOrPath} ";
		}


		/// <summary>
		/// returns the command for opening the services
		/// </summary>
		/// <returns></returns>
		public static string OpenServices()
		{
			return $"services.msc";
		}

		/// <summary>
		/// returns the command for opening the control panel
		/// </summary>
		/// <returns></returns>
		public static string OpenControlPanel()
		{
			return $"control";
		}

		/// <summary>
		/// returns the command for shutting down the host
		/// </summary>
		/// <param name="force"></param>
		/// <returns></returns>
		public static string Shutdown(bool force)
		{
			var shutdown = "shutdown -s";
			if (force)
				shutdown = $"{shutdown} -f";
			return shutdown;
		}

		/// <summary>
		/// returns the command for rebooting the host
		/// </summary>
		/// <param name="force"></param>
		/// <param name="delay"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public static string Reboot(bool force, TimeSpan? delay = null, string? message = null)
		{
			var shutdown = "shutdown -r";
			if (force)
				shutdown = $"{shutdown} -f";
			if (delay != null)
			{
				shutdown = $"{shutdown} -t {delay.GetValueOrDefault().TotalSeconds}";
			}
			if (message != null)
			{
				shutdown = $"{shutdown} -c \"{message}\"";
			}
			return shutdown;
		}

		/// <summary>
		/// returns the hcommand for aborting a shutdown or restart
		/// </summary>
		/// <returns></returns>
		public static string AbortShutDownOrReboot()
		{
			var shutdown = "shutdown -a";
			return shutdown;
		}

		/// <summary>
		/// Windows has a y/ n prompt.To get the prompt with Unix, use rm - i.The i means "interactive".
		/// </summary>
		/// <param name="directory"></param>
		/// <returns></returns>
		public static string RemoveDirectory(string directory)
		{
			return $"rmdir /s";
		}



		/// <summary>
		/// Set on Windows prints a list of all environment variables. For individual environment variables, set <variable> is the same as echo $<variable> on Unix.
		/// </summary>
		/// <returns></returns>
		public static string GetEnvironmentVariables()
		{
			return $"set";
		}


		/// <summary>
		/// returns the command for mapping a drive 
		/// </summary>
		/// <param name="driveLetter"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string MapDrive(char driveLetter, string path)
		{
			var cmd = $" net use {driveLetter}: {path}";
			return cmd;
		}

		/// <summary>
		/// returns the command for un-mapping a drive 
		/// </summary>
		/// <param name="driveLetter"></param>
		/// <returns></returns>
		public static string UnMapDrive(char driveLetter)
		{
			var cmd = $"net use {driveLetter}: /Delete /y";
			return cmd;

		}

	}
}