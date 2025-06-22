using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces;
using TLGames.Infrastructure.Configuration;
using TLGames.Infrastructure.Data;
using TLGames.Infrastructure.Persistence;
using Wpf.Ui.Controls;

namespace TLGames
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
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

        public static IDbConnectionFactory ConnectionFactory { get; private set; }
        public static IServiceProvider SystemServices { get; private set; }
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SystemServices = InfrastructureServicesConfiguration.ConfigureServices();
            SnakeCaseMapperInitializer.RegisterAllEntities();
            try
            {
                ConnectionFactory = SystemServices.GetRequiredService<IDbConnectionFactory>();
                using IDbConnection connection = ConnectionFactory.CreateConnection();
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when connect to db! {ex.Message}");
                Shutdown();
            }
            List<CartModel> carts = await (new CartDAO(ConnectionFactory)).GetAllAsync();
            //MainWindow mainWindow = new();
            //mainWindow.Show();
#if DEBUG
            AllocConsole(); // Gán console cho tiến trình hiện tại
            Console.OutputEncoding = Encoding.UTF8; // Đặt mã hóa UTF-8
            Console.InputEncoding = Encoding.UTF8;
            foreach (CartModel cart in carts)
                Console.WriteLine(cart.TotalPrice);
#endif
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
