// Controllers/OrderDetailsController.cs
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories;

namespace FoodDeliveryApp.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Item> _itemRepository;

        public OrderDetailsController(IRepository<OrderDetail> orderDetailRepository, IRepository<Order> orderRepository, IRepository<Item> itemRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
        }

        public IActionResult Index()
        {
            return View(_orderDetailRepository.GetAll());
        }

        public IActionResult Create()
        {
            ViewBag.Orders = _orderRepository.GetAll();
            ViewBag.Items = _itemRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _orderDetailRepository.Add(orderDetail);
                _orderDetailRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Orders = _orderRepository.GetAll();
            ViewBag.Items = _itemRepository.GetAll();
            return View(orderDetail);
        }

        public IActionResult Edit(int id)
        {
            var orderDetail = _orderDetailRepository.GetById(id);
            if (orderDetail == null) return NotFound();
            ViewBag.Orders = _orderRepository.GetAll();
            ViewBag.Items = _itemRepository.GetAll();
            return View(orderDetail);
        }

        [HttpPost]
        public IActionResult Edit(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.OrdDetId) return NotFound();

            if (ModelState.IsValid)
            {
                _orderDetailRepository.Update(orderDetail);
                _orderDetailRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Orders = _orderRepository.GetAll();
            ViewBag.Items = _itemRepository.GetAll();
            return View(orderDetail);
        }

        public IActionResult Delete(int id)
        {
            var orderDetail = _orderDetailRepository.GetById(id);
            if (orderDetail == null) return NotFound();
            return View(orderDetail);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var orderDetail = _orderDetailRepository.GetById(id);
            _orderDetailRepository.Delete(orderDetail);
            _orderDetailRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}