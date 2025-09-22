using Application.Common;
using Application.Common.DTOs.Classroom;
using Application.Common.DTOs.User;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class ClassroomService : IClassroomService
    {

        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClassroomService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<bool>> AddUserToClassroomAsync(Guid classroomId, Guid userId)
        {
            try
            {
                var classroom = await _context.Classrooms.FindAsync(classroomId);
                if (classroom == null)
                {
                    return Result<bool>.Fail("Classroom not found");
                }
                var user = await _context.Students.Include(x => x.StudentClassrooms).Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (user == null)
                {
                    return Result<bool>.Fail("Student not found");
                }

                if (user.StudentClassrooms.Any(x=>x.Id == userId))
                {
                    return Result<bool>.Fail("Student is already in this classroom");
                }

                user.StudentClassrooms.Add(classroom);
                _context.Students.Update(user);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"Cannot add student to classroom: {ex.Message}");
            }
        }

        public async Task<Result<bool>> RemoveUserFromClassroomAsync(Guid classroomId, Guid userId)
        {
            try
            {
                var classroom = await _context.Classrooms.FindAsync(classroomId);
                if (classroom == null)
                {
                    return Result<bool>.Fail("Classroom not found");
                }
                var user = await _context.Students.Include(x => x.StudentClassrooms).Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (user == null)
                {
                    return Result<bool>.Fail("Student not found");
                }

                if (!user.StudentClassrooms.Any(x=>x.Id == userId))
                {
                    return Result<bool>.Fail("Student is not in this classroom");
                }
                user.StudentClassrooms.Remove(classroom);
                _context.Students.Update(user);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"Cannot remove student from classroom: {ex.Message}");
            }
        }

        public async Task<Result<bool>> CreateClassroomAsync(string name)
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Result<bool>.Fail("User not authenticated");

                var classroom = new Classroom
                {
                    Name = name,
                    TeacherId = Guid.Parse(userIdClaim)
                };

                await _context.Classrooms.AddAsync(classroom);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"Cannot create classroom: {ex.Message}");
            }
        }


        public async Task<Result<bool>> DeleteClassroomAsync(Guid classroomId)
        {
            try
            {
                var classroom = await _context.Classrooms.FindAsync(classroomId);
                if (classroom == null)
                {
                    return Result<bool>.Fail("Classroom not found");
                }
                _context.Classrooms.Remove(classroom);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"Cannot delete classroom: {ex.Message}");
            }
        }

        public async Task<Result<GetClassroomDto>> GetClassroomByIdAsync(Guid classroomId)
        {
            try
            {
                var classroom = await _context.Classrooms.Include(x => x.Students).Include(x => x.Teacher)
                   .Where(x=>x.Id == classroomId).AsNoTracking().FirstOrDefaultAsync();

                if (classroom == null)
                {
                    return Result<GetClassroomDto>.Fail("Classroom not found")!;
                }
                var classroomDto = new GetClassroomDto
                {
                    Id = classroom.Id,
                    Name = classroom.Name,
                    Teacher = classroom.Teacher != null ? new GetUserDto
                    {
                        Id = classroom.Teacher.Id,
                        Name = classroom.Teacher.Name,
                        Surname = classroom.Teacher.Surname,
                        Email = classroom.Teacher.Email
                    } : null!,
                    Students = classroom.Students != null ? classroom.Students.Select(s => new GetUserDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Surname = s.Surname,
                        Email = s.Email
                    }).ToList() : new List<GetUserDto>()
                };
                return Result<GetClassroomDto>.Success(classroomDto);
            }
            catch (Exception ex)
            {
                return Result<GetClassroomDto>.Fail($"Cannot get classroom details: {ex.Message}")!;
            }
        }

        public async Task<Result<bool>> UpdateClassroomAsync(Guid classroomId, string name)
        {
            try
            {
                var classroom = await _context.Classrooms.FindAsync(classroomId);
                if (classroom == null)
                {
                    return Result<bool>.Fail("Classroom not found");
                }
                classroom.Name = name;
                _context.Classrooms.Update(classroom);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"Cannot update classroom: {ex.Message}");
            }
        }
    }
}
