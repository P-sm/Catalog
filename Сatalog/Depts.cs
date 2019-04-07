using System;
using System.Collections.Generic;

namespace Сatalog
{
	// README Данный класс не используеться, см. Models/Dept.cs
    public partial class Depts
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
    }

    public class DeptsListModel
    {
        public int? Seed { get; set; }
        public IEnumerable<Depts> News { get; set; }
    }
}
