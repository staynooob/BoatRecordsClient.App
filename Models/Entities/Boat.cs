using BoatRecords.Models.Enums;

namespace BoatRecords.Models.Entities;

internal class Boat : ICategorisableEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int SeatsNumber { get; set; }
    public bool IsCoxed { get; set; }
    public bool Paired { get; set; }

    public override string ToString()
    {
        return Name ?? GetCategoryName();
    }

    public string GetCategoryName()
    {
        return SeatsNumber
            + (Paired ? "x" : "")
            + (IsCoxed ? "+" : (SeatsNumber == 1 || (Paired && SeatsNumber == 2) ? "" : "-"));
    }
}
