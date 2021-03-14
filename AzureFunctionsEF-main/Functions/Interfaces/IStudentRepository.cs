using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Models;

namespace Functions.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(string studentId);
        Task DeleteStudentAsync(string studentId);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
    }
}