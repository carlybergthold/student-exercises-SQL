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
                    cmd.CommandText = @"SELECT s.Id AS StudentId, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, c.CohortName, 
                                            se.ExerciseId,
                                            e.ExerciseName, e.Language 
                                        FROM Students s
                                        JOIN Cohorts c
                                        On c.Id = s.CohortId
                                        JOIN StudentExercises se
                                        ON se.StudentId = s.Id
                                        JOIN Exercises e 
                                        ON e.Id = se.ExerciseId";
                    SqlDataReader reader = cmd.ExecuteReader();

                    var students = new List<Student>();

                    var exercises = new List<Exercise>();

                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };

                        Exercise exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("ExerciseName")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };

                        exercises.Add(exercise);

                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            Cohort = cohort,
                            StudentExercises = exercises
                        };

                        students.Add(student);
                    }

                    reader.Close();

                    return students;
                }
            }
        }

        public void PrintStudents(List<Student> stuList)
        {
            stuList.ForEach(stu =>
               Console.WriteLine($"Student: {stu.FirstName} is in {stu.Cohort.CohortName}")
            );
        }

        public void PrintStudentsWithExercises(List<Student> stuList)
        {
            foreach (Student stu in stuList)
                foreach (Exercise ex in stu.StudentExercises)
                    Console.WriteLine($"Student: {stu.FirstName} is in {stu.Cohort.CohortName} " +
                    $"and is working on {ex.ExerciseName}");
        }

        //TO DO - write method in repo that accepts a Cohort and Exercise and assigns that exercise
        // to each student in that cohort UNLESS the student has already been assigned
    }
}
