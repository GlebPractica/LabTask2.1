using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public class Discipline
    {
        public string Name { get; set; }
        public int Semester { get; set; }
        public int Course { get; set; }
        public string Speciality { get; set; }
        public int LectureCount { get; set; }
        public int LabCount { get; set; }
        public string ControlType { get; set; }
        public Lecturer Lecture { get; set; }
    }
}
