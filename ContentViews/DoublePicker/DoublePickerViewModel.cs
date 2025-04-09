using BoatRecords.ContentViews.DoublePicker.Commands;
using BoatRecords.ContentViews.DoublePicker.EventArgsuments;
using BoatRecords.Models.Collections;
using BoatRecords.Models.Entities;
using BoatRecords.Pages;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BoatRecords.ContentViews;

public class DoublePickerViewModel : BaseViewModel
{
    private Dictionary<string, List<ICategorisableEntity>> _categorisedItems;
    private readonly ObservableCollection<string> _categories = new ObservableCollection<string>();
    private readonly ObservableCollection<ICategorisableEntity> _subCategories = new ObservableCollection<ICategorisableEntity>();
    private string _selectedCategory;
    private string _categoryTitle { get; set; }
    private int _selectedSubCategoryIndex;
    private string _subCategoryTitle { get; set; }

    public Dictionary<string, List<ICategorisableEntity>> CategorisedItems => _categorisedItems;
    public ObservableCollection<string> Categories => _categories;
    public ObservableCollection<ICategorisableEntity> SubCategories => _subCategories;
    public string SelectedCategory
    {
        get { return _selectedCategory; }
        set
        {
            _selectedCategory = value;
            OnPropertyChange(nameof(SelectedCategory));
        }
    }
    public string CategoryTitle
    {
        get { return _categoryTitle; }
        set
        {
            _categoryTitle = value;
            OnPropertyChange(nameof(CategoryTitle));
        }
    }
    public int SelectedSubCategoryIndex
    {
        get { return _selectedSubCategoryIndex; }
        set
        {
            _selectedSubCategoryIndex = value;
            OnPropertyChange(nameof(SelectedSubCategoryIndex));
        }
    }
    public string SubCategoryTitle
    {
        get { return _subCategoryTitle; }
        set
        {
            _subCategoryTitle = value;
            OnPropertyChange(nameof(SubCategoryTitle));
        }
    }

    public ICommand ChangeCategoryCommand { get; }
    public ICommand ChangeSubCategoryCommand { get; }

    public event EventHandler OnSuccess;

    public DoublePickerViewModel()
    {
        ChangeCategoryCommand = new ChangeCategory(this);
        ChangeSubCategoryCommand = new ChangeSubCategory(this);
    }

    public void SetCategorisedItems(DictionarisedEntity categorisedItems)
    {
        _categorisedItems = categorisedItems.CategorisedDictionary;

        foreach (var key in _categorisedItems.Keys)
        {
            _categories.Add(key);
        } 
    }

    public void OnSubCategorySelected()
    {
        if (_selectedSubCategoryIndex == -1) {
            return;
        }

        OnSuccess?.Invoke(this, new SubCategoryEventArguments(_selectedCategory, _subCategories.ElementAt(_selectedSubCategoryIndex)));
    }
}
