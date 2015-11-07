using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BasicCrud.Infrastructure;
using BasicCrud.Models;

namespace BasicCrud.ViewModels
{
    public class ListsIndex
    {
        public PagedData<List> Lists { get; set; } 
        public bool Exist { get; set; }
    }

    public class ListsDetails
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Type of List")]
        public string Type { get; set; }
    }

    public class ListsCreate
    {
        [Required, MaxLength(128)]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
    }

    public class ListsEdit
    {
        public int? Id { get; set; }
        [Required, MaxLength(128)]
        public string Name { get; set; }
    }
}