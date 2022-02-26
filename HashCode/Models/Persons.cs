using System.Collections.Generic;

namespace HashCode.Models
{
    public class Person
    {
        public string Name { get; set; }
        public string TotalSkill { get; set; }
        public List<PersonSkill> PersonSkills { get; set; }
    }

    public class PersonSkill
    {
        public string Skill { get; set; }
        public string SkillLevel { get; set; }
    }

    public class Project
    {
        public string NameProject { get; set; }
        public string Day { get; set; }
        public string BestPracticeDay { get; set; }
        public string Score { get; set; }
        public string TotalSkillNumberOfRole { get; set; }
        public List<ProjectRole> ProjectRoles { get; set; }
    }


    public class ProjectRole
    {
        public string RoleName { get; set; }
        public string RoleLevel { get; set; }
    }

    public class OutputSubmission
    {
        public OutputSubmission()
        {
            Persons = new List<Person>();
        }
        public string TotalProject { get; set; }
        public string ProjectName { get; set; }
        public List<Person> Persons { get; set; }
    }
}
