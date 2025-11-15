using Microsoft.EntityFrameworkCore;
using Lab5_TH.Models;
using System;
using System.Linq;

namespace Lab5_TH.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SchoolContext(
                serviceProvider.GetRequiredService<DbContextOptions<SchoolContext>>()))
            {
                context.Database.EnsureCreated();

                if (context.Majors.Any())
                {
                    return;
                }

                var majors = new Major[]
                {
                    new Major { MajorName = "IT" },
                    new Major { MajorName = "Economics" },
                    new Major { MajorName = "Mathematics" },
                    new Major { MajorName = "Physics" }
                };

                foreach (var major in majors)
                {
                    context.Majors.Add(major);
                }
                context.SaveChanges();
                var learners = new Learner[]
                {
                    new Learner {
                        FirstMidName = "Carson",
                        LastName = "Alexander",
                        EnrollmentDate = DateTime.Parse("2005-09-01"),
                        MajorID = 1
                    },
                    new Learner {
                        FirstMidName = "Meredith",
                        LastName = "Alonso",
                        EnrollmentDate = DateTime.Parse("2002-09-01"),
                        MajorID = 2
                    },
                    new Learner {
                        FirstMidName = "Arturo",
                        LastName = "Anand",
                        EnrollmentDate = DateTime.Parse("2003-09-01"),
                        MajorID = 3
                    },
                    new Learner {
                        FirstMidName = "Gytis",
                        LastName = "Barzdukas",
                        EnrollmentDate = DateTime.Parse("2002-09-01"),
                        MajorID = 1
                    }
                };

                foreach (Learner l in learners)
                {
                    context.Learners.Add(l);
                }
                context.SaveChanges();

                var courses = new Course[]
                {
                    new Course { CourseID = 1050, Title = "Chemistry", Credits = 3 },
                    new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3 },
                    new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 3 },
                    new Course { CourseID = 1045, Title = "Calculus", Credits = 4 },
                    new Course { CourseID = 3141, Title = "Trigonometry", Credits = 4 },
                    new Course { CourseID = 2021, Title = "Composition", Credits = 3 },
                    new Course { CourseID = 2042, Title = "Literature", Credits = 4 }
                };

                foreach (Course c in courses)
                {
                    context.Courses.Add(c);
                }
                context.SaveChanges();

                var enrollments = new Enrollment[]
                {
                    new Enrollment { LearnerID = 1, CourseID = 1050, Grade = 5.5f },
                    new Enrollment { LearnerID = 1, CourseID = 4022, Grade = 7.5f },
                    new Enrollment { LearnerID = 1, CourseID = 4041, Grade = 8.0f },
                    new Enrollment { LearnerID = 2, CourseID = 1045, Grade = 3.5f },
                    new Enrollment { LearnerID = 2, CourseID = 3141, Grade = 4.0f },
                    new Enrollment { LearnerID = 2, CourseID = 2021, Grade = 4.0f },
                    new Enrollment { LearnerID = 3, CourseID = 1050, Grade = 4.5f },
                    new Enrollment { LearnerID = 4, CourseID = 1050, Grade = 3.5f },
                    new Enrollment { LearnerID = 4, CourseID = 4022, Grade = 4.0f },
                    new Enrollment { LearnerID = 4, CourseID = 4041, Grade = 4.0f }
                };

                foreach (Enrollment e in enrollments)
                {
                    context.Enrollments.Add(e);
                }
                context.SaveChanges();
            }
        }
    }
}