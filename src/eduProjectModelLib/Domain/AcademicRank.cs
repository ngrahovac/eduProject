using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Domain
{
    public enum AcademicRank
    {
        [Display(Name = "Asistent")]
        TeachingAssistant = 1,

        [Display(Name = "Viši asistent")]
        SeniorTeachingAssistant = 2,

        [Display(Name = "Docent")]
        AssistantProfessor = 3,

        [Display(Name = "Redovni profesor")]
        FullProfessor = 4,

        [Display(Name = "Gostujući profesor")]
        VisitingProfessor = 5,

        [Display(Name = "Profesor emeritus")]
        ProfessorEmeritus = 6
    }
}
