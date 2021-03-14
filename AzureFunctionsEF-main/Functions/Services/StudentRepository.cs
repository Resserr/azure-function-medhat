using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Models;
using Functions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Functions.Services
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddStudentAsync(Student student)
        {
            if(student == null)
                throw new InvalidOperationException("Student object is empty");
            await _context.Students.AddAsync(student); // Adding new Student to the database
            await _context.SaveChangesAsync();  // Save changes to the database
        }

        public async Task DeleteStudentAsync(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
                throw new InvalidOperationException("Can't find user.");
            // First of all, we need to get user from the database, so we can delete him or her later
            Student student = await GetProperStudentAsync(studentId); 
            if (student == null)
                throw new InvalidOperationException("Can't delete user. User doesn't exist in the database");

            _context.Students.Remove(student); //Remove student from the database if they exist
            await _context.SaveChangesAsync();
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Students.OrderBy(s => s.School).ToListAsync();
        }

        public async Task<Student> GetStudentAsync(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
                throw new InvalidOperationException();

            return await GetProperStudentAsync(studentId);
        }

        public async Task UpdateStudentAsync(Student student)
        {
            if (student == null)
                throw new InvalidOperationException("Student object is empty");
            // First of all, we need to get user from the database, so we can delete him or her later
            Student currentStudent = await GetProperStudentAsync(student.StudentId);
            if(currentStudent == null)
                throw new InvalidOperationException("Can't find user.");

            // Updating student data
            currentStudent.FirstName = student.FirstName;
            currentStudent.LastName = student.LastName;
            currentStudent.School = student.School;

            await _context.SaveChangesAsync(); // Saving new data
        }

        // Encapsulate getting proper Student from database process
        private async Task<Student> GetProperStudentAsync(string studentId)
        {
            return await _context.Students.Where(student => student.StudentId == studentId).FirstOrDefaultAsync();
        }
    }
}