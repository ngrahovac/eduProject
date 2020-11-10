using Blazored.Modal;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Shared
{
    public partial class ProjectCreateAddCollaborator
    {
        private bool editing = false;

        // Posto sam napravio Singleton instancu, ova linija koda je nepotrebna
        // private ProjectInputModel projectInputModel = new ProjectInputModel();

        // input model for collaborator profile the user is currently creating
        private CollaboratorProfileInputModel collaboratorProfileInputModel = new CollaboratorProfileInputModel();

        // selected values
        private string cycleStr;
        private string yearStr;

        // combo boxes backing lists
        private ICollection<StudyField> studyFields = new List<StudyField>();
        private ICollection<Faculty> faculties = new List<Faculty>();
        private ICollection<string> cycles = new List<string>();
        private ICollection<StudyProgram> programs = new List<StudyProgram>();
        private ICollection<StudyProgramSpecialization> specializations = new List<StudyProgramSpecialization>();
        private ICollection<int> years = new List<int>();
        private ICollection<Tag> tags = new List<Tag>();

        private Faculty faculty;
        int cycle;
        private StudyProgram program;
        private StudyProgramSpecialization specialization;

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }

        protected override void OnInitialized() => BlazoredModal.SetTitle("Dodavanje saradnika");

        private async void CollaboratorProfileTypeChanged(CollaboratorProfileType type)
        {
            collaboratorProfileInputModel = new CollaboratorProfileInputModel();
            yearStr = string.Empty;
            cycleStr = string.Empty;

            collaboratorProfileInputModel.CollaboratorProfileType = type;
            Console.WriteLine($"Odabran tip saradnika {type}");
        }

        private async void AddCollaboratorProfile()
        {
            Console.WriteLine($"Adding {collaboratorProfileInputModel.CollaboratorProfileType}");
            collaboratorProfileInputModel.AddedOnCreate = !editing;
            projectInputModel.CollaboratorProfileInputModels.Add(collaboratorProfileInputModel);
            collaboratorProfileInputModel = new CollaboratorProfileInputModel();

            yearStr = string.Empty;
            cycleStr = string.Empty;
            
            _ = BlazoredModal.Close();
        }

        private async void FacultySelected(string facultyName)
        {
            Console.WriteLine($"Odabran fakultet {facultyName}");
            cycles.Clear();
            programs.Clear();
            specializations.Clear();
            years.Clear();
            cycleStr = string.Empty;
            yearStr = string.Empty;

            if (facultyName != string.Empty)
            {
                collaboratorProfileInputModel.FacultyName = facultyName;
                faculty = faculties.Where(f => f.Name == facultyName).First();
                cycles = faculty.StudyPrograms.Select(p => p.Cycle).Distinct().Select(c => $"{c}").ToList();
            }
            else
            {
                collaboratorProfileInputModel.FacultyName = null;
            }
        }

        private async void CycleSelected(string cycleStr)
        {
            this.cycleStr = cycleStr;
            Console.WriteLine($"Odabran ciklus {cycleStr}");
            programs.Clear();
            specializations.Clear();
            years.Clear();

            if (cycleStr != string.Empty)
            {
                collaboratorProfileInputModel.Cycle = int.Parse(cycleStr);
                cycle = int.Parse(cycleStr);
                programs = faculty.StudyPrograms.Where(sp => sp.Cycle == cycle).ToList();
            }
            else
            {
                collaboratorProfileInputModel.Cycle = null;
            }
        }

        private async void ProgramSelected(string programName)
        {
            Console.WriteLine($"Odabran program {programName}");
            specializations.Clear();
            years.Clear();
            yearStr = string.Empty;

            if (programName != string.Empty)
            {
                collaboratorProfileInputModel.StudyProgramName = programName;
                var program = programs.Where(p => p.Cycle == cycle && p.Name == programName).First();
                specializations = program.StudyProgramSpecializations.ToList();
                years = Enumerable.Range(1, program.DurationYears).ToList();
            }
            else
            {
                collaboratorProfileInputModel.StudyProgramName = null;
            }
        }

        private async void YearSelected(string yearStr)
        {
            this.yearStr = yearStr;
            Console.WriteLine($"Odabran ciklus {yearStr}");

            if (yearStr != string.Empty)
            {
                collaboratorProfileInputModel.StudyYear = int.Parse(yearStr);
            }
            else
            {
                collaboratorProfileInputModel.StudyYear = null;
            }
        }

        private async void SpecializationSelected(string specializationName)
        {
            Console.WriteLine($"Odabran smjer {specializationName}");

            if (specializationName != string.Empty)
            {
                collaboratorProfileInputModel.StudyProgramSpecializationName = specializationName;
            }
            else
            {
                collaboratorProfileInputModel.StudyProgramSpecializationName = null;
            }
        }
    }
}
