using HashCode.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode
{
    static class Program
    {
        static void Main(string[] args)
        {
            string[] fileList = {
                "a_an_example.in.txt",
                "b_better_start_small.in.txt",
                "c_collaboration.in.txt",
                "d_dense_schedule.in.txt",
                "e_exceptional_skills.in.txt",
                "f_find_great_mentors.in.txt",
            };

            for (int i = 0; i < fileList.Length; i++)
            {
                loadData(fileList[i]);
            }
        }

        private static void loadData(string fileName)
        {
            List<Person> persons = new List<Person>();
            List<Project> projects = new List<Project>();
            string readedFile = ReadTxtFile(fileName);

            string[] firstLineParts;
            string contractorTotal;
            int personlastLine;
            ParseFile(readedFile, out firstLineParts, out contractorTotal, out personlastLine);
            personlastLine = FillPersons(persons, firstLineParts, contractorTotal, personlastLine);
            FillProjects(projects, firstLineParts, personlastLine);

            List<OutputSubmission> resultOutputSubmissions = Output(persons, projects);
            string resultFile = string.Empty;
            resultOutputSubmissions[0].TotalProject = resultOutputSubmissions.Where(C => C.Persons.Count() != 0).Count().ToString();

            resultFile += resultOutputSubmissions[0].TotalProject + Environment.NewLine;
            foreach (var item in resultOutputSubmissions)
            {
                if (item.Persons.Count() < 1)
                {
                    continue;
                }
                resultFile += item.ProjectName + Environment.NewLine;
                foreach (var person in item.Persons)
                {
                    resultFile += person.Name + " ";
                }
                resultFile.TrimEnd();
                resultFile = resultFile + Environment.NewLine;
            }
            File.WriteAllText(@"C:\"+ fileName + "", resultFile);
            Console.WriteLine("PROCESSİNG:" + fileName);
        }

        private static List<OutputSubmission> Output(List<Person> persons, List<Project> projects)
        {
            List<OutputSubmission> outputSubmissions = new List<OutputSubmission>();
            foreach (var project in projects)
            {
                OutputSubmission outputSubmission = new OutputSubmission();
                outputSubmission.TotalProject = projects.Count().ToString();
                foreach (var projectRole in project.ProjectRoles)
                {
                    foreach (var per in persons)
                    {
                        foreach (var item in per.PersonSkills)
                        {
                            if ((item.Skill == projectRole.RoleName) && Convert.ToInt32(item.SkillLevel) >= Convert.ToInt32(projectRole.RoleLevel))
                            {
                                outputSubmission.ProjectName = project.NameProject;
                                outputSubmission.Persons.Add(per);
                            }
                        }
                    }
                }
                outputSubmissions.Add(outputSubmission);
            }
            return outputSubmissions;
        }

        private static void ParseFile(string readedFile, out string[] firstLineParts, out string contractorTotal, out int personlastLine)
        {
            firstLineParts = readedFile.Split('\n');
            contractorTotal = firstLineParts[0].Split(' ')[0];
            string projectsTotal = firstLineParts[0].Split(' ')[1];
            personlastLine = 0;
        }

        private static string ReadTxtFile(string fileName)
        {
            string readedFile;
            string path = Path.Combine(Environment.CurrentDirectory, @"InputFolder\", fileName);
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                readedFile = streamReader.ReadToEnd();
            }

            return readedFile;
        }

        private static void FillProjects(List<Project> projects, string[] firstLineParts, int personlastLine)
        {
            for (int i = personlastLine; i < firstLineParts.Length; i++)
            {
                if (firstLineParts[i] == "")
                {
                    continue;
                }
                Project project = new Project();
                project.NameProject = firstLineParts[i].Split(' ')[0];
                project.Day = firstLineParts[i].Split(' ')[1];
                project.Score = firstLineParts[i].Split(' ')[2];
                project.BestPracticeDay = firstLineParts[i].Split(' ')[3];
                project.TotalSkillNumberOfRole = firstLineParts[i].Split(' ')[4];

                List<ProjectRole> projectRoles = new List<ProjectRole>();
                for (int j = 1; j <= Convert.ToInt32(project.TotalSkillNumberOfRole); j++)
                {
                    if (firstLineParts[i] == "")
                    {
                        continue;
                    }
                    int numberOfSubLine = i + j;
                    if (firstLineParts.Length - 1 <= numberOfSubLine)
                    {
                        break;
                    }
                    ProjectRole projectRole = new ProjectRole();
                    projectRole.RoleName = firstLineParts[i + j].Split(' ')[0];
                    projectRole.RoleLevel = firstLineParts[numberOfSubLine].Split(' ')[1];
                    projectRoles.Add(projectRole);
                }
                project.ProjectRoles = projectRoles;

                i = i + Convert.ToInt32(project.TotalSkillNumberOfRole);
                projects.Add(project);
            }
        }

        private static int FillPersons(List<Person> persons, string[] firstLineParts, string contractorTotal, int personlastLine)
        {
            for (int i = 1; i < firstLineParts.Length; i++)
            {
                personlastLine = i;
                if (persons.Count() == Convert.ToInt32(contractorTotal))
                {
                    break;
                }
                Person person = new Person();
                person.Name = firstLineParts[i].Split(' ')[0];
                person.TotalSkill = firstLineParts[i].Split(' ')[1];

                List<PersonSkill> personSkillList = new List<PersonSkill>();
                for (int j = 1; j <= Convert.ToInt32(person.TotalSkill); j++)
                {
                    if (firstLineParts[i] == "")
                    {
                        continue;
                    }
                    int numberOfSubLine = i + j;
                    if (firstLineParts.Length - 1 <= numberOfSubLine)
                    {
                        break;
                    }
                    PersonSkill personSkill = new PersonSkill();
                    personSkill.Skill = firstLineParts[i + j].Split(' ')[0];
                    personSkill.SkillLevel = firstLineParts[numberOfSubLine].Split(' ')[1];
                    personSkillList.Add(personSkill);
                }
                person.PersonSkills = personSkillList;
                i = i + Convert.ToInt32(person.TotalSkill);
                persons.Add(person);
            }

            return personlastLine;
        }
    }}