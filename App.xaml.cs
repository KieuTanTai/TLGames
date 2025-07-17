using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TLGames.Application.Services;
using TLGames.Infrastructure.Configuration;
using TLGames.Infrastructure.Persistence;
using TLGames.WPFUI.ViewModels;
using Wpf.Ui.Controls;

namespace TLGames
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        private IServiceProvider? _rootServiceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            /* Test GET DATA */
            IServiceCollection services = new ServiceCollection();
            try
            {
                services.AddInfrastructureServices();
                services.AddApplicationServices();
                services.AddViewModelServices();
                _rootServiceProvider = services.BuildServiceProvider();
                GetProviderService.SetServiceProvider(_rootServiceProvider);
                SnakeCaseMapperInitializer.RegisterAllEntities();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Application startup failed: {ex.Message}", "Fatal Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
            //GetProviderService.SetSystemServices();
            //GetConnectionFactoryService.GetConnectionFactory();
#if DEBUG
            AllocConsole(); // Gán console cho tiến trình hiện tại
            Console.OutputEncoding = Encoding.UTF8; // Đặt mã hóa UTF-8
            Console.InputEncoding = Encoding.UTF8;

#endif
            MainWindowViewModel mainWindowViewModel = _rootServiceProvider.GetRequiredService<MainWindowViewModel>();
            MainWindow mainWindow = new MainWindow(mainWindowViewModel);
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Giải phóng console khi ứng dụng đóng (chỉ trong Debug mode)
#if DEBUG
            FreeConsole();
#endif
            base.OnExit(e);
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && !textBox.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                textBox.Focus();
            }
        }

    }
}
