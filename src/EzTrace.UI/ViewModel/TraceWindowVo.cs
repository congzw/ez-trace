using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Windows;
using EzTrace.Common;
using EzTrace.Helpers;
using EzTrace.Jaegers;

namespace EzTrace.UI.ViewModel
{
    public class TraceWindowVo
    {
        public TraceWindowVo(TraceWindow window)
        {
            TheWindow = window;
        }

        public TraceWindow TheWindow { get; set; }
        public JaegerRunner Runner { get; set; }
        public MyTraceHelper DataHelper { get; set; }

        public void InitComponent()
        {
            JaegerEnv.Instance.ClearEnv(EnvironmentVariableTarget.Process);
            TheWindow.BtnStart.Click += BtnStart_Click;
            TheWindow.BtnStop.Click += BtnStop_Click;
            TheWindow.BtnReset.Click += BtnReset_Click;
            TheWindow.BtnLoad.Click += BtnLoad_Click;
        }

        private async void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (DataHelper == null)
            {
                DataHelper = new MyTraceHelper();
            }
            var endPoint = @"http://localhost:14268/api/traces";
            var myTrace = DataHelper.PrepareMyTrace(endPoint);

            if (myTrace.SpanDic == null || myTrace.SpanDic.Count == 0)
            {
                MessageBox.Show("没有发现要导入的数据，请确认路径下是否存在有效日志", "系统消息",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var start = DateTime.Now;

            BtnReset_Click(sender, e);
            
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync(new Uri("http://localhost:16686"));
            var resetSuccess = responseMessage.IsSuccessStatusCode;

            var message = $@"重置执行完毕: {(resetSuccess ? "成功" : "失败")} => 此次花费时间{(DateTime.Now - start).TotalSeconds}s";

            if (!resetSuccess)
            {
                MessageBox.Show(message + Environment.NewLine + "启动Jaeger-all-in-one失败，请确认默认路径下是否缺少文件", 
                    "系统消息",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }
            
            MessageBox.Show(message, 
                "系统消息",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            DataHelper.LoadMyTrace(myTrace);

            //todo nested in cef-sharp
            //var mainWindow = new MainWindow();
            //mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //mainWindow.WindowStyle = WindowStyle.ToolWindow;
            //mainWindow.WindowState = WindowState.Maximized;
            //mainWindow.ShowDialog();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (Runner == null)
            {
                Runner = new JaegerRunner();
            }

            if (Runner.IsRunning())
            {
                MessageBox.Show("Jaeger is running!");
                return;
            }

            //jaeger-all-in-one --collector.zipkin.http-port=9411
            var jaegerExe = "jaeger-all-in-one.exe";
            var args = "--collector.zipkin.http-port=9411";
            var jaegerPath = AppDomain.CurrentDomain.Combine(jaegerExe);
            
            if (!File.Exists(jaegerPath))
            {
                MessageBox.Show($"[{jaegerPath}] not exist!");
                return;
            }

            Runner.Start(jaegerPath, args);
            System.Diagnostics.Process.Start("http://localhost:16686");
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (Runner == null || !Runner.IsRunning())
            {
                MessageBox.Show("Jaeger is not running!");
                return;
            }

            Runner.Stop();
            Console.WriteLine(@"stop jaeger!");
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (Runner == null)
            {
                Runner = new JaegerRunner();
            }

            var isRunning = Runner.IsRunning();
            if (isRunning)
            {
                var messageBoxResult = MessageBox.Show("此操作将重置现有数据，确定执行吗？", "前方高能，请确认 :)", 
                    MessageBoxButton.OKCancel, 
                    MessageBoxImage.Warning);

                if (messageBoxResult != MessageBoxResult.OK)
                {
                    return;
                }
            }

            if (Runner.IsRunning())
            {
                Runner.Stop();
            }

            Thread.Sleep(2000);
            BtnStart_Click(sender, e);
        }
    }
}
