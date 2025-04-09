using BoatRecords.Models.Enums;

namespace BoatRecords.Models.Entities;

internal class User : ICategorisableEntity
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? NickName { get; set; }
    public required DateOnly BirthDate { get; set; }
    public required Gender Gender { get; set; }

    public override string ToString()
    {
        return NickName == null
            ? Name
            : NickName + "(" + Name + ")";
    }

    public RowingCategory GetRowingCategory()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var age = today.Year - BirthDate.Year;

        return age switch
        {
            _ when age <= 10 => RowingCategory.Eleve,
            _ when age <= 12 => RowingCategory.Younger,
            _ when age <= 14 => RowingCategory.Older,
            _ when age <= 16 => RowingCategory.JuniorB,
            _ when age <= 18 => RowingCategory.JuniorA,
            _ when age <= 22 => RowingCategory.SeniorU23,
            _ => RowingCategory.Senior,
        };
    }

    public string GetCategoryName()
    {
        return GetRowingCategory() switch
        {
            RowingCategory.Eleve => "přípravka",
            RowingCategory.Younger => Gender.Equals(Gender.Female) ? "žkym" : "žcim",
            RowingCategory.Older => Gender.Equals(Gender.Female) ? "žkys" : "žcis",
            RowingCategory.JuniorB => Gender.Equals(Gender.Female) ? "dky" : "dci",
            RowingCategory.JuniorA => Gender.Equals(Gender.Female) ? "jky" : "jři",
            RowingCategory.SeniorU23 => Gender.Equals(Gender.Female) ? "ženy U23" : "muži U23",
            RowingCategory.Senior => Gender.Equals(Gender.Female) ? "ženy" : "muži",
            _ => throw new NotImplementedException(),
        };
    }
}
