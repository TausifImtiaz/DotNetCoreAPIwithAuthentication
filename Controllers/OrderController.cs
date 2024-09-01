using DotNetCoreAPIwithAuthentication.DAL;
using DotNetCoreAPIwithAuthentication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace DotNetCoreAPIwithAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private readonly MyDbContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public OrderController(MyDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _dbContext.OrderMasters.Include(o => o.OrderDetail).ToList();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrder(int orderId)
        {
            var order = _dbContext.OrderMasters.Include(o => o.OrderDetail).FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder()
        {
            if (ModelState.IsValid)
            {
                var order = new OrderMaster
                {
                    CustomerName = HttpContext.Request.Form["CustomerName"],
                    OrderDate = DateTime.Parse(HttpContext.Request.Form["OrderDate"]),
                    IsComplete = bool.Parse(HttpContext.Request.Form["IsComplete"])
                };

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];
                if (imageFile != null)
                {
                    var filename = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var path = Path.Combine(_hostEnvironment.WebRootPath ?? string.Empty, "images", filename);

                    order.ImagePath = path;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                }

                string orderDetailJson = HttpContext.Request.Form["OrderDetail"];
                if (!string.IsNullOrEmpty(orderDetailJson))
                {
                    var orderDetailList = JsonConvert.DeserializeObject<List<OrderDetail>>(orderDetailJson);
                    order.OrderDetail.AddRange(orderDetailList);
                }

                _dbContext.OrderMasters.Add(order);
                await _dbContext.SaveChangesAsync();

                var allOrders = _dbContext.OrderMasters.Include(o => o.OrderDetail).ToList();
                return Ok(allOrders);
            }

            return BadRequest(ModelState);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOrder(int id, [FromForm] OrderMaster order)
        //{
        //    var existingOrder = _dbContext.OrderMasters.Include(o => o.OrderDetail).FirstOrDefault(o => o.OrderId == id);
        //    if (existingOrder == null)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        existingOrder.CustomerName = HttpContext.Request.Form["CustomerName"];
        //        existingOrder.OrderDate = DateTime.Parse(HttpContext.Request.Form["OrderDate"]);
        //        existingOrder.IsComplete = bool.Parse(HttpContext.Request.Form["IsComplete"]);

        //        IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];
        //        if (imageFile != null)
        //        {
        //            var filename = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        //            var path = Path.Combine(_hostEnvironment.WebRootPath ?? string.Empty, "images", filename);

        //            existingOrder.ImagePath = path;
        //            using (var stream = new FileStream(path, FileMode.Create))
        //            {
        //                await imageFile.CopyToAsync(stream);
        //            }
        //        }

        //        var orderDetailJson = HttpContext.Request.Form["OrderDetail"];
        //        if (!string.IsNullOrEmpty(orderDetailJson))
        //        {
        //            var updatedOrderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(orderDetailJson);

        //            foreach (var updatedOrderDetail in updatedOrderDetails)
        //            {
        //                var existingOrderDetail = existingOrder.OrderDetail.FirstOrDefault(od => od.DetailId == updatedOrderDetail.DetailId);
        //                if (existingOrderDetail != null)
        //                {
        //                    existingOrderDetail.PlantId = updatedOrderDetail.PlantId;
        //                    existingOrderDetail.Quantity = updatedOrderDetail.Quantity;
        //                    existingOrderDetail.Price = updatedOrderDetail.Price;
        //                }
        //                else
        //                {
        //                    var newOrderDetail = new OrderDetail
        //                    {
        //                        PlantId = updatedOrderDetail.PlantId,
        //                        Quantity = updatedOrderDetail.Quantity,
        //                        Price = updatedOrderDetail.Price,
        //                    };
        //                    existingOrder.OrderDetail.Add(newOrderDetail);
        //                }
        //            }
        //        }

        //        await _dbContext.SaveChangesAsync();
        //        return Ok(existingOrder);
        //    }

        //    return BadRequest(ModelState);
        //}


        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, [FromForm] OrderMaster order)
        {
            var existingOrder = _dbContext.OrderMasters
                .Include(o => o.OrderDetail)
                .FirstOrDefault(o => o.OrderId == id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                existingOrder.CustomerName = HttpContext.Request.Form["CustomerName"];
                existingOrder.OrderDate = DateTime.Parse(HttpContext.Request.Form["OrderDate"]);
                existingOrder.IsComplete = bool.Parse(HttpContext.Request.Form["IsComplete"]);

                IFormFile imageFile = HttpContext.Request.Form.Files["ImageFile"];
                if (imageFile != null)
                {
                    var filename = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var path = Path.Combine(_hostEnvironment.WebRootPath ?? string.Empty, "images", filename);

                    existingOrder.ImagePath = path;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                }

                var orderDetailJson = HttpContext.Request.Form["OrderDetail"];
                if (!string.IsNullOrEmpty(orderDetailJson))
                {
                    var updatedOrderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(orderDetailJson);

                    // Remove order details that are not present in the updated order details
                    var orderDetailIdsToRemove = existingOrder.OrderDetail
                        .Select(od => od.DetailId)
                        .Except(updatedOrderDetails.Select(od => od.DetailId))
                        .ToList();

                    foreach (var detailId in orderDetailIdsToRemove)
                    {
                        var detailToRemove = existingOrder.OrderDetail.FirstOrDefault(od => od.DetailId == detailId);
                        if (detailToRemove != null)
                        {
                            _dbContext.OrderDetails.Remove(detailToRemove);
                        }
                    }

                    // Update or add order details
                    foreach (var updatedOrderDetail in updatedOrderDetails)
                    {
                        var existingOrderDetail = existingOrder.OrderDetail.FirstOrDefault(od => od.DetailId == updatedOrderDetail.DetailId);
                        if (existingOrderDetail != null)
                        {
                            // Update existing order detail
                            existingOrderDetail.PlantId = updatedOrderDetail.PlantId;
                            existingOrderDetail.Quantity = updatedOrderDetail.Quantity;
                            existingOrderDetail.Price = updatedOrderDetail.Price;
                        }
                        else
                        {
                            // Add new order detail
                            var newOrderDetail = new OrderDetail
                            {
                                PlantId = updatedOrderDetail.PlantId,
                                Quantity = updatedOrderDetail.Quantity,
                                Price = updatedOrderDetail.Price,
                            };
                            existingOrder.OrderDetail.Add(newOrderDetail);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
                return Ok(existingOrder);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = _dbContext.OrderMasters.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound();
            }

            _dbContext.OrderMasters.Remove(order);
            await _dbContext.SaveChangesAsync();

            var allOrders = _dbContext.OrderMasters.Include(o => o.OrderDetail).ToList();
            return Ok(allOrders);
        }
    }
}
