using System;
using System.Collections.Generic;

namespace Сatalog
{
	
	// README Данный класс не используеться, см. Models/Worker.cs
    public partial class Workers
    {
        public int Id { get; set; }
        public int DeptId { get; set; }
        public string FullName { get; set; }
        public int PositionId { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public int GenderType { get; set; }
    }
}
