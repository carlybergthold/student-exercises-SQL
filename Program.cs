using System;
using StudentExercises5.Data;
using System.Collections.Generic;
using System.Linq;
using StudentExercises5.Models;

namespace StudentExercises5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Repository repository = new Repository();
            List<Exercise> exercises = repository.GetExercises();
            exercises.ForEach(ex => Console.WriteLine(ex.ExerciseName));
        }
    }
}
