﻿namespace Catalog.Host.Services.Interfaces
{
    public interface ICatalogBrandService
    {
        Task<int?> Add(string brand);
        Task<int?> Remove(int id);
        Task<int?> Update(int id, string brand);
    }
}
