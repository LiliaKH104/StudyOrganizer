using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyOrganizer
{
    internal class Repository : IRepository
    {
        string connectionString = @"Server=Lilia-LOQ\SQLEXPRESS;Database=StudyPlanner;Trusted_Connection=True;TrustServerCertificate=True;";
        public List<string> TimeTable()
        {
            List<string> list = new List<string>();

            string query = "SELECT Subject, Classroom, Time  FROM Subject";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string subject = reader["Subject"].ToString();
                            string classroom = reader["Classroom"].ToString();
                            string time = reader["Time"].ToString();

                            list.Add($"{subject} | {classroom} | {time}");
                        }
                    }
                }
            }
            return list;

        }

        public void AddSubject(string subject, string professor, DateTime midterm1, DateTime midterm2, DateTime final, string classroom, string time, int m1p, int m2p, int fp, int partic, int attendance, int proj, float m1g, float m2g, float fg, float participg, float atteng, float projg)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {

                        string subjectQuery = @"
                    INSERT INTO Subject ( Subject, Professor, MidTerm1_date, MidTerm2_date, Final_date, Classroom, Time
                    ) 
                    VALUES (
                        @Subject, @Professor, @MidTerm1_date, @MidTerm2_date, @Final_date, @Classroom, @Time
                    );";


                        int newSubjectId;
                        using (SqlCommand cmd1 = new SqlCommand(subjectQuery, conn, transaction))
                        {
                            cmd1.Parameters.AddWithValue("@Subject", subject);
                            cmd1.Parameters.AddWithValue("@Professor", professor);
                            cmd1.Parameters.AddWithValue("@MidTerm1_date", midterm1);
                            cmd1.Parameters.AddWithValue("@MidTerm2_date", midterm2);
                            cmd1.Parameters.AddWithValue("@Final_date", final);
                            cmd1.Parameters.AddWithValue("@Classroom", classroom);
                            cmd1.Parameters.AddWithValue("@Time", time);

                            newSubjectId = (int)cmd1.ExecuteScalar();
                        }

                        string scheduleQuery = @"
                    INSERT INTO Percentage ( Subject, M1_percent, M2_percent, F_percent, Participation, Attendance, Project
                    ) 
                    VALUES (
                        @Subject, @M1_percent, @M2_percent, @F_percent, @Participation, @Attendance, @Project
                    );";

                        using (SqlCommand cmd2 = new SqlCommand(scheduleQuery, conn, transaction))
                        {
                            cmd2.Parameters.AddWithValue("@Subject", newSubjectId);
                            cmd2.Parameters.AddWithValue("@M1_percent", m1p);
                            cmd2.Parameters.AddWithValue("@M2_percent", m2p);
                            cmd2.Parameters.AddWithValue("@F_percent", fp);
                            cmd2.Parameters.AddWithValue("@Participation", partic);
                            cmd2.Parameters.AddWithValue("@Attendance", attendance);
                            cmd2.Parameters.AddWithValue("@Project", proj);

                            cmd2.ExecuteNonQuery();
                        }

                        string gradeQuery = @"
                    INSERT INTO Grades (
                        Subject, M1_G, M2_G, F_G, Participation_G, Attendance_G, Project_G
                    ) 
                    VALUES (
                        @Subject, @M1_G, @M2_G, @F_G, @Participation_G, @Attendance_G, @Project_G
                    );";

                        using (SqlCommand cmd3 = new SqlCommand(gradeQuery, conn, transaction))
                        {
                            cmd3.Parameters.AddWithValue("@Subject", newSubjectId);
                            cmd3.Parameters.AddWithValue("@M1_G", m1p);
                            cmd3.Parameters.AddWithValue("@M2_G", m2p);
                            cmd3.Parameters.AddWithValue("@F_G", fp);
                            cmd3.Parameters.AddWithValue("@Participation_G", partic);
                            cmd3.Parameters.AddWithValue("@Attendance_G", attendance);
                            cmd3.Parameters.AddWithValue("@Project_G", proj);


                            cmd3.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }

        public string SubjectInfo(string subject)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {

                    string query = "SELECT * FROM Subject WHERE Subject = @Subject";

                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {

                        cmd.Parameters.AddWithValue("@Subject", subject);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                string prof = reader["Professor"].ToString();
                                string classroom = reader["Classroom"].ToString();
                                string time = reader["Time"].ToString();


                                string result = "Professor: " + prof + "\n" + "Classroom: " + classroom + "\n" + "Time: " + time + "\n";
                                transaction.Commit();
                                return result;
                            }
                        }
                    }

                    transaction.Commit();
                }
            }
            return "Subject not found";
        }
        public float Grade()
        {
            return 0;
        }
        public void UpdateGrade(string subject, int m1, int m2, int f, int p, int a, int proj)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {

                    string query = "UPDATE Grade SET Score = @Score WHERE Subject = @Subject";

                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {

                        cmd.Parameters.AddWithValue("@Subject", subject);
                        cmd.Parameters.AddWithValue("@M1_G", m1);
                        cmd.Parameters.AddWithValue("@M2_G", m2);
                        cmd.Parameters.AddWithValue("@F_G", f);
                        cmd.Parameters.AddWithValue("@Participation_G", p);
                        cmd.Parameters.AddWithValue("@Attendance_G", a);
                        cmd.Parameters.AddWithValue("@Project_G", proj);

                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}