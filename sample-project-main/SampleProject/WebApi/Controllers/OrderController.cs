using BusinessEntities.Models;
using Core.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class OrderController : ApiController
    {
        private static readonly OrderService _service = new OrderService();

        [HttpGet]
        [Route("orders/getAll")]
        public IHttpActionResult GetAll()
        {
            return Ok(_service.GetAll(null));
        }

        [HttpGet]
        [Route("orders/{id:guid}/getById")]
        public IHttpActionResult GetById(Guid id)
        {
            var item = _service.GetById(id);
            return Ok(item);
        }

        [HttpPost]
        [Route("orders/create")]
        public IHttpActionResult Create([FromBody] Order order)
        {
            _service.Add(order);
            return Ok(order);
        }

        [HttpPut]
        [Route("orders/{id:guid}/update")]
        public IHttpActionResult Update(Guid id, [FromBody] Order order)
        {
            order.Id = id;
            _service.Update(order);
            return Ok(order);
        }

        [HttpDelete]
        [Route("orders/{id:guid}/delete")]
        public IHttpActionResult Delete(Guid id)
        {
            _service.Delete(id);
            return Ok("Item Deleted");
        }
    }
}
