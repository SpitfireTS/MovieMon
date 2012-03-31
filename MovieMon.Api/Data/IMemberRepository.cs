using System.Collections.Generic;
using MovieMon.Api.Models;

namespace MovieMon.Api.Data
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAll();
        Member GetById(string id);
        Member Add(Member member);
        void Remove(int id);
        bool UpdateUser(Member member);
    }
}