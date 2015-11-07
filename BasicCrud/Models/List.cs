using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace BasicCrud.Models
{
    public class List
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }
    }

    public class ListMap : ClassMapping<List>
    {
        public ListMap()
        {
            Table("lists");

            Id(x => x.Id, x => x.Generator(Generators.Identity));
            Property(x => x.Name, x => x.NotNullable(true));
            Property(x => x.Type, x => x.NotNullable(true));
        }
    }
}