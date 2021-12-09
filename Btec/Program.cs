using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Btec
{
    public class Program : BaseProgram
    {
        private static IEnumerable<Student> GetList(string tablename)
        {
            try
            {
                var query = @"Select * from Students";

                using (var conn = new SqlConnection(ConnectionString))
                {
                    var listStudents = conn.Query<Student>(query);
                    return listStudents;
                }
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
        }

        private static int Create(string name, DateTime dob, string email, string address, string batch)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    try
                    {
                        string query = @"Insert into Students values(@id,@name,@dob,@email,@address,@batch)";
                        var param = new
                        {
                            @id = Guid.NewGuid(),
                            @name = name,
                            @dob = dob,
                            @email = email,
                            @address = address,
                            @batch = batch
                        };
                        var studentId = conn.Execute(query, param);
                        return studentId;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static int Delete(Guid Id)
        {
            return WithConnection(conn =>
              {
                  int query = conn.QuerySingle<int>(@"Delete From Students Where Id=@id", new { Id = Id });
                  return query;
              });
        }

        private static int Update(Guid Id, Student updateStudent)
        {
            return WithConnection(conn =>
            {
                int query = conn.QuerySingle<int>(@"Update Students Set name=@name,dob=@dob,email=@email,address=@address,batch=@batch", new { @name = updateStudent.Name, @dob = updateStudent.Dob, @email = updateStudent.Email, @address = updateStudent.Address, @batch = updateStudent.Batch });

                return query;
            });
        }

        private static void Main(string[] args)
        {
            List<Student> students = new List<Student>(GetList("students"));
            List<Lecturer> lecturers = new List<Lecturer>();
            while (true)
            {
                try
                {
                    WriteLog("==================================");
                    WriteLog("     1. Manage Students");
                    WriteLog("     2. Manage Lectures");
                    WriteLog("     3. Exit");
                    WriteLog("==================================");
                    int choice = int.Parse(Console.ReadLine());
                    Console.Clear();
                    if (choice == 1)
                    {
                        while (true)
                        {
                            try
                            {
                                WriteLog("==================================");
                                WriteLog("    1.	Add new student");
                                WriteLog("    2.	View all students");
                                WriteLog("    3.	Search students");
                                WriteLog("    4.	Delete students");
                                WriteLog("    5.	Update student");
                                WriteLog("    6.	Back to main menu");
                                WriteLog("==================================");
                                int subChoice = int.Parse(Console.ReadLine());
                                Console.Clear();
                                if (subChoice == 1)
                                {
                                    Guid id = Guid.NewGuid();
                                    WriteLogOneLine("Name: ");
                                    string Name = Console.ReadLine();
                                    WriteLogOneLine("Dob: ");
                                    DateTime Dob = DateTime.Parse(Console.ReadLine());
                                    WriteLogOneLine("Email: ");
                                    string Email = Console.ReadLine();
                                    WriteLogOneLine("Address: ");
                                    string Address = Console.ReadLine();
                                    WriteLogOneLine("Batch: ");
                                    string Batch = Console.ReadLine();
                                    students.Add(new Student(id, Name, Dob, Email, Address, Batch));
                                    // add director to db
                                    Create(Name, Dob, Email, Address, Batch);
                                    WriteLog("Add Successfully");
                                }
                                else if (subChoice == 2)
                                {
                                    WriteLog("====\tList Students\t===");
                                    foreach (Student student in students)
                                    {
                                        WriteLog(student.ToString());
                                        WriteLog("==========================");
                                        WriteLog();
                                    }
                                }
                                else if (subChoice == 3)
                                {
                                    WriteLog("====\tStudent\t===");
                                    WriteLog("Name or Id to Search ...");
                                    string search = Console.ReadLine();
                                    var containStudent = students.Where(x => x.Name.Contains(search) || x.Id.Equals(search));
                                    Console.WriteLine(containStudent);
                                    foreach (Student student in containStudent)
                                    {
                                        Console.WriteLine(student.ToString());
                                        WriteLog("==========================");
                                        WriteLog();
                                    }
                                }
                                else if (subChoice == 4)
                                {
                                    WriteLogOneLine("Id: ");
                                    Guid deleteId = Guid.Parse(Console.ReadLine());
                                    var deleteStudent = students.Find(x => x.Id.Equals(deleteId));
                                    if (deleteStudent == null) WriteLog("Can't find anyone");
                                    else
                                    {
                                        students.Remove(deleteStudent);
                                        //Remove to database
                                        //Delete(deleteId);
                                        WriteLog("Delete Successfully");
                                    }
                                }
                                else if (subChoice == 5)
                                {
                                    WriteLogOneLine("Id: ");
                                    Guid updateId = Guid.Parse(Console.ReadLine());
                                    var updateStudent = students.Find(x => x.Id.Equals(updateId));
                                    if (updateStudent == null) WriteLog("Can't find anyone");
                                    else
                                    {
                                        WriteLogOneLine("Name: ");
                                        updateStudent.Name = Console.ReadLine();
                                        WriteLogOneLine("Dob: ");
                                        updateStudent.Dob = DateTime.Parse(Console.ReadLine());
                                        WriteLogOneLine("Email: ");
                                        updateStudent.Email = Console.ReadLine();
                                        WriteLogOneLine("Address: ");
                                        updateStudent.Address = Console.ReadLine();
                                        WriteLogOneLine("Batch: ");
                                        updateStudent.Batch = Console.ReadLine();
                                        WriteLog("Update Successfully");
                                        //Update(updateId, updateStudent);
                                        // If you want update to db, let do it
                                    }
                                }
                                else if (subChoice == 6) break;
                                else WriteLog("Wrong key !!");
                            }
                            catch (Exception)
                            {
                                WriteLogOneLine("Errr");
                            }
                        }
                    }
                    else if (choice == 2)
                    {
                        WriteLog("==================================");
                        WriteLog("    1.	Add new Lectures");
                        WriteLog("    2.	View all Lectures");
                        WriteLog("    3.	Search Lectures");
                        WriteLog("    4.	Delete Lectures");
                        WriteLog("    5.	Update Lectures");
                        WriteLog("    6.	Back to main menu");
                        WriteLog("==================================");
                        int subChoice = int.Parse(Console.ReadLine());
                        Console.Clear();
                        if (subChoice == 1)
                        {
                            Guid id = Guid.NewGuid();
                            WriteLogOneLine("Name: ");
                            string Name = Console.ReadLine();
                            WriteLogOneLine("Dob: ");
                            DateTime Dob = DateTime.Parse(Console.ReadLine());
                            WriteLogOneLine("Email: ");
                            string Email = Console.ReadLine();
                            WriteLogOneLine("Address: ");
                            string Address = Console.ReadLine();
                            WriteLogOneLine("Department: ");
                            string Department = Console.ReadLine();
                            lecturers.Add(new Lecturer(id, Name, Dob, Email, Address, Department));
                            // add director to db
                            Create(Name, Dob, Email, Address, Department);
                            WriteLog("Add Successfully");
                        }
                        else if (subChoice == 2)
                        {
                            WriteLog("====\tList Students\t===");
                            foreach (Lecturer lecturer in lecturers)
                            {
                                WriteLog(lecturer.ToString());
                                WriteLog("==========================");
                                WriteLog();
                            }
                        }
                        else if (subChoice == 3)
                        {
                            WriteLog("====\tLecturer\t===");
                            WriteLog("Name or Id to Search ...");
                            string search = Console.ReadLine();
                            var containLecture = lecturers.Where(x => x.Name.Contains(search) || x.Id.Equals(search));
                            foreach (Lecturer lecturer in containLecture)
                            {
                                WriteLog(lecturer.ToString());
                                WriteLog("==========================");
                                WriteLog();
                            }
                        }
                        else if (subChoice == 4)
                        {
                            WriteLogOneLine("Id: ");
                            Guid lectureId = Guid.Parse(Console.ReadLine());
                            var deleteLecture = students.Find(x => x.Id.Equals(lectureId));
                            if (deleteLecture == null) WriteLog("Can't find anyone");
                            else
                            {
                                students.Remove(deleteLecture);
                                //Remove to database
                                Delete(lectureId);
                                WriteLog("Delete Successfully");
                            }
                        }
                        else if (subChoice == 5)
                        {
                            WriteLogOneLine("Id: ");
                            Guid updateId = Guid.Parse(Console.ReadLine());
                            var updateLecture = students.Find(x => x.Id.Equals(updateId));
                            if (updateLecture == null) WriteLog("Can't find anyone");
                            else
                            {
                                WriteLogOneLine("Name: ");
                                updateLecture.Name = Console.ReadLine();
                                WriteLogOneLine("Dob: ");
                                updateLecture.Dob = DateTime.Parse(Console.ReadLine());
                                WriteLogOneLine("Email: ");
                                updateLecture.Email = Console.ReadLine();
                                WriteLogOneLine("Address: ");
                                updateLecture.Address = Console.ReadLine();
                                WriteLogOneLine("Batch: ");
                                updateLecture.Batch = Console.ReadLine();
                                WriteLog("Update Successfully");
                                Update(updateId, updateLecture);
                                // If you want update to db, let do it
                            }
                        }
                    }
                    else if (choice == 6) break;
                    else WriteLog("Wrong key !!");
                }
                catch (ReBuildException e)
                {
                    WriteLog(e.BadException("Have errr"));
                }
            }
        }
    }

    public class ReBuildException : Exception
    {
        public string BadException(string data)
        {
            return "Customer Err:" + data;
        }
    }
}