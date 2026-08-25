using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace StudyOrganizer
{
    internal class Repository : IRepository
    {
        private string connectionString =
            @"Server=Lilia-LOQ\SQLEXPRESS;Database=StudyPlanner;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<string> TimeTable()
        {
            List<string> list = new List<string>();

            string query = @"
                SELECT Subject, Classroom, Time
                FROM Subject";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
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

            return list;
        }

        public void AddSubject(
            string subject,
            string professor,
            DateTime midterm1,
            DateTime midterm2,
            DateTime final,
            string classroom,
            string time,
            int m1p,
            int m2p,
            int fp,
            int partic,
            int attendance,
            int proj,
            float m1g,
            float m2g,
            float fg,
            float participg,
            float atteng,
            float projg)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string subjectQuery = @"
                            INSERT INTO Subject
                            (
                                Subject,
                                Professor,
                                MidTerm1_date,
                                MidTerm2_date,
                                Final_date,
                                Classroom,
                                Time
                            )
                            OUTPUT INSERTED.ID
                            VALUES
                            (
                                @Subject,
                                @Professor,
                                @MidTerm1_date,
                                @MidTerm2_date,
                                @Final_date,
                                @Classroom,
                                @Time
                            );";

                        int newSubjectId;

                        using (SqlCommand cmd1 =
                            new SqlCommand(subjectQuery, conn, transaction))
                        {
                            cmd1.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = subject;
                            cmd1.Parameters.Add("@Professor", SqlDbType.NVarChar).Value = professor;
                            cmd1.Parameters.Add("@MidTerm1_date", SqlDbType.DateTime).Value = midterm1;
                            cmd1.Parameters.Add("@MidTerm2_date", SqlDbType.DateTime).Value = midterm2;
                            cmd1.Parameters.Add("@Final_date", SqlDbType.DateTime).Value = final;
                            cmd1.Parameters.Add("@Classroom", SqlDbType.NVarChar).Value = classroom;
                            cmd1.Parameters.Add("@Time", SqlDbType.NVarChar).Value = time;

                            newSubjectId = Convert.ToInt32(cmd1.ExecuteScalar());
                        }

                        string percentageQuery = @"
                            INSERT INTO Percentage
                            (
                                Subject,
                                M1_percent,
                                M2_percent,
                                F_percent,
                                Participation,
                                Attendance,
                                Project
                            )
                            VALUES
                            (
                                @Subject,
                                @M1_percent,
                                @M2_percent,
                                @F_percent,
                                @Participation,
                                @Attendance,
                                @Project
                            );";

                        using (SqlCommand cmd2 =
                            new SqlCommand(percentageQuery, conn, transaction))
                        {
                            cmd2.Parameters.Add("@Subject", SqlDbType.Int).Value = newSubjectId;
                            cmd2.Parameters.Add("@M1_percent", SqlDbType.Int).Value = m1p;
                            cmd2.Parameters.Add("@M2_percent", SqlDbType.Int).Value = m2p;
                            cmd2.Parameters.Add("@F_percent", SqlDbType.Int).Value = fp;
                            cmd2.Parameters.Add("@Participation", SqlDbType.Int).Value = partic;
                            cmd2.Parameters.Add("@Attendance", SqlDbType.Int).Value = attendance;
                            cmd2.Parameters.Add("@Project", SqlDbType.Int).Value = proj;

                            cmd2.ExecuteNonQuery();
                        }

                        string gradeQuery = @"
                            INSERT INTO Grade
                            (
                                Subject,
                                M1_G,
                                M2_G,
                                F_G,
                                Participation_G,
                                Attendance_G,
                                Project_G
                            )
                            VALUES
                            (
                                @Subject,
                                @M1_G,
                                @M2_G,
                                @F_G,
                                @Participation_G,
                                @Attendance_G,
                                @Project_G
                            );";

                        using (SqlCommand cmd3 =
                            new SqlCommand(gradeQuery, conn, transaction))
                        {
                            cmd3.Parameters.Add("@Subject", SqlDbType.Int).Value = newSubjectId;
                            cmd3.Parameters.Add("@M1_G", SqlDbType.Float).Value = m1g;
                            cmd3.Parameters.Add("@M2_G", SqlDbType.Float).Value = m2g;
                            cmd3.Parameters.Add("@F_G", SqlDbType.Float).Value = fg;
                            cmd3.Parameters.Add("@Participation_G", SqlDbType.Float).Value = participg;
                            cmd3.Parameters.Add("@Attendance_G", SqlDbType.Float).Value = atteng;
                            cmd3.Parameters.Add("@Project_G", SqlDbType.Float).Value = projg;

                            cmd3.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public string SubjectInfo(string subject)
        {
            string query = @"
                SELECT Professor, Classroom, Time
                FROM Subject
                WHERE Subject = @Subject";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = subject;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string professor = reader["Professor"].ToString();
                            string classroom = reader["Classroom"].ToString();
                            string time = reader["Time"].ToString();

                            return
                                "Professor: " + professor + "\n" +
                                "Classroom: " + classroom + "\n" +
                                "Time: " + time + "\n";
                        }
                    }
                }
            }

            return "Subject not found";
        }

        public float Grade()
        {
            return 0;
        }

        public void UpdateGrade(
            string subject,
            int m1,
            int m2,
            int f,
            int p,
            int a,
            int proj)
        {
            string query = @"
                UPDATE Grades
                SET
                    M1_G = @M1_G,
                    M2_G = @M2_G,
                    F_G = @F_G,
                    Participation_G = @Participation_G,
                    Attendance_G = @Attendance_G,
                    Project_G = @Project_G
                WHERE Subject = 
                (
                    SELECT ID
                    FROM Subject
                    WHERE Subject = @Subject
                );";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar).Value = subject;
                    cmd.Parameters.Add("@M1_G", SqlDbType.Float).Value = m1;
                    cmd.Parameters.Add("@M2_G", SqlDbType.Float).Value = m2;
                    cmd.Parameters.Add("@F_G", SqlDbType.Float).Value = f;
                    cmd.Parameters.Add("@Participation_G", SqlDbType.Float).Value = p;
                    cmd.Parameters.Add("@Attendance_G", SqlDbType.Float).Value = a;
                    cmd.Parameters.Add("@Project_G", SqlDbType.Float).Value = proj;

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Subject not found or grades were not updated.");
                    }
                }
            }
        }
    }
}