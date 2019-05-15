using System;
using System.Diagnostics;
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
        private string CmdLocation { get; }
        public bool CreateNoWindow { get; set; } = false;
        public bool UseShellExecute { get; set; } = false;
        public bool RedirectStandardError { get; set; }
        public bool RedirectStandardOutput { get; set; }
        public CommandPrompt(string cmdLocation = @"C:\windows\system32\cmd.exe", bool hideWindow = true)
        {
            CmdLocation = cmdLocation;
            CreateNoWindow = hideWindow;
        }

        public CommandPrompt(string runAsUser, string password, string cmdLocation = @"C:\windows\system32\cmd.exe", bool hideWindow = true)
        {
            RunAsUser = runAsUser;
            Password = CreateSecurePassword(password);
            CmdLocation = cmdLocation;
            CreateNoWindow = hideWindow;
        }

        public CommandPrompt(string runAsUser, string cmdLocation = @"C:\windows\system32\cmd.exe", bool hideWindow = true)
        {
            RunAsUser = runAsUser; ;
            CmdLocation = cmdLocation;
            CreateNoWindow = hideWindow;
        }

        public void UpdateDefaultUserAndPassword(string runAsUser, string password)
        {
            RunAsUser = runAsUser;
            Password = CreateSecurePassword(password);
        }
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
        /// <param name="command">command to run</param>
        /// <param name="workingDirectory">directory to run command from </param>
        /// <param name="hideWindow">if true will show cmd will show during execution of the command </param>
        /// <param name="outputDataReceived">event handler for responses return during the execution of the command</param>
        /// <param name="errorDataReceived">event handler for error responses return during the execution of the command</param>
        /// <returns></returns>

        public ProcessStartInfo CreateStartInfo(string command, string workingDirectory,bool hideWindow = true, DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null)
        {
            var info = new ProcessStartInfo(CmdLocation, "/c " + command)
                {
                    CreateNoWindow = hideWindow,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardError = errorDataReceived != null,
                    RedirectStandardOutput = outputDataReceived != null,
                    Password = Password,
                    UserName = RunAsUser
                };
            return info;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="outputDataReceived">event handler for responses return during the execution of the command</param>
        /// <param name="errorDataReceived">event handler for error responses return during the execution of the command</param>
        /// <param name="exited"></param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(string command, DataReceivedEventHandler outputDataReceived , DataReceivedEventHandler errorDataReceived , EventHandler exited )
        {
            return RunCommand(command, null, outputDataReceived, errorDataReceived, exited);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="workingDirectory"></param>
        /// <param name="timeoutInMilliseconds"></param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(string command, string workingDirectory,int timeoutInMilliseconds)
        {
            return RunCommand(command, workingDirectory,null,null,null,timeoutInMilliseconds);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="workingDirectory"></param>
        /// <param name="timeoutInMilliseconds"></param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(string command, string workingDirectory, EventHandler exited, int timeoutInMilliseconds = int.MaxValue)
        {
            return RunCommand(command, workingDirectory, null, null, exited, timeoutInMilliseconds);
        }

        public int? RunCommand(string command, string workingDirectory)
        {
            return RunCommand(command, workingDirectory, null, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="workingDirectory"></param>
        /// <param name="outputDataReceived">event handler for responses return during the execution of the command</param>
        /// <param name="errorDataReceived">event handler for error responses return during the execution of the command</param>
        /// <param name="exited"></param>
        /// <param name="timeoutInMilliseconds"></param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(string command, string workingDirectory, DataReceivedEventHandler outputDataReceived , DataReceivedEventHandler errorDataReceived, System.EventHandler exited , int timeoutInMilliseconds = int.MaxValue)
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


            process?.WaitForExit(timeoutInMilliseconds);
            var exitcode = process?.ExitCode;
            process?.Close();
            return exitcode;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="info"></param>
        /// <param name="outputDataReceived">event handler for responses return during the execution of the command</param>
        /// <param name="errorDataReceived">event handler for error responses return during the execution of the command</param>
        /// <param name="exited"></param>
        /// <param name="timeoutInMilliseconds"></param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(ProcessStartInfo info ,DataReceivedEventHandler outputDataReceived, DataReceivedEventHandler errorDataReceived  ,EventHandler exited , int timeoutInMilliseconds = int.MaxValue)
        {

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
            // var err = process?.StandardError.ReadToEnd();
            // var msg = process?.StandardOutput.ReadToEnd();


            process?.WaitForExit(timeoutInMilliseconds);
            var exitcode = process?.ExitCode;
            process?.Close();
            return exitcode;
        }

        public void Dispose()
        {
            
        }
    }
}