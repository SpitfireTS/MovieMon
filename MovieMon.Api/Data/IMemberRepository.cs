using System;
using System.Collections.Generic;
using MovieMon.Api.Models;

namespace MovieMon.Api.Data
{
    public interface IRepositoryBase<TModel>
    {
        IEnumerable<TModel> GetAll();
        Member GetById(Guid id);
        Member Add(TModel member);
        void Remove(int id);
        bool Update(TModel member);
    }

    public interface IMemberRepository : IRepositoryBase<Member>
    {

    }
}