using BoatRecords.Commands;

namespace BoatRecords.ContentViews.DoublePicker.Commands;

internal class ChangeSubCategory : BaseCommand
{
    private readonly DoublePickerViewModel _doublePickerViewModel;

    public ChangeSubCategory(DoublePickerViewModel doublePickerViewModel)
    {
        _doublePickerViewModel = doublePickerViewModel;
    }

    public override void Execute(object? parameter)
    {
        string SelectedCategory = _doublePickerViewModel.SelectedCategory;
        _doublePickerViewModel.OnSubCategorySelected();
    }
}
