namespace P01_StudentSystem
{
    using Data;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            string[] courceLevel = 
            {
                "Basic",
                "Fundamentals",
                "Advanced",
                "DataBase"
            };

            string[] languages =
            {
                "C#",
                "Java",
                "JavaScript",
                "PHP",
                "Piton",
                "C/C++",
                "VisualBasic",
                "Delphi"
            };

            string[] firstNames =
            {
                "Angel",
                "Ivan",
                "Petar",
                "Georgi",
                "Vasil",
                "Diana",
                "Silvia",
                "Dora",
                "Elena"
            };

            string[] familyName =
{
                "Angelov",
                "Ivanov",
                "Petarov",
                "Georgiev",
                "Vasilev",
                "Nikolaev",
                "Zdravkov",
                "Blagoev",
                "Veselinov"
            };

            int rolsAfected;
            var courcesName = GenerateStrings(languages, courceLevel);
            var courses = GenerateCources(courcesName);
            rolsAfected = SeedTable<Course>(courses);
            Console.WriteLine(rolsAfected);

            var studentsName = GenerateStrings(firstNames, familyName);
            var students = GenerateStudents(studentsName);
            rolsAfected = SeedTable<Student>(students);
            Console.WriteLine(rolsAfected);

            var resources = GenerateResources(courses);
            rolsAfected = SeedTable<Resource>(resources);
            Console.WriteLine(rolsAfected);

            var relations = GenerateStudentsCourses(students, courses);
            rolsAfected = SeedTable<StudentCourse>(relations);
            Console.WriteLine(rolsAfected);
        }

        private static List<T> GetDbEntity<T>() where T : class
        {
            using (var db = new StudentSystemContext())
            {
                return db.Set<T>().ToList();
            }
        }

        private static List<StudentCourse> GenerateStudentsCourses(List<Student> students, List<Course> courses)
        {
            var random = new Random();
            var randomKvp = new List<KeyValuePair<int, int>>();
            var relations = new List<StudentCourse>();

            for (int i = 0; i < 100; i++)
            {
                var studentId = random.Next(1, students.Count);
                var courseId = random.Next(1, courses.Count);

                randomKvp.Add(new KeyValuePair<int, int>(studentId, courseId));           
            }

            randomKvp = randomKvp.Distinct().ToList();

            foreach (var kvp in randomKvp)
            {
                var relation = new StudentCourse
                {
                    StudentId = kvp.Key,
                    CourseId = kvp.Value
                };

                relations.Add(relation);
            }

            return relations;
        }

        private static List<Resource> GenerateResources(List<Course> courses)
        {
            var resources = new List<Resource>();

            Random rand = new Random();
                        
            for (int i = 1; i < 20; i++)
            {
                var index = rand.Next(0, courses.Count);
                var courseName = courses[index].Name.Replace(' ', '_');
                var resource = new Resource
                {
                    Name = $"Lecture {i} for course {courseName}",
                    Url = $"www.softuni.bg/{courseName}/Lecture/{i}/Video",
                    ResourceType = ResourceType.Video,
                    CourseId = index
                };

                resources.Add(resource);
            }

            return resources;
        }

        private static List<Student> GenerateStudents(List<String> studentsName)
        {
            var students = new List<Student>();
            var number = 1835;

            foreach (var name in studentsName)
            {
                number += 8346;
                var student = new Student
                {
                    Name = name,
                    PhoneNumber = $"+359{number.ToString("000000")}"
                };

                students.Add(student);
            }

            return students;
        }

        private static List<Course> GenerateCources(List<String> courcesName)
        {
            var courses = new List<Course>();
            var price = 100;

            foreach (var name in courcesName)
            {
                price += 5;
                var course = new Course
                {
                    Name = name,
                    Description = $"Description for course {name}.",
                    Price = price
                };

                courses.Add(course);
            }

            return courses;
        }

        private static List<String> GenerateStrings(string[] strs1, string[] strs2)
        {
            var result = new List<String>();

            foreach (var s1 in strs1)
            {
                foreach (var s2 in strs2)
                {
                    result.Add($"{s1} {s2}");
                }
            }

            return result;
        }

        private static int SeedTable<TEntity>(List<TEntity> entities) where TEntity : class
        {
            using (var context = new StudentSystemContext())
            {
                context.Set<TEntity>().AddRange(entities);

                return context.SaveChanges();
            }
        }
    }
}
