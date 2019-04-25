using System;
using System.Diagnostics;

namespace DotNetHelper_CommandLine
{
    /// <summary>
    /// A command-line helper class that makes it easy to run commands.
    /// </summary>
    public class CommandPrompt
    {
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
        public int? RunCommand(string command, DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null, System.EventHandler exited = null)
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
        public int? RunCommand(string command, string workingDirectory,int timeoutInMilliseconds = int.MaxValue)
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
        public int? RunCommand(string command, string workingDirectory, System.EventHandler exited, int timeoutInMilliseconds = int.MaxValue)
        {
            return RunCommand(command, workingDirectory, null, null, exited, timeoutInMilliseconds);
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
        public int? RunCommand(string command, string workingDirectory, DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null, System.EventHandler exited = null, int timeoutInMilliseconds = int.MaxValue)
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
        public int? RunCommand(ProcessStartInfo info ,DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null, System.EventHandler exited = null, int timeoutInMilliseconds = int.MaxValue)
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

            var err = process?.StandardError.ReadToEnd();
            var msg = process?.StandardOutput.ReadToEnd();


            process?.WaitForExit(timeoutInMilliseconds);
            var exitcode = process?.ExitCode;
            process?.Close();
            return exitcode;
        }

    }
}