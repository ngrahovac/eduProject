using eduProjectModel.Domain;
using System.Collections.Generic;

namespace eduProjectModel.Input
{
    public class FacultyInputModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<StudyProgramInputModel> StudyProgramInputModels { get; set; } = new List<StudyProgramInputModel>();

        public void MapTo(Faculty faculty)
        {
            faculty.Name = Name;
            faculty.Address = Address;

            foreach (var model in StudyProgramInputModels)
            {
                StudyProgram studyProgram = new StudyProgram();

                model.MapTo(studyProgram);

                faculty.StudyPrograms.Add(studyProgram);
            }
        }
    }
}
