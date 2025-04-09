using BoatRecords.Commands;

namespace BoatRecords.ContentViews.DoublePicker.Commands;

internal class ChangeCategory : BaseCommand
{
    private readonly DoublePickerViewModel _doublePickerViewModel;

    public ChangeCategory(DoublePickerViewModel doublePickerViewModel)
    {
        _doublePickerViewModel = doublePickerViewModel;
    }

    public override void Execute(object? parameter)
    {
        string SelectedCategory = _doublePickerViewModel.SelectedCategory;
        _doublePickerViewModel.SubCategories.Clear();

        if (SelectedCategory == null)
        {
            return;
        }

        foreach (var category in _doublePickerViewModel.CategorisedItems.Keys)
        {
            if (category != SelectedCategory)
            {
                continue;
            }

            foreach (var subCategory in _doublePickerViewModel.CategorisedItems[category])
            {
                _doublePickerViewModel.SubCategories.Add(subCategory);
            }
        }
    }
}
