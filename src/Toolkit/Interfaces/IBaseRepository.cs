﻿using Toolkit.Data;

namespace Toolkit.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<long> CountAsync();
    Task<TEntity> AddAsync(TEntity entity, bool applySave = true);
    Task<TEntity> UpdateAsync(TEntity entity, bool applySave = true);
    Task<bool> DeleteAsync(int id, bool applySave = true);
    Task<TEntity> GetObjectByIDAsync(int id);
    Task<IEnumerable<TEntity>> GetAsync(int limit = 50, int start = 0);
    Task<bool> SaveChangesAsync();
}