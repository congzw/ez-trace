using System;
using System.Diagnostics;
using System.Linq;

namespace EzTrace.Jaegers
{
    public class JaegerRunner
    {
        public JaegerRunner()
        {
            ProcessName = "jaeger-all-in-one";
        }

        public string ProcessName { get; set; }

        public bool IsRunning()
        {
            var processes = Process.GetProcessesByName(ProcessName);
            return processes.Length > 0;
        }

        public Process TryGetProcess()
        {
            var processes = Process.GetProcessesByName(ProcessName);
            return processes.FirstOrDefault();
        }
        
        public void Start(string exePath, string args)
        {
            var isRunning = IsRunning();
            if (isRunning)
            {
                Console.WriteLine(ProcessName + " is running!");
                return;
            }

            var theProcess = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.Verb = "runas";
            startInfo.UseShellExecute = false;
            startInfo.FileName = exePath;
            startInfo.RedirectStandardInput = true;
            //此代码意味着程序自行结束，不启用窗口，则需要自己使用Kill关闭进程（例如某些关闭命令行退出的场景）
            startInfo.CreateNoWindow = true;
            if (!string.IsNullOrWhiteSpace(args))
            {
                startInfo.Arguments = args;
            }
            theProcess.StartInfo = startInfo;
            theProcess.Start();
        }
        public void Stop()
        {
            var isRunning = IsRunning();
            if (!isRunning)
            {
                Console.WriteLine(ProcessName + " is not running!");
                return;
            }

            var theProcess = TryGetProcess();

            if (theProcess != null)
            {
                var theProcessHasExited = theProcess.HasExited;
                if (!theProcessHasExited)
                {
                    Console.WriteLine("killing");
                    theProcess.Kill();
                    Console.WriteLine("killed");
                }
                theProcess.Dispose();
            }
        }

        public static JaegerRunner Instance = new JaegerRunner();
    }
}
