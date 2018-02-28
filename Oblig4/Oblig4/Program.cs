using Oblig4.Context;
using Oblig4.Model;
using System;
using System.Data.SqlClient;
using System.Globalization;

namespace Oblig4
{
    /// <summary>
    /// Student / Course application
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        static void Main()
        {
            Console.WriteLine("Welcome to the student/course application.");
            SelectionInfo();
            Boolean quit = false;
            while (!quit)
            {
                string input = Console.ReadLine();
                Console.WriteLine();
                switch (input)
                {
                    case "1":
                        GetDataFromDb("Student");
                        break;
                    case "2":
                        GetDataFromDb("Course");
                        break;
                    case "3":
                        PostToDb("Student");
                        break;
                    case "4":
                        PostToDb("Course");
                        break;
                    case "5":
                        AddStudentToCourse();
                        break;
                    case "6":
                        CheckCourseStudents();
                        break;
                    case "q":
                        quit = true;
                        break;
                    case "Q":
                        quit = true;
                        break;
                    case "test":
                        //PopulateDbWithTestData();
                        break;
                    default:
                        Console.WriteLine("Please type a valid command.");
                        break;
                }
                SelectionInfo();
            }
        }

        /// <summary>
        /// Print selection information
        /// </summary>
        private static void SelectionInfo()
        {
            String[] lines =
            {
                "Please select an option:",
                "   1 - Get students from DB",
                "   2 - Get courses from DB",
                "   3 - Add new student to DB",
                "   4 - Add new course to DB",
                "   5 - Add student to an existing course",
                "   6 - Check course for students",
                "   Q - Quit application"
            };
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Checks the course students.
        /// </summary>
        private static void CheckCourseStudents()
        {
            GetDataFromDb("Course");
            Console.WriteLine("Specify the ID of the course you want to check");
            var courseId = Console.ReadLine();
            bool isNumeric = int.TryParse(courseId, out int cId);
            if (isNumeric)
            {
                try
                {
                    using (var con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Oblig4.Context.DatabaseContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE StudentId IN (SELECT StudentId FROM StudentCourse WHERE CourseId = " + cId + ")", con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        Console.WriteLine(reader.GetValue(i));
                                    }
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Error: Input is not a number.");
            }
        }

        /// <summary>
        /// Adds the student to course.
        /// </summary>
        private static void AddStudentToCourse()
        {
            Console.WriteLine("Specify the ID of a course to add a student to:");
            GetDataFromDb("Course");
            var courseId = Console.ReadLine();
            bool isNumeric = int.TryParse(courseId, out int cId);
            if (isNumeric)
            {
                Console.WriteLine("Specify the ID of the student to add to this course:");
                GetDataFromDb("Student");
                var studentId = Console.ReadLine();
                isNumeric = int.TryParse(studentId, out int sId);
                if (isNumeric)
                {
                    try
                    {
                        using (var con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Oblig4.Context.DatabaseContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO StudentCourse (StudentId,CourseId) VALUES('"
                                                    + sId + "','" + cId + "')", con);
                            cmd.ExecuteReader();
                            con.Close();
                            Console.WriteLine("Added student " + sId + " to course " + cId);
                        }
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Error: Input is not a number.");
                }
            }
            else
            {
                Console.WriteLine("Error: Input is not a number.");
            }
        }

        /// <summary>
        /// Posts to database.
        /// </summary>
        /// <param name="table">The table.</param>
        private static void PostToDb(string table)
        {
            using (var db = new DatabaseContext())
            {
                string format = "yyyy.MM.dd";
                if (table == "Course")
                {
                    Console.WriteLine("Please write the course title.");
                    string title = Console.ReadLine();

                    var course = new Course() { Title = title };
                    db.Courses.Add(course);

                    Console.WriteLine("Created course: " + title);
                    Console.WriteLine();
                }
                else if (table == "Student")
                {
                    Console.WriteLine("Please write the students first name");
                    string fName = Console.ReadLine();
                    Console.WriteLine("Please write the students last name");
                    string lName = Console.ReadLine();
                    Console.WriteLine("Please write the students birth date (Format: YYYY.MM.DD");
                    string d = Console.ReadLine();
                    if (DateTime.TryParseExact(d, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    {
                        if (date.Year < 1753)
                        {
                            Console.WriteLine("Nice try");
                        }
                        else
                        {
                            var student = new Student() { FirstName = fName, LastName = lName, DateOfBirth = date };
                            db.Students.Add(student);

                            Console.WriteLine("Added student: " + fName + " " + lName + " - " + date);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong date format specified");
                    }
                    Console.WriteLine();
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the data from database.
        /// </summary>
        /// <param name="table">The table.</param>
        private static void GetDataFromDb(string table)
        {
            try
            {
                using (var con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Oblig4.Context.DatabaseContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + table, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.WriteLine(reader.GetValue(i));
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /*
        private static void PopulateDbWithTestData()
        {
            try
            {
                // Test the model
                var mike = new Student()
                {
                    FirstName = "Michael",
                    LastName = "M. Simon",
                    DateOfBirth = new DateTime(1994, 1, 1)
                };
                var per = new Student()
                {
                    FirstName = "Per",
                    LastName = "Knudsen",
                    DateOfBirth = new DateTime(1984, 5, 19)
                };
                var knut = new Student()
                {
                    FirstName = "Knut",
                    LastName = "Henriksson",
                    DateOfBirth = new DateTime(1997, 3, 12)
                };
                var jonas = new Student()
                {
                    FirstName = "Jonas",
                    LastName = "Vestgarden",
                    DateOfBirth = new DateTime(1993, 3, 07)
                };

                var dotNet = new Course() { Title = ".NET", Students = new List<Student>() { mike, per, jonas } };
                var mod = new Course() { Title = "3D modellering", Students = new List<Student>() { knut, per } };
                var bigData = new Course() { Title = "Big Amounts of Data", Students = new List<Student>() { mike, jonas } };

                Console.WriteLine("Please wait while the database is created.");
                using (var db = new DatabaseContext())
                {
                    db.Courses.Add(dotNet);
                    db.Courses.Add(mod);
                    db.Courses.Add(bigData);

                    db.Students.Add(mike);
                    db.Students.Add(jonas);
                    db.Students.Add(per);
                    db.Students.Add(knut);


                    db.SaveChanges();
                }
                Console.WriteLine("Operation completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while creating the database.");
                Console.WriteLine(ex.Message);
            }
        }
        */
    }
}