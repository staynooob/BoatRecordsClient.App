using BoatRecords.Models.Entities;

namespace BoatRecords.ContentViews.DoublePicker.EventArgsuments;

internal class SubCategoryEventArguments : EventArgs
{
    public string Category { get; set; }
    public ICategorisableEntity CategoryEntity { get; set; }

    public SubCategoryEventArguments(string category, ICategorisableEntity entity)
    {
        Category = category;
        CategoryEntity = entity;
    }
}
