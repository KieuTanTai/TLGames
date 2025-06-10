using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace TLGames
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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
