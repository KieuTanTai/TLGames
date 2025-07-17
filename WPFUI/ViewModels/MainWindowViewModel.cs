using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TLGames.Core.Entities;
using TLGames.Core.Interfaces.IServices.ICategory;

namespace TLGames.WPFUI.ViewModels
{
    //TODO: CONVERT IT TO DI ( ADD IT TO SERVICE PROVIDER, ADD SOME INTERFACE FOR APPLICATION SERVICE! )
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ICategoryManagementService<CategoryModel> _categoryManagementService;
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

        //public MainWindowViewModel()
        //{
        //    Categories = new ObservableCollection<CategoryModel>();
        //}

        public MainWindowViewModel(ICategoryManagementService<CategoryModel> categoryManagementService)
        {
            Categories = new ObservableCollection<CategoryModel>();
            _categoryManagementService = categoryManagementService;
        }

        public async Task LoadCategoriesAsync()
        {
            try
            {
                Console.WriteLine("Attempting to load categories...");
                //CategoryManagementService categoryService = GetProviderService.SystemServices.GetRequiredService<CategoryManagementService>();
                Console.WriteLine("TEMP : " + _categoryManagementService);
                List<CategoryModel> categoryList = await _categoryManagementService.GetAllCategoriesAsync();
                Categories.Clear();
                foreach (var category in categoryList)
                    Categories.Add(category);
                Console.WriteLine($"Categories count after load in ViewModel: {Categories.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading categories: {ex.Message}");
            }
        }
    }
}
