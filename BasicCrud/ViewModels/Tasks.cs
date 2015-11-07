using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BasicCrud.Models;

namespace BasicCrud.ViewModels
{
    public class TasksIndex
    {
        public IEnumerable<Task> Tasks { get; set; }
        public int? ListId { get; set; }
        public string ListName { get; set; }
        public bool Exist { get; set; }
    }

    public class TasksDetails
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Starts")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Ends")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FinishDate { get; set; }

        public int Quantity { get; set; }
        public string Details { get; set; }
        public int? ListId { get; set; }
        public string ListType { get; set; }
    }

    public class TasksCreate
    {
        [Required, MaxLength(128)]
        public string Name { get; set; }

        [Display(Name = "Starts")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Ends")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FinishDate { get; set; }

        public int Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        public string Details { get; set; }

        public int? ListId { get; set; }
        public string ListName { get; set; }
        public string ListType { get; set; }
    }

    public class TasksEdit
    {
        public int Id { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }

        [Display(Name = "Starts")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Ends")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FinishDate { get; set; }

        public int Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        public string Details { get; set; }

        public int? ListId { get; set; }
        public string ListType { get; set; }
    }
}