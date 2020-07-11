using System.Collections.Generic;

namespace eduProjectModel.Domain
{
    public class Faculty : IValueObject
    {
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
            StudyPrograms = new HashSet<StudyProgram>(); // TODO: provjera da ima bar jedan studijski program
        }

        public void AddStudyProgram(StudyProgram program)
        {
            StudyPrograms.Add(program);
        }
        // ograniciti programe na 5 ?
    }
}
