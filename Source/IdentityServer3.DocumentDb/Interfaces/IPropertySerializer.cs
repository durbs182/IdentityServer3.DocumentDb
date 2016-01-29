﻿using System.Threading.Tasks;

namespace IdentityServer3.DocumentDb.Interfaces
{
    public interface IPropertySerializer
    {
        Task<T> Deserialize<T>(string propertyString);
        Task<string> Serialize<T>(T propertyValue);
    }
}