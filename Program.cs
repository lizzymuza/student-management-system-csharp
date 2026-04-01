using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StudentManagementSystem
{
    // Student class represents individual students
    class Student
    {
        public string StudentID;  // ID in format S12345
        public string Name;       // Full name
        public int[] Grades;      // Array of grades

        private static int studentCount = 0; // Tracks number of Student objects

        // Static constructor 
        static Student()
        {
            Console.WriteLine("Initializing Student class...\n");
        }

        // Instance constructor 
        public Student(string studentID, string name, int[] grades)
        {
            StudentID = studentID;
            Name = name;
            Grades = grades;
            studentCount++;
        }

        // Calculates the average grade
        public double GetAverageGrade()
        {
            int sum = 0;
            foreach (int grade in Grades)
                sum += grade;
            return Grades.Length > 0 ? (double)sum / Grades.Length : 0;
        }

        // Validates Student ID (e.g., S12345)
        public static bool ValidateID(string id)
        {
            return Regex.IsMatch(id, @"^S\d{5}$");
        }

        // Returns number of students
        public static int GetStudentCount()
        {
            return studentCount;
        }

        // Destructor – called when object is destroyed
        ~Student()
        {
            Console.WriteLine($"Student object for {Name} is being destroyed.");
        }
    }

    // Manages student list and menu
    class StudentManager
    {
        static List<Student> students = new List<Student>();

        public static void Run()
        {
            int option;
            do
            {
                Console.WriteLine("\n----- Student Management System Menu -----");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Remove Student");
                Console.WriteLine("3. Display All Students");
                Console.WriteLine("4. Search Student by ID");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out option))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (option)
                {
                    case 1: AddStudent(); break;
                    case 2: RemoveStudent(); break;
                    case 3: DisplayAll(); break;
                    case 4: SearchStudent(); break;
                    case 0: Console.WriteLine("Exiting program..."); break;
                    default: Console.WriteLine("Invalid option."); break;
                }

            } while (option != 0);
        }

        // Adds a new student to the list
        static void AddStudent()
        {
            try
            {
                Console.Write("\nEnter Student ID (Format: S12345): ");
                string id = Console.ReadLine();

                if (!Student.ValidateID(id))
                    throw new FormatException("Invalid Student ID format.");

                Console.Write("Enter Student Name: ");
                string name = Console.ReadLine();

                Console.Write("Enter 3 grades separated by commas (e.g., 78,85,90): ");
                string[] gradeStrings = Console.ReadLine().Split(',');

                if (gradeStrings.Length != 3)
                    throw new ArgumentException("You must enter exactly 3 grades.");

                int[] grades = Array.ConvertAll(gradeStrings, int.Parse);

                students.Add(new Student(id, name, grades));
                Console.WriteLine("Student added successfully.");
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Input Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected Error: " + ex.Message);
            }
        }

        // Removes a student by ID
        static void RemoveStudent()
        {
            Console.Write("\nEnter Student ID to remove: ");
            string id = Console.ReadLine();

            int removed = students.RemoveAll(s => s.StudentID == id);
            if (removed > 0)
                Console.WriteLine("Student removed.");
            else
                Console.WriteLine("Student not found.");
        }

        // Displays all students in a table format
        static void DisplayAll()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("\nNo students to display.");
                return;
            }

            Console.WriteLine("\n{0,-10} {1,-20} {2,10}", "ID", "Name", "Average");
            Console.WriteLine(new string('-', 42));

            foreach (Student s in students)
            {
                Console.WriteLine("{0,-10} {1,-20} {2,10:F2}", s.StudentID, s.Name, s.GetAverageGrade());
            }
        }

        // Searches for a student by ID
        static void SearchStudent()
        {
            Console.Write("\nEnter Student ID to search: ");
            string id = Console.ReadLine();

            foreach (Student s in students)
            {
                if (s.StudentID == id)
                {
                    Console.WriteLine("\nStudent Found!");
                    Console.WriteLine("Name: " + s.Name);
                    Console.WriteLine("Average Grade: " + s.GetAverageGrade().ToString("F2"));
                    return;
                }
            }

            Console.WriteLine("Student not found.");
        }
    }

    // Main program entry
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome to the Student Management System");
            StudentManager.Run();

            Console.WriteLine("\nProgram ended. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
