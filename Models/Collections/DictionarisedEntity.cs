using BoatRecords.Models.Entities;

namespace BoatRecords.Models.Collections;

public class DictionarisedEntity
{
    public Dictionary<String, List<ICategorisableEntity>> CategorisedDictionary { get; }

    public DictionarisedEntity(IEnumerable<ICategorisableEntity> entities)
    {
        CategorisedDictionary = new Dictionary<String, List<ICategorisableEntity>>();

        foreach (var entity in entities)
        {
            var key = entity.GetCategoryName();

            if (!CategorisedDictionary.ContainsKey(key))
            {
                CategorisedDictionary[key] = new List<ICategorisableEntity>();
            }

            CategorisedDictionary[key].Add(entity);
        }
    }
}
