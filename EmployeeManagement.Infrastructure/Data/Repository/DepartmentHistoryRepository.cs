using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Data.Repository
{
    public class DepartmentHistoryRepository : IRepository<DepartmentHistory>
    {
        private readonly EmployeeContext _context;

        public DepartmentHistoryRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepartmentHistory>> GetAllAsync()
        {
            return await _context.DepartmentHistories.ToListAsync();
        }

        public async Task<IEnumerable<DepartmentHistory>> GetAllAsync(Expression<Func<DepartmentHistory, bool>> predicate)
        {
            return await _context.DepartmentHistories.Where(predicate).ToListAsync();
        }

        public async Task<DepartmentHistory> GetByIdAsync(int id)
        {
            return await _context.DepartmentHistories.FindAsync(id);
        }

        public async Task AddAsync(DepartmentHistory entity)
        {
            await _context.DepartmentHistories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DepartmentHistory entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DepartmentHistory entity)
        {
            _context.DepartmentHistories.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}