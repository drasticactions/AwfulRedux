﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace AwfulRedux.Database.Tools
{
    public class Repository<T> where T : new()
    {
        private readonly SQLiteAsyncConnection _db;
        public Repository(SQLiteAsyncConnection db)
        {
            _db = db;
        }

        public AsyncTableQuery<T> Items() => _db.Table<T>();

        public async Task<List<T>> GetAllWithChildren()
        {
            return await _db.GetAllWithChildrenAsync<T>();
        }

        public async Task<int> Create(T newEntity)
        {
            return await _db.InsertAsync(newEntity);
        }

        public async Task CreateAllWithChildren(List<T> newEntity)
        {
            await _db.InsertAllWithChildrenAsync(newEntity);
        }

        public async Task CreateWithChildren(T newEntity)
        {
            await _db.InsertWithChildrenAsync(newEntity);
        }

        public async Task RemoveAll(IEnumerable<T> objects)
        {
            await _db.DeleteAllAsync(objects);
        }

        public async Task Remove(T newEntity)
        {
            await _db.DeleteAsync(newEntity);
        }

        public async Task<int> Update(T entity)
        {
            return await _db.UpdateAsync(entity);
        }

        public async Task UpdateWithChildren(T newEntity)
        {
            await _db.UpdateWithChildrenAsync(newEntity);
        }

        public async Task<int> Delete(T entity)
        {
            return await _db.DeleteAsync(entity);
        }
    }
}
