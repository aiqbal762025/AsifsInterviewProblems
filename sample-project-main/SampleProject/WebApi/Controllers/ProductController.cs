using BusinessEntities.Models;
using Core.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ProductController : ApiController
    {
        private static readonly ProductService _service = new ProductService();

        [HttpGet]
        [Route("products/getAll")]
        public IHttpActionResult GetAll()
        {
            return Ok(_service.GetAll(null));
        }

        [HttpGet]
        [Route("products/{id:guid}/getById")]
        public IHttpActionResult GetById(Guid id)
        {
            var item = _service.GetById(id);
            return Ok(item);
        }

        [HttpPost]
        [Route("products/create")]
        public IHttpActionResult Create([FromBody] Product product)
        {
            _service.Add(product);
            return Ok(product);
        }

        [HttpPut]
        [Route("products/{id:guid}/update")]
        public IHttpActionResult Update(Guid id, [FromBody] Product product)
        {
            product.Id = id;
            _service.Update(product);
            return Ok(product);
        }

        [HttpDelete]
        [Route("products/{id:guid}/delete")]
        public IHttpActionResult Delete(Guid id)
        {
            _service.Delete(id);
            return Ok("Item Deleted");
        }
    }
}
