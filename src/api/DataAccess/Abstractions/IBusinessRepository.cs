﻿using DataAccess.Entities;

namespace DataAccess.Abstractions
{
    public interface IBusinessRepository : IRepository<Business, int>
    {
        public Task<Business> GetBusinessIncludingAll(int id);

        public Task<Business> GetUserBusinessIncludingAll(string userId);
    }
}
