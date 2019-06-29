using System;
using System.Windows.Forms;
using EzTrace.DemoApp.Helpers;

namespace EzTrace.DemoApp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ApplicationExit += Application_ApplicationExit;
            JaegerFactory.Init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            AsyncFormEventBus.ShouldRaise = () => false;
            var recorder = JaegerFactory.GetMySpanRecorder();
            recorder.ShouldRecording = () => false;
            recorder.Flush();
            //MessageBox.Show(@"ApplicationExit!");
        }
    }
}
