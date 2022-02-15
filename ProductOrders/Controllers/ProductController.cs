using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductOrders.Models.Database;

namespace ProductOrders.Controllers
{
    public class ProductController : Controller
    {
        private ProductsOrdersEntities _db;
        public ProductController( )
        {
            _db = new ProductsOrdersEntities();
        }
        // GET: Product
        [HttpGet]
        public ActionResult Index()
        {
            var products = _db.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = _db.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            var existingProduct = _db.Products.Find(product.Id);
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            _db.SaveChanges();
            return RedirectToAction("index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = _db.Products.Find(id);
            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var product = _db.Products.Find(id);
            return View(product);
        }
    }
}