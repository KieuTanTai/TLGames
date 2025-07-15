using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TLGames.Application.Services;
using TLGames.Application.Services.Category;
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
            LoadCategoriesAsync();
        }

        public async void LoadCategoriesAsync()
        {
            List<CategoryModel> categoryList = await new GetCategoryService(GetConnectionFactoryService.ConnectionFactory).GetTags();

            Categories = new ObservableCollection<CategoryModel>(categoryList);

            Console.WriteLine($"Categories count after load in ViewModel: {Categories.Count}");
        }
    }
}
