using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADO_Example.DLA;
using ADO_Example.Models;

namespace ADO_Example.Controllers
{
    public class ProductController : Controller
    {

       ProductDAL _productDAL = new ProductDAL();
        // GET: Product
        public ActionResult Index()
        {
            var productList= _productDAL.GetAllProdcuts();

            if(productList.Count==0)
            {
                TempData["InfoMessage"] = "Currently Products not available in tha database";
            }
            return View(productList);
        }

        // GET: Product/Details/5
        

        // GET: Product/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            bool IsInserted = false;

            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    IsInserted = _productDAL.InsertProducts(product);

                    if (IsInserted)
                    {
                        TempData["SuccessMessage"] = "Product Details Saved Successfully..!";
                    }
                    else
                    {
                        TempData["ErroMessaage"] = "Unable to save the product";
                    }

                }
                return RedirectToAction("Index");

            }
            catch(Exception ex)
            {
                TempData["ErroMessaage"] = ex.Message;
                return View();
            }
        
          
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var products=_productDAL.GetProdcutsByID(id).FirstOrDefault();
            if(products == null)
            {
                TempData["InfoMessage"] = "Product Not available with Id";
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // POST: Product/Edit/5
        [HttpPost,ActionName("Edit")]
        public ActionResult UpdateProduct(Product product)
        {
            bool IsUpdated=_productDAL.UpdateProducts(product); 
            try
            {
                if(IsUpdated)
                {
                    TempData["SuccessMessage"] = "Product details Updated";
                }
                else
                {
                    TempData["ErroMessage"] = "Product is already available/unable to update prodcut details";
                }
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ErroMessage"]=ex.Message;
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var product = _productDAL.GetProdcutsByID(id).FirstOrDefault();
                if (product == null)
                {
                    TempData["InfoMessage"] = "Prodcut not available with id" + id.ToString();
                    return RedirectToAction("Index");
                }

                return View(product);
            }
            catch(Exception ex)
            {
                TempData["ErroMessage"] = ex.Message;
                return View();
            }
            
            
        }

        // POST: Product/Delete/5
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {
                string result = _productDAL.DeleteProduct(id);
                if(result.Contains("deleted"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErroMessage"]=result;
                }
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ErroMessage"]=ex.Message;
                return View();
            }
        }
    }
}
