﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using vudly.Models;
using vudly.ViewModels;

namespace vudly.Controllers
{
    public class CustomersController : Controller
    {

        private ApplicationDbContext _context;


        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();

        }

        // GET: Customers
        public ActionResult Index()
        {
            var customer = _context.Customer.Include(c =>c.MembershipType).ToList() ;
           
            return View(customer);
        }

        

        public ActionResult Details(int id)
        {
            var customer = _context.Customer.Include(c =>c.MembershipType).SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);

        }

        public ActionResult New()
        {
            var MembershipType = _context.MembershipType.ToList();
            var viewModel = new CustomerFormViewModel
            {
                MembershipTypes = MembershipType
            };

            return View("CustomerForm",viewModel);
        }
        [HttpPost]
        public ActionResult Save( Customer customer)
        {
            if (customer.Id == 0)
            {
                _context.Customer.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customer.Single(c => c.Id == customer.Id);

                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipType = customer.MembershipType;
                customerInDb.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;

            }         
            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");

           
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customer.SingleOrDefault(c => c.Id == id);
            if (customer ==  null)
            {
                return HttpNotFound();
            }
            var viewModel = new CustomerFormViewModel {
                Customers = customer,
                MembershipTypes = _context.MembershipType.ToList()
            };
            return View("CustomerForm", viewModel);
        }
    }
}