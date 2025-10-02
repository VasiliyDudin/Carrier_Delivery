using MajorRequestServer.Dto;
using MajorRequestServer.Models;
using MajorRequestServer.Repository;
using MajorRequestServer.Repository.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace MajorRequestServer.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RequestController : Controller
    {
        private BaseRepository<Request> _request { get; set; }
        private BaseRepository<Status> _status { get; set; }
        private BaseRepository<Courier> _courier { get; set; }

        public RequestController(BaseRepository<Request> request, BaseRepository<Status> status, BaseRepository<Courier> courier)
        {
            _request = request;
            _status = status;
            _courier = courier;
        }

        /// <summary>
        /// Получение значений из таблицы - Request
        /// </summary>
        /// <returns></returns>
        // GET api/Request/GetRequest
        [HttpGet]
        public async Task<List<Request>> GetRequestAsync()
        {
            IList <Request> resultList = new List <Request>();
            resultList = await _request.GetValuesAsync();

            return resultList.ToList();
        }

        /// <summary>
        /// Получение значений из таблицы - Status
        /// </summary>
        /// <returns></returns>
        // GET api/Request/GetStatus
        [HttpGet]
        public async Task<List<Status>> GetStatusAsync()
        {
            IList<Status> resultList = new List<Status>();
            resultList = await _status.GetValuesAsync();

            return resultList.ToList();
        }

        /// <summary>
        /// Получение значений из таблицы - Courier
        /// </summary>
        /// <returns></returns>
        // GET api/Request/GetCouriers
        [HttpGet]
        public async Task<List<Courier>> GetCouriersAsync()
        {
            IList<Courier> resultList = new List<Courier>();
            resultList = await _courier.GetValuesAsync();

            return resultList.ToList();
        }

        /// <summary>
        /// Получение всех значений по заявкам со значениями из связанных таблиц
        /// </summary>
        /// <returns></returns>
        // GET api/Request/GetAllRequest
        [HttpGet]
        public async Task<List<RequestDto>> GetAllRequestAsync()
        {
            IList<RequestDto> resultList = new List<RequestDto>();
            resultList = await _request.GetAllRequestAsync();

            return resultList.ToList();
        }

        /// <summary>
        /// Получение всех найденных заявок
        /// </summary>
        /// <returns></returns>
        // GET api/Request/GetFindRequest/
        [HttpGet]
        public async Task<List<RequestDto>> GetFindRequestAsync(string findStr)
        {
            IList <RequestDto> resultList = new List <RequestDto>();
            resultList = await _request.GetFindRequestAsync(findStr);

            return resultList.ToList();
        }

        /// <summary>
        /// Регистрация/Создание новой заявки
        /// </summary>
        /// <returns></returns>
        // POST: api/Request/Create
        [HttpPost]
        public async Task<ActionResult> CreateAsync(JsonElement collection)
        {
            Request request = new Request();

            var deserializedDto = JsonSerializer.Deserialize<RequestDto>(collection);
            if (deserializedDto == null)
                return BadRequest("Failed Deserialize");

            request.StatusID = deserializedDto.StatusID;
            request.ClientFIO = deserializedDto.ClientFIO;
            request.CourierID = deserializedDto.CourierID;
            request.Address = deserializedDto.Address;
            request.Text = deserializedDto.Text;
            request.CanceledText = deserializedDto.CanceledText;

            return await _request.CreateAsync(request) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Перевод заявки на выполнение (изменение статуса и курьера по заявке)
        /// </summary>
        /// <returns></returns>
        // PUT: api/Request/Edit/5
        [HttpPut]
        public async Task<ActionResult> EditAsync(int requestId, int statusID, int courierID)
        {
            return await _request.UpdateRequestAsync(requestId, statusID, courierID) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Редактирование полей с данными, если заявка находится в статусе «Новая»
        /// </summary>
        /// <returns></returns>
        // PUT: api/Request/Edit/5
        [HttpPut]
        public async Task<ActionResult> EditAll(JsonElement collection)
        {
            Request request = new Request();

            var deserializedDto = JsonSerializer.Deserialize<RequestDto>(collection);
            if (deserializedDto == null)
                return NoContent();

            if (deserializedDto.StatusID != 1)
                return ValidationProblem("Статус заявки не - Новая");

            request.ID = deserializedDto.ID;
            request.StatusID = deserializedDto.StatusID;
            request.ClientFIO = deserializedDto.ClientFIO;
            request.CourierID = deserializedDto.CourierID;
            request.Address = deserializedDto.Address;
            request.Text = deserializedDto.Text;
            request.CanceledText = deserializedDto.CanceledText;

            return await _request.UpdateAsync(request) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Удаление заявки по id
        /// </summary>
        /// <returns></returns>
        // DELETE: api/Request/Delete/5
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            return await _request.DeleteRequestAsync(id) ? Ok() : BadRequest();
        }
    }
}
