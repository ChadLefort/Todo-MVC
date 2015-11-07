using System;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace BasicCrud.Models
{
    public class Task
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? FinishDate { get; set; }
        public virtual bool IsFinished { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string Details { get; set; }
        public virtual int TaskOrder { get; set; }
        public virtual List List { get; set; }
    }

    public class TaskMap : ClassMapping<Task>
    {
        public TaskMap()
        {
            Table("tasks");

            Id(x => x.Id, x => x.Generator(Generators.Identity));
            Property(x => x.Name, x => x.NotNullable(true));
            Property(x => x.StartDate, x => x.Column("start_date"));
            Property(x => x.FinishDate, x => x.Column("finish_date"));
            Property(x => x.IsFinished, x => x.Column("is_finished"));
            Property(x => x.Quantity);
            Property(x => x.Details);
            Property(x => x.TaskOrder, x =>
            {
                x.Column("task_order");
                x.NotNullable(true);
            });
            ManyToOne(x => x.List, x =>
            {
                x.Column("list_id");
                x.NotNullable(true);
            });
        }
    }
}