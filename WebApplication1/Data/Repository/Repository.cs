﻿using WebApplication1.Data.AbstractClasses;
using WebApplication1.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication1.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected AppDBContext Context { get; set; }
        private DbSet<T> EntitySet => Context.Set<T>();

        public Repository(AppDBContext appDBContext)
        {
            Context = appDBContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity; 
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await EntitySet.Where(predicate).CountAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            EntitySet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await EntitySet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await EntitySet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }
    }
}