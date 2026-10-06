using System;

namespace Bai5_4_TreeView_ListView
{
    public class Employee
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public string Department { get; set; }
        public string Group { get; set; }

        public Employee(string id, string fullName, string position, DateTime startDate, string department, string group)
        {
            Id = id;
            FullName = fullName;
            Position = position;
            StartDate = startDate;
            Department = department;
            Group = group;
        }
    }
}
