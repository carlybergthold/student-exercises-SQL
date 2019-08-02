using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using StudentExercises5.Models;


namespace StudentExercises5.Data
{
    public class Repository
    {
        public SqlConnection Connection
        {
            get
            {
                string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=StudentExercises;Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }

        public List<Exercise> GetExercises()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, ExerciseName, Language FROM Exercises";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int languageColumnPosition = reader.GetOrdinal("Language");
                        string languageValue = reader.GetString(languageColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("ExerciseName");
                        string nameValue = reader.GetString(nameColumnPosition);

                        Exercise exercise = new Exercise
                        {
                            Id = idValue,
                            ExerciseName = nameValue,
                            Language = languageValue
                        };

                        exercises.Add(exercise);
                    }

                    reader.Close();

                    return exercises;
                }
            }
        }

        public void PrintExercises(List<Exercise> exList)
        {
            exList.ForEach(ex =>
                Console.WriteLine($"Exercise: {ex.ExerciseName}, Language: {ex.Language}"));
        }

        public void AddExerciseToDB(Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO Exercises (ExerciseName, Language) VALUES ('{exercise.ExerciseName}', '{exercise.Language}')";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Instructor> GetAllInstructorsWithCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT i.Id, i.FirstName, i.LastName, i.SlackHandle, i.Specialty,i.CohortId, c.CohortName
                                        FROM Instructors i
                                        JOIN Cohorts c
                                        ON i.CohortId = c.Id";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Instructor> instructors = new List<Instructor>();

                    while (reader.Read())
                    {
                        int idValue = reader.GetInt32(reader.GetOrdinal("Id"));
                        string firstNameValue = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastNameValue = reader.GetString(reader.GetOrdinal("LastName"));
                        string slackHandleValue = reader.GetString(reader.GetOrdinal("SlackHandle"));
                        string specialtyValue = reader.GetString(reader.GetOrdinal("Specialty"));
                        int CohortIdValue = reader.GetInt32(reader.GetOrdinal("CohortId"));
                        string cohortNameValue = reader.GetString(reader.GetOrdinal("CohortName"));

                        Instructor instructor = new Instructor
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            SlackHandle = slackHandleValue,
                            Specialty = specialtyValue
                        };

                        Cohort cohort = new Cohort
                        {
                            Id = CohortIdValue,
                            CohortName = cohortNameValue
                        };

                        instructor.Cohort = cohort;

                        instructors.Add(instructor);
                    }

                    reader.Close();

                    return instructors;
                }
            }
        }
        public void PrintInstructors(List<Instructor> inList)
        {
            inList.ForEach(inst =>
                Console.WriteLine($"{inst.FirstName} {inst.LastName} is an instructor in {inst.Cohort.CohortName}. Their specialty is {inst.Specialty}."));
        }
        public void AddInstructorToDB(Instructor inst)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO Instructors (FirstName, LastName, SlackHandle, CohortId, Specialty) VALUES ('{inst.FirstName}', '{inst.LastName}', '{inst.SlackHandle}', '{inst.Cohort.Id}', '{inst.Specialty}')";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Student> GetStudents()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, FirstName, LastName, SlackHandle, CohortId 
                                        FROM Students";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();

                    while (reader.Read())
                    {
                        int cohortIdValue = reader.GetInt32(reader.GetOrdinal("CohortId"));

                        Cohort cohort = new Cohort
                        {
                            Id = cohortIdValue,
                            CohortName: 
                        };

                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("Id")),
                            LastName = reader.GetString(reader.GetOrdinal("Id")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("Id")),
                            Cohort = cohort
                        };

                        students.Add(student);
                    }

                    reader.Close();

                    return students;
                }
            }
        }
    }
}
