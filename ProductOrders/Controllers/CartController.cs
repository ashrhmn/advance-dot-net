using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ProductOrders.Models;
using ProductOrders.Models.Database;

namespace ProductOrders.Controllers
{
    public class CartController : Controller
    {
        private ProductsOrdersEntities db;
        public CartController()
        {
            db = new ProductsOrdersEntities();
        }
        // GET: Cart
        [HttpGet]
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            ViewBag.products = products;
            if (Session["items"] == null)
            {
                Session["items"] = "[]";
            }
            List<CartItem> cartItems = new JavaScriptSerializer().Deserialize<List<CartItem>>((string)Session["items"]);
            return View(cartItems);
        }

        [HttpGet]
        public ActionResult SaveOrders()
        {
            if (Session["items"] == null || (string)Session["items"] == "[]")
            {
                TempData["message"] = "No items found to add";
                return RedirectToAction("Index");
            }

            List<OrderItem> orderItems = new List<OrderItem>();
            List<CartItem> cartItems = new JavaScriptSerializer().Deserialize<List<CartItem>>((string)Session["items"]);
            foreach (var cartItem in cartItems)
            {
                OrderItem order = new OrderItem()
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity
                };
                orderItems.Add(order);
            }

            db.OrderItems.AddRange(orderItems);
            db.SaveChanges();
            Session["items"] = "[]";
            TempData["message"] = "Products Added to order successfully";
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CartItem cartItem)
        {
            var product = db.Products.Find(cartItem.ProductId);
            if (product == null)
            {
                TempData["message"] = "Product not found with Product Id "+cartItem.ProductId;
                return RedirectToAction("Create");
            }

            if (Session["items"] == null)
            {
                Session["items"] = "[]";
            }
            List<CartItem> cartItems = new JavaScriptSerializer().Deserialize<List<CartItem>>((string)Session["items"]);
            if (cartItems.Any(item => item.ProductId == cartItem.ProductId))
            {
                var index = cartItems.FindIndex(item => item.ProductId == cartItem.ProductId);
                CartItem updatedItem = new CartItem()
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItems[index].Quantity + cartItem.Quantity
                };
                cartItems[index] = updatedItem;
            }
            else
            {
                cartItems.Add(cartItem);
            }
            var json = new JavaScriptSerializer().Serialize(cartItems);
            Session["items"] = json;
            return RedirectToAction("Index");
        }
    }
}