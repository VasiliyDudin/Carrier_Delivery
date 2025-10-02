using MajorRequestServer.Database;
using MajorRequestServer.Dto;
using MajorRequestServer.Models;
using MajorRequestServer.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using Request = MajorRequestServer.Models.Request;

namespace MajorRequestServer.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseModel
    {
        private RequestContext _context { get; set; }
        public BaseRepository(RequestContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Универсальный метод для записи данных в таблицы БД
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Результат выполнения операции</returns>
        public async Task<bool> CreateAsync(T model)
        {
            bool result = true;

            try
            {
                await _context.Set<T>().AddAsync(model);
                result = await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Универсальный метод для обновления данных в таблицах БД
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Результат выполнения операции</returns>
        public async Task<bool> UpdateAsync(T model)
        {
            bool result = true;

            try
            {
                _context.Set<T>().Update(model);
                result = await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Метод для асинхронного обновления статуса и курьера в заявках
        /// </summary>
        /// <param name="id">Номер заявки</param>
        /// <param name="statusID">Стаус заявки из таблицы Status</param>
        /// <param name="courierID">ID курьера на которого была распределена заявка</param>
        /// <returns>Результат выполнения операции</returns>
        public async Task<bool> UpdateRequestAsync(int id, int statusID, int courierID = 0)
        {
            bool result = false;

            Request? request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return result;
            }

            request.StatusID = statusID > 0 ? statusID : 0;

            if(courierID > 0)
                request.CourierID = courierID;

            try
            {
                result = await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Универсальный метод для получения значений из таблиц в БД
        /// </summary>
        /// <returns></returns>
        public async Task<IList<T>> GetValuesAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Метод для асинхронного получения всех необходимых значений для клиента
        /// Возвращает список RequestDto
        /// </summary>
        /// <returns></returns>
        public async Task<IList<RequestDto>> GetAllRequestAsync()
        {
            string query = "select r.ID,r.StatusID,st.StatusName,r.CourierID,r.ClientFIO,cr.FIO,cr.Rating,r.Address,r.Text,r.CanceledText " +
                           "from Request r left join Status st on st.ID = r.StatusID left join Courier cr on cr.ID = r.CourierID";

            var requestDtos = await _context.RequestDtos
                .FromSqlRaw(query)
                .AsNoTracking()
                .OrderBy(x => x.ID)
                .ToListAsync();

            return requestDtos;
        }

        /// <summary>
        /// Метод для получение всех найденных заявок по переданному параметру для поиска
        /// </summary>
        /// <param name="findStr"></param>
        /// <returns>Возвращает отфильтрованный список по параметрам из RequestDto</returns>
        public async Task<IList<RequestDto>> GetFindRequestAsync(string findStr)
        {
            List<RequestDto> findRequestDtos = new List<RequestDto>();

            var requestDtos = await GetAllRequestAsync();

            if (requestDtos != null && requestDtos.Count > 0)
            {
                foreach (RequestDto r in requestDtos)
                {
                    if (r.Address.Contains(findStr) || r.FIO.Contains(findStr) || r.StatusName.Contains(findStr) || r.ClientFIO.Contains(findStr) || r.FIO.Contains(findStr) || r.Rating.ToString().Equals(findStr) || r.Text.Contains(findStr) || r.CanceledText.Contains(findStr))
                    {
                        findRequestDtos.Add(r);
                    }
                }
            }

            return findRequestDtos;
        }

        /// <summary>
        /// Удаление заявки по ID из БД
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Результат выполнения операции</returns>
        public async Task<bool> DeleteRequestAsync(int id)
        {
            bool result = false;
            Request? request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return result;
            }

            try 
            {
                _context.Requests.Remove(request);
                result = await _context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}