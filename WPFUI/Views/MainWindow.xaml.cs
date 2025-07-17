using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TLGames.WPFUI.ViewModels;
using Wpf.Ui.Controls;


namespace TLGames
{
    /// <summary>  
    /// Interaction logic for MainWindow.xaml  
    /// </summary>  
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _ = _viewModel.LoadCategoriesAsync();
            DataContext = _viewModel;
            SetDefaultForm();
            AddEventForButtons();
        }

        // NOTE: FOR INIT  
        public void SetDefaultForm()
        {
            SPHeaderMenu.Height = GHeaderForm.Height;
            SPControlForm.Height = GHeaderForm.Height;
            ResizeFormFunctionImages([ImgExitFormButton, ImgMinimizeButton, ImgFullScreenButton], 20, 20);
            SetPaddingForCards([CExitFormContainer, CMinimizeContainer, CToggleFullScreenContainer], 20);
            SetBorderRadiusForBorderContainer([BrHeaderSearchContainer, BrHeaderSearchButtonContainer], 5);
        }

        public static void SetBorderRadiusForBorderContainer(List<Border> borders, int borderRadius)
        {
            foreach (Border border in borders)
                border.CornerRadius = new CornerRadius(borderRadius);
        }

        public static void SetPaddingForCards(List<Card> cards, int padding)
        {
            foreach (Card card in cards)
            {
                card.Padding = new Thickness(padding);
                Debug.WriteLine(card.Height);
                Debug.WriteLine(card.BorderThickness);
                Debug.WriteLine(card.ActualHeight);
                Debug.WriteLine(card.Padding);

            }
        }

        public void ResizeFormFunctionImages(List<System.Windows.Controls.Image> images, int width, int height)
        {
            foreach (System.Windows.Controls.Image image in images)
            {
                image.Height = height;
                image.Width = width;
            }
        }
        // NOTE: FOR LOGICS  

        //NOTE: FOR VALIDATE  

        // NOTE: FOR EVENTS  
        public void AddEventForButtons()
        {
            CExitFormContainer.MouseLeftButtonDown += CExitFormContainer_MouseLeftButtonDown;
            CMinimizeContainer.MouseLeftButtonDown += CMinimizeContainer_MouseLeftButtonDown;
            CToggleFullScreenContainer.MouseLeftButtonDown += CToggleFullScreenContainer_MouseLeftButtonDown;
            this.MouseDown += Main_MouseDown;
        }

        public void CExitFormContainer_MouseLeftButtonDown(object sender, EventArgs args)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void CMinimizeContainer_MouseLeftButtonDown(object sender, EventArgs args)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void CToggleFullScreenContainer_MouseLeftButtonDown(object sender, EventArgs args)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                GHeaderForm.Margin = new Thickness(0, 0, 0, 0);
                GMainContainer.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
                GHeaderForm.Margin = new Thickness(0, 10, 10, 0);
                GMainContainer.Margin = new Thickness(10);
            }
        }

        private void Main_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
