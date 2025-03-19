﻿namespace AzureTables.Data
{
    public interface IUserService
    {
        Task UpsertAsync(UserEntity user);

        Task DeleteAsync(string country, string id);

        Task<UserEntity> GetAsync(string country, string id);

        Task<List<UserEntity>> GetAllAsync();
    }
}
