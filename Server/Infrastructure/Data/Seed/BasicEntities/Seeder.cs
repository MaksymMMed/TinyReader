using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Seed.BasicEntities
{
    public abstract class Seeder<T> where T : class
    {
        public List<T> Entities { get; set; } = new List<T>();
        public virtual void GenerateData(EntityTypeBuilder<T> model)
        {
            model.HasData(Entities);
        }
    }
}
