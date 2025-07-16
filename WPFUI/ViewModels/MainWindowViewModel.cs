using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TLGames.Application.Services.Category;
using TLGames.Application.Services.Utils;
using TLGames.Core.Entities;

namespace TLGames.WPFUI.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private ObservableCollection<CategoryModel> _categories;

        public ObservableCollection<CategoryModel> Categories
        {
            get => _categories;
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged(nameof(Categories));
                }
            }
        }

        public MainWindowViewModel()
        {
            Categories = new ObservableCollection<CategoryModel>();
            //LoadCategoriesAsync();
        }

        public async Task LoadCategoriesAsync()
        {
            try
            {
                Console.WriteLine("Attempting to load categories...");
                // **Sử dụng Service Locator để lấy CategoryManagementService**
                // Đây là cách bạn sẽ lấy service nếu không dùng DI trong constructor của ViewModel
                var categoryService = GetProviderService.SystemServices.GetRequiredService<CategoryManagementService>();

                List<CategoryModel> categoryList = (await categoryService.GetAllCategoriesAsync()).ToList();

                // Cập nhật ObservableCollection bằng cách Clear và Add từng phần tử
                // Đây là cách đúng để làm việc với Data Binding trên ObservableCollection
                Categories.Clear();
                foreach (var category in categoryList)
                {
                    Categories.Add(category);
                }

                Console.WriteLine($"Categories count after load in ViewModel: {Categories.Count}");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi: Log lỗi, hiển thị thông báo cho người dùng
                Console.WriteLine($"Error loading categories: {ex.Message}");
                // Ví dụ: MessageBox.Show($"Failed to load categories: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false; // Tắt trạng thái bận
            }
        }
    }
}
