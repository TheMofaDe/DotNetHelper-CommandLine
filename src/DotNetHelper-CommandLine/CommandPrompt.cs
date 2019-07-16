using System;
using System.Diagnostics;
#if NETSTANDARD
using System.Runtime.InteropServices;
#endif
using System.Security;

namespace DotNetHelper_CommandLine
{
    /// <summary>
    /// A command-line helper class that makes it easy to run commands.
    /// </summary>
    public class CommandPrompt : IDisposable
    {
        public string RunAsUser { get; private set; }
        private SecureString Password { get;  set; }

        public TimeSpan TimeOut { get; set; } = TimeSpan.FromMinutes(20);
        private string CmdLocation
        {
            get
            {
#if NETSTANDARD
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
#if NETSTANDARD
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
            if (password == null) return null;
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
        /// <param name="outputDataReceived">event handler for responses return during the execution of the command</param>
        /// <param name="errorDataReceived">event handler for error responses return during the execution of the command</param>
        /// <returns>a new instance of ProcessStartInfo </returns>
        private ProcessStartInfo CreateStartInfo(string command, string workingDirectory,bool hideWindow = true, DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null)
        {
            var info = new ProcessStartInfo(CmdLocation, Argument + command)
                {
                    CreateNoWindow = hideWindow,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory, // defaults  $@"./"
                    RedirectStandardError = errorDataReceived != null,
                    RedirectStandardOutput = outputDataReceived != null,
                    Password = Password,
                    UserName = RunAsUser,
                    Verb = string.IsNullOrEmpty(RunAsUser) ? null : "runas",               
                };
            return info;
        }

        /// <summary>
        /// Starts a new instance of a command terminal and runs the specified command
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="outputDataReceived">event handler for responses return during the execution of the command</param>
        /// <param name="errorDataReceived">event handler for error responses return during the execution of the command</param>
        /// <param name="exited">Occurs when the process exits</param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(string command, EventHandler exited, DataReceivedEventHandler outputDataReceived , DataReceivedEventHandler errorDataReceived  )
        {
            return RunCommand(command, $@"./", exited, outputDataReceived, errorDataReceived);
        }

        /// <summary>
        /// Starts a new instance of a command terminal and runs the specified command
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="workingDirectory">sets the working directory for the command to be run</param>
        /// <param name="exited">Occurs when the process exits</param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(string command, string workingDirectory, EventHandler exited)
        {
            return RunCommand(command, workingDirectory, exited, null, null);
        }

        /// <summary>
        /// Starts a new instance of a command terminal and runs the specified command
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="workingDirectory">sets the working directory for the command to be run</param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(string command, string workingDirectory)
        {
            return RunCommand(command, workingDirectory, null, null, null);
        }

        /// <summary>
        /// Starts a new instance of a command terminal and runs the specified command
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(string command)
        {
            return RunCommand(command, null, null, null, null);
        }

        /// <summary>
        /// Starts a new instance of a command terminal and runs the specified command
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="workingDirectory">sets the working directory for the command to be run</param>
        /// <param name="outputDataReceived">event handler for responses return during the execution of the command</param>
        /// <param name="errorDataReceived">event handler for error responses return during the execution of the command</param>
        /// <param name="exited">Occurs when the process exits</param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(string command, string workingDirectory, EventHandler exited , DataReceivedEventHandler outputDataReceived , DataReceivedEventHandler errorDataReceived)
        {

            var info = CreateStartInfo(command, workingDirectory,CreateNoWindow,outputDataReceived,errorDataReceived);
            var process = Process.Start(info);

            if (outputDataReceived != null)
            {
                if (process != null)
                {
                    process.OutputDataReceived += outputDataReceived;
                    process.BeginOutputReadLine();
                }
            }

            if (errorDataReceived != null)
            {
                if (process != null)
                {
                    process.ErrorDataReceived += errorDataReceived;
                    process.BeginErrorReadLine();
                }
            }

            if (exited != null)
            {
                if (process != null)
                {
                    process.Exited += exited;
                }
            }

            process?.WaitForExit(int.Parse(TimeOut.TotalMilliseconds.ToString()));
            var exitcode = process?.ExitCode;
            process?.Close();
            return exitcode;
        }




        /// <summary>
        /// Starts a new instance of a command terminal and runs the specified command
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="outputDataReceived">event handler for responses return during the execution of the command</param>
        /// <param name="errorDataReceived">event handler for error responses return during the execution of the command</param>
        /// <param name="exited">Occurs when the process exits</param>
        /// <returns>the process </returns>
        public Process GetProcess(string command, EventHandler exited, DataReceivedEventHandler outputDataReceived, DataReceivedEventHandler errorDataReceived)
        {
            return GetProcess(command, $@"./", exited, outputDataReceived, errorDataReceived);
        }

        /// <summary>
        /// Starts a new instance of a command terminal and runs the specified command
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="workingDirectory">sets the working directory for the command to be run</param>
        /// <param name="exited">Occurs when the process exits</param>
        /// <returns>the process  </returns>
        public Process GetProcess(string command, string workingDirectory, EventHandler exited)
        {
            return GetProcess(command, workingDirectory, exited, null, null);
        }

        /// <summary>
        /// Starts a new instance of a command terminal and runs the specified command
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="workingDirectory">sets the working directory for the command to be run</param>
        /// <returns>the process </returns>
        public Process GetProcess(string command, string workingDirectory)
        {
            return GetProcess(command, workingDirectory, null, null, null);
        }

        /// <summary>
        /// Starts a new instance of a command terminal and runs the specified command
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <returns>the process </returns>
        public Process GetProcess(string command)
        {
            return GetProcess(command, null, null, null, null);
        }

        /// <summary>
        /// Starts a new instance of a command terminal and runs the specified command
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="workingDirectory">sets the working directory for the command to be run</param>
        /// <param name="outputDataReceived">event handler for responses return during the execution of the command</param>
        /// <param name="errorDataReceived">event handler for error responses return during the execution of the command</param>
        /// <param name="exited">Occurs when the process exits</param>
        /// <returns>the process </returns>
        public Process GetProcess(string command, string workingDirectory, EventHandler exited, DataReceivedEventHandler outputDataReceived, DataReceivedEventHandler errorDataReceived)
        {
            var info = CreateStartInfo(command, workingDirectory, CreateNoWindow, outputDataReceived, errorDataReceived);
            var process = Process.Start(info);
            if (outputDataReceived != null)
            {
                if (process != null)
                {
                    process.OutputDataReceived += outputDataReceived;
                    process.BeginOutputReadLine();
                }
            }
            if (errorDataReceived != null)
            {
                if (process != null)
                {
                    process.ErrorDataReceived += errorDataReceived;
                    process.BeginErrorReadLine();
                }
            }
            if (exited != null)
            {
                if (process != null)
                {
                    process.Exited += exited;
                }
            }
            process?.WaitForExit(int.Parse(TimeOut.TotalMilliseconds.ToString()));
            return process;
        }



        public void Dispose()
        {
            RunAsUser = null;
            Password?.Clear();
        }
    }
}