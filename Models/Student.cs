﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StudentExercises5.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SlackHandle { get; set; }
        public Cohort Cohort { get; set; }
        public List<Exercise> StudentExercises { get; set; }
    }
}
