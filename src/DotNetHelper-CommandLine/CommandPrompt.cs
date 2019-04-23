using System.Diagnostics;

namespace DotNetHelper_Contracts.Tools
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
        /// <param name="hideWindow">self-explained. If true will show command promopt during execution of thecommand </param>
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
        /// <param name="info"></param>
        /// <param name="outputDataReceived">event handler for responses return during the execution of the command</param>
        /// <param name="errorDataReceived">event handler for error responses return during the execution of the command</param>
        /// <param name="exited"></param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(string command, ProcessStartInfo info = null, DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null, System.EventHandler exited = null)
        {
            return RunCommand(command, null, info, outputDataReceived, errorDataReceived, exited);
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">the command to run</param>
        /// <param name="info"></param>
        /// <param name="outputDataReceived">event handler for responses return during the execution of the command</param>
        /// <param name="errorDataReceived">event handler for error responses return during the execution of the command</param>
        /// <param name="exited"></param>
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(string command, string workingDirectory, ProcessStartInfo info = null, DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null, System.EventHandler exited = null)
        {
            if (info == null)
            {
                info = new ProcessStartInfo(CmdLocation, "/c " + command)
                {
                    CreateNoWindow = CreateNoWindow,
                    UseShellExecute = false,
                    RedirectStandardError = errorDataReceived != null,
                    RedirectStandardOutput = outputDataReceived != null,
                    WorkingDirectory = workingDirectory,
                };
            }

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



            process?.WaitForExit();
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
        /// <returns>the process Exit Code </returns>
        public int? RunCommand(ProcessStartInfo info ,DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null, System.EventHandler exited = null)
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


            process?.WaitForExit();
            var exitcode = process?.ExitCode;
            process?.Close();
            return exitcode;
        }

    }
}