using System.Collections.Generic;

namespace eduProjectModel.Domain
{
    public class Faculty
    {
        public int FacultyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<StudyProgram> StudyPrograms { get; set; }

        public Faculty()
        {
            StudyPrograms = new HashSet<StudyProgram>();
        }
        public Faculty(string name, string address)
        {
            Name = name;
            Address = address;
            StudyPrograms = new HashSet<StudyProgram>();
        }

        public void AddStudyProgram(StudyProgram program)
        {
            StudyPrograms.Add(program);
        }
    }
}
