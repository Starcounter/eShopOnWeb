using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Infrastructure.Data.Starcounter
{
    public class StarcounterRepository<T> : IRepository<T>, IAsyncRepository<T> where T : BaseEntity
    {
        public T GetById(ulong id)
        {
            return Db.Transact(() => {
                var dbObject = Db.FromId<T>(id);
                if (dbObject == null)
                {
                    return null;
                }
                return DbObjectToPoco(dbObject);
            });
        }

        public T GetSingleBySpec(ISpecification<T> spec)
        {
            return List(spec).FirstOrDefault();
        }

        public IEnumerable<T> ListAll()
        {
            return QueryableToPocos(DbLinq.Objects<T>());
        }

        public IEnumerable<T> List(ISpecification<T> spec)
        {
            return QueryableToPocos(DbLinq.Objects<T>()
                .Where(spec.Criteria));
        }

        //todo: returning DB-T makes no sense, as it can't be manipulated outside of a transaction
        public T Add(T entity)
        {
            return Db.Transact(() => Mapper.Map(entity, Db.Insert<T>()));
        }
        
        public virtual void Update(T entity)
        {
            // todo
            // no-op?
        }

        public virtual void Delete(T basket)
        {
            Db.Transact(() => {
                Db.Delete(Db.FromId<T>(basket.IntId)); 
            });
        }

        public Task<T> GetByIdAsync(ulong id)
        {
            return Task.FromResult(GetById(id));
        }

        public Task<List<T>> ListAllAsync()
        {
            return Task.FromResult(ListAll().ToList());
        }

        public Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            return Task.FromResult(List(spec).ToList());
        }

        public Task<T> AddAsync(T entity)
        {
            return Task.FromResult(Add(entity));
        }

        public Task UpdateAsync(T entity)
        {
            Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            Delete(entity);
            return Task.CompletedTask;
        }
        private IEnumerable<T> QueryableToPocos(IQueryable<T> queryable)
        {
            return Db.Transact(() => queryable
                .ToList()
                .Select(DbObjectToPoco)
                .ToList());
        }

        protected virtual T DbObjectToPoco(T dbObject)
        {
            var poco = Mapper.Map<T>(dbObject);
            poco.IntId = dbObject.GetObjectNo();
            return poco;
        }
    }
}