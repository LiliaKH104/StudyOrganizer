using System;
using System.Collections.Generic;

namespace StudyOrganizer
{
    internal class Program
    {
        public IRepository repo = new Repository();

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Start();
        }


        // ==================================================
        // START PROGRAM
        // ==================================================

        private void Start()
        {
            Console.Clear();

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~ Welcome Lilia ~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine();

            MainPage();
        }


        // ==================================================
        // MAIN PAGE
        // ==================================================

        private void MainPage()
        {
            Console.WriteLine("If you want to see your courses      press (1)");
            Console.WriteLine("If you want to add a course          press (2)");
            Console.WriteLine("If you want to update your grades    press (3)");
            Console.WriteLine("If you want to exit                  press (0)");
            Console.WriteLine();

            int a = ReadInt("Choose an option: ");

            if (a == 1)
            {
                ShowCourses();
            }
            else if (a == 2)
            {
                AddCourse();
            }
            else if (a == 3)
            {
                UpdateGrades();
            }
            else if (a == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Goodbye!");
                return;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Invalid option.");
                Console.WriteLine("Press any key to try again...");
                Console.ReadKey();

                Console.Clear();
                MainPage();
            }
        }


        // ==================================================
        // SHOW COURSES
        // ==================================================

        private void ShowCourses()
        {
            Console.Clear();

            Console.WriteLine("==============================================");
            Console.WriteLine("                 MY COURSES                   ");
            Console.WriteLine("==============================================");
            Console.WriteLine();

            List<string> list = repo.TimeTable();

            if (list.Count == 0)
            {
                Console.WriteLine("You don't have any courses yet.");
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine(list[i]);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main page...");
            Console.ReadKey();

            Console.Clear();
            MainPage();
        }


        // ==================================================
        // ADD COURSE
        // ==================================================

        private void AddCourse()
        {
            Console.Clear();

            Console.WriteLine("==============================================");
            Console.WriteLine("              ADD NEW SUBJECT                 ");
            Console.WriteLine("==============================================");
            Console.WriteLine();

            // ==========================================
            // BASIC INFORMATION
            // ==========================================

            Console.WriteLine("------------ Basic Information -------------");
            Console.WriteLine();

            Console.Write("Subject name: ");
            string subject = Console.ReadLine();

            Console.Write("Professor's name: ");
            string professor = Console.ReadLine();

            Console.Write("Classroom: ");
            string classroom = Console.ReadLine();

            Console.Write("Class time: ");
            string time = Console.ReadLine();


            // ==========================================
            // EXAM DATES
            // ==========================================

            Console.WriteLine();
            Console.WriteLine("--------------- Exam Dates -----------------");
            Console.WriteLine();

            DateTime midterm1 =
                ReadDate("Midterm 1 date (dd/MM/yyyy): ");

            DateTime midterm2 =
                ReadDate("Midterm 2 date (dd/MM/yyyy): ");

            DateTime final =
                ReadDate("Final exam date (dd/MM/yyyy): ");


            // ==========================================
            // GRADE PERCENTAGES
            // ==========================================

            Console.WriteLine();
            Console.WriteLine("----------- Grade Percentages --------------");
            Console.WriteLine();

            int m1p =
                ReadInt("Midterm 1 percentage: ");

            int m2p =
                ReadInt("Midterm 2 percentage: ");

            int fp =
                ReadInt("Final percentage: ");

            int partic =
                ReadInt("Participation percentage: ");

            int attandance =
                ReadInt("Attendance percentage: ");

            int proj =
                ReadInt("Project percentage: ");


            // ==========================================
            // CHECK PERCENTAGES
            // ==========================================

            int totalPercentage =
                m1p + m2p + fp + partic + attandance + proj;

            Console.WriteLine();
            Console.WriteLine($"Total percentage: {totalPercentage}%");

            if (totalPercentage != 100)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "WARNING: The percentages should add up to 100%."
                );

                Console.WriteLine();
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();

                Console.Clear();
                MainPage();
                return;
            }


            // ==========================================
            // CURRENT GRADES
            // ==========================================

            Console.WriteLine();
            Console.WriteLine("-------------- Current Grades --------------");
            Console.WriteLine();

            float m1g =
                ReadFloat("Midterm 1 grade: ");

            float m2g =
                ReadFloat("Midterm 2 grade: ");

            float fg =
                ReadFloat("Final grade: ");

            float participg =
                ReadFloat("Participation grade: ");

            float atteng =
                ReadFloat("Attendance grade: ");

            float projg =
                ReadFloat("Project grade: ");


            // ==========================================
            // SAVE SUBJECT
            // ==========================================

            repo.AddSubject(
                subject,
                professor,
                midterm1,
                midterm2,
                final,
                classroom,
                time,
                m1p,
                m2p,
                fp,
                partic,
                attandance,
                proj,
                m1g,
                m2g,
                fg,
                participg,
                atteng,
                projg
            );


            // ==========================================
            // SUCCESS MESSAGE
            // ==========================================

            Console.WriteLine();
            Console.WriteLine("==============================================");
            Console.WriteLine("       ✓ SUBJECT ADDED SUCCESSFULLY!         ");
            Console.WriteLine("==============================================");

            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main page...");
            Console.ReadKey();

            Console.Clear();
            MainPage();
        }


        // ==================================================
        // UPDATE GRADES
        // ==================================================

        private void UpdateGrades()
        {
            Console.Clear();

            Console.WriteLine("==============================================");
            Console.WriteLine("              UPDATE GRADES                   ");
            Console.WriteLine("==============================================");
            Console.WriteLine();

            Console.WriteLine("This section is not implemented yet.");

            Console.WriteLine();
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();

            Console.Clear();
            MainPage();
        }


        // ==================================================
        // READ INTEGER
        // ==================================================

        private int ReadInt(string message)
        {
            int value;

            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid input. Please enter a whole number."
                );
            }
        }


        // ==================================================
        // READ FLOAT
        // ==================================================

        private float ReadFloat(string message)
        {
            float value;

            while (true)
            {
                Console.Write(message);

                if (float.TryParse(Console.ReadLine(), out value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid input. Please enter a number."
                );
            }
        }


        // ==================================================
        // READ DATE
        // ==================================================

        private DateTime ReadDate(string message)
        {
            DateTime value;

            while (true)
            {
                Console.Write(message);

                if (DateTime.TryParse(Console.ReadLine(), out value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid date. Please use a format such as 25/08/2026."
                );
            }
        }
    }
}