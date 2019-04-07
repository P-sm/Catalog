using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Сatalog.Models
{
    [Table("Depts")]
    public partial class Dept
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите родительский номер отдела")]
        [Display(Name = "Родительский номер отдела")]
        public int? ParentId { get; set; }
        
        
        [Required(ErrorMessage = "Укажите название отдела")]
        [Remote("CheckNameDept", "Home", ErrorMessage = "Данное название компании уже существует")]
        [Display(Name = "Название отдела")]
        public string Name { get; set; }
    }
    
    public class DeptsListModel
    {
        [Display(Name = "Показать данные вложенных узлов")]
        public bool IsShowDataDepthNodes { get; set; }
        // TODO: rename to "ChildId"
        public int? Seed { get; set; }
        public IEnumerable<Dept> Depts { get; set; }
        public IEnumerable<Worker> Workers { get; set; }
    }

    public class DeptsValidEditListModel
    {
        // TODO: rename to "ChildId"
        public Dept DeptEdit { get; set; }
        public IEnumerable<Dept> DeptsValid { get; set; }
        public IEnumerable<SelectListItem> DeptsValidItem { get; set; }

        public int? parentId;
        public string name;
    }

    
}
