using System.Data;
using FluentMigrator;

namespace BasicCrud.Migrations
{
    [Migration(1)]
    public class _001_ListsAndTasks : Migration
    {
        public override void Up()
        {
            Create.Table("lists")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128)
                .WithColumn("type").AsString(128);

            Create.Table("tasks")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128)
                .WithColumn("start_date").AsDateTime().Nullable()
                .WithColumn("finish_date").AsDateTime().Nullable()
                .WithColumn("is_finished").AsBoolean().Nullable()
                .WithColumn("quantity").AsInt32().Nullable()
                .WithColumn("details").AsCustom("TEXT").Nullable()
                .WithColumn("task_order").AsInt32()
                .WithColumn("list_id").AsInt32().ForeignKey("lists", "id").OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("tasks");
            Delete.Table("lists");
        }
    }
}