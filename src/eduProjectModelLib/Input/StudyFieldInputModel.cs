using eduProjectModel.Domain;

namespace eduProjectModel.Input
{
    public class StudyFieldInputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public void MapTo(StudyField field)
        {
            field.Name = Name;
            field.Description = Description;
        }
    }
}
