using MajorRequestServer.Dto;
using MajorRequestServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace MajorRequestServer.Repository.Abstractions
{
    public interface IRepository<T> where T : BaseModel
    {

        Task<bool> CreateAsync(T model);

        Task<bool> UpdateAsync(T model);

        Task<bool> UpdateRequestAsync(int id, int statusID, int courierID = 0);

        Task<IList<T>> GetValuesAsync();

        Task<IList<RequestDto>> GetAllRequestAsync();
        Task<IList<RequestDto>> GetFindRequestAsync(string findStr);
        Task<bool> DeleteRequestAsync(int id);
    }
}
