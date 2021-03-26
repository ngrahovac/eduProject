using Blazored.Modal;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Shared
{
    public partial class UserInformationView
    {

        // input model for collaborator profile the user is currently creating
        private CollaboratorProfileInputModel collaboratorProfileInputModel = new CollaboratorProfileInputModel();

        // selected values
        private string cycleStr;
        private string yearStr;

        [Parameter] public UserProfileInputModel UserProfileInputModel { get; set; }

        // combo boxes backing lists
        [Parameter] public ProjectInputModel ProjectInputModel { get; set; }
        [Parameter] public ICollection<StudyField> studyFields { get; set; } = new List<StudyField>();
        [Parameter] public ICollection<Faculty> faculties { get; set; } = new List<Faculty>();
        [Parameter] public bool editingProfile { get; set; }
        [Parameter] public int profileIndex { get; set; }
        //[Parameter] public ICollection<Tag> tags { get; set; } = new List<Tag>();

        private ICollection<string> cycles = new List<string>();
        private ICollection<StudyProgram> programs = new List<StudyProgram>();
        private ICollection<StudyProgramSpecialization> specializations = new List<StudyProgramSpecialization>();
        private ICollection<int> years = new List<int>();

        private Faculty faculty;
        int cycle;

        private async Task UserAccountTypeChanged(UserAccountType type)
        {
            if (type != UserProfileInputModel.UserAccountType)
            {
                UserProfileInputModel.Clear();
                UserProfileInputModel.UserAccountType = type;

                // cistimo formu
                yearStr = string.Empty;
                cycleStr = string.Empty;

                Console.WriteLine($"Odabran tip korisnika {type}");
            }
        }

        private async Task FacultySelected(string facultyName)
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
                UserProfileInputModel.FacultyName = facultyName;
                faculty = faculties.Where(f => f.Name == facultyName).First();
                cycles = faculty.StudyPrograms.Select(p => p.Cycle).Distinct().Select(c => $"{c}").ToList();
            }
            else
            {
                UserProfileInputModel.FacultyName = null;
            }
        }

        private async Task CycleSelected(string cycleStr)
        {
            this.cycleStr = cycleStr;
            Console.WriteLine($"Odabran ciklus {cycleStr}");
            programs.Clear();
            specializations.Clear();
            years.Clear();

            if (cycleStr != string.Empty)
            {
                UserProfileInputModel.Cycle = int.Parse(cycleStr);
                cycle = int.Parse(cycleStr);
                programs = faculty.StudyPrograms.Where(sp => sp.Cycle == cycle).ToList();
            }
            else
            {
                UserProfileInputModel.Cycle = null;
            }
        }

        private async Task ProgramSelected(string programName)
        {
            Console.WriteLine($"Odabran program {programName}");
            specializations.Clear();
            years.Clear();
            yearStr = string.Empty;

            if (programName != string.Empty)
            {
                UserProfileInputModel.StudyProgramName = programName;
                var program = programs.Where(p => p.Cycle == cycle && p.Name == programName).First();
                Console.WriteLine("program je ");
                Console.WriteLine(program);
                specializations = program.StudyProgramSpecializations.ToList();
                years = Enumerable.Range(1, program.DurationYears).ToList();
            }
            else
            {
                UserProfileInputModel.StudyProgramName = null;
            }
        }

        private async Task YearSelected(string yearStr)
        {
            this.yearStr = yearStr;
            Console.WriteLine($"Odabran ciklus {yearStr}");

            if (yearStr != string.Empty)
            {
                UserProfileInputModel.StudyYear = int.Parse(yearStr);
            }
            else
            {
                UserProfileInputModel.StudyYear = null;
            }
        }

        private async Task SpecializationSelected(string specializationName)
        {
            Console.WriteLine($"Odabran smjer {specializationName}");

            if (specializationName != string.Empty)
            {
                UserProfileInputModel.StudyProgramSpecializationName = specializationName;
            }
            else
            {
                UserProfileInputModel.StudyProgramSpecializationName = null;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            /*
            if (editingProfile == true)
            {
                collaboratorProfileInputModel = ProjectInputModel.CollaboratorProfileInputModels.ElementAt(profileIndex);
                base.StateHasChanged();

                var model = collaboratorProfileInputModel;
                await CollaboratorProfileTypeChanged(model.CollaboratorProfileType);

                if (model.CollaboratorProfileType == CollaboratorProfileType.Student)
                {
                    if (model.FacultyName != null)
                    {
                        await FacultySelected(model.FacultyName);
                        if (model.Cycle != null)
                        {
                            await CycleSelected(model.Cycle.ToString());
                            if (model.StudyProgramName != null)
                            {
                                await ProgramSelected(model.StudyProgramName);
                                if (model.StudyYear != null)
                                {
                                    await YearSelected(model.StudyYear.ToString());
                                    if (model.StudyProgramSpecializationName != null)
                                    {
                                        await SpecializationSelected(model.StudyProgramSpecializationName);
                                    }
                                }
                            }
                        }
                    }
                }

                else if (model.CollaboratorProfileType == CollaboratorProfileType.FacultyMember)
                {
                    if (model.FacultyName != null)
                    {
                        await FacultySelected(model.FacultyName);
                    }
                    Console.WriteLine($"NAUCNA OBLAST JE {model.StudyFieldName}");
                    base.StateHasChanged();
                }
            }
            */
        }


        /*
    private async void SaveUserProfile()
    {
        Console.WriteLine("Saving..\n");
        foreach (var prop in typeof(UserProfileInputModel).GetProperties())
        {
            Console.WriteLine($"{prop.Name} ima vrijednost {prop.GetValue(UserProfileInputModel)}\n");
        }

        if (!editingProfile)
        {
            Console.WriteLine($"Adding {collaboratorProfileInputModel.CollaboratorProfileType}");
            collaboratorProfileInputModel.ExistingProfile = false;
            ProjectInputModel.CollaboratorProfileInputModels.Add(collaboratorProfileInputModel);
            collaboratorProfileInputModel = new CollaboratorProfileInputModel();

            yearStr = string.Empty;
            cycleStr = string.Empty;

            await BlazoredModal.Close();
        }
        else
        {
            Console.WriteLine($"Editing {collaboratorProfileInputModel.CollaboratorProfileType}");
            collaboratorProfileInputModel.ExistingProfile = false;
            ProjectInputModel.CollaboratorProfileInputModels.RemoveAt(profileIndex);
            ProjectInputModel.CollaboratorProfileInputModels.Insert(profileIndex, collaboratorProfileInputModel);

            collaboratorProfileInputModel = new CollaboratorProfileInputModel();

            yearStr = string.Empty;
            cycleStr = string.Empty;

            await BlazoredModal.Close();
        }
        
    }*/


    }
}
