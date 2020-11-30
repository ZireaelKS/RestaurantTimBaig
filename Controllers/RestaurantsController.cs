﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantTimBaig.Domain.DB;
using RestaurantTimBaig.Domain.Model.Common;
using RestaurantTimBaig.Models;

namespace RestaurantTimBaig.Controllers
{
    [Route("{controller}")]
    public class RestaurantsController : Controller
    {

        private readonly RestaurantDBContext _restaurantDBContext;

        public RestaurantsController(RestaurantDBContext restaurantDBContext)
        {
            _restaurantDBContext = restaurantDBContext ?? throw new ArgumentNullException(nameof(restaurantDBContext));
        }

        /// <summary>
        /// Вывод списка ресторанов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ListRestaurants()
        {
            var restaurants = _restaurantDBContext.Restaurants
                .Select(x => new RestaurantViewModel
                {
                    Name = x.RestaurantName,
                    Address = x.RestaurantAddress
                }).OrderByDescending(x => x.Name);               
            return View(restaurants.ToList());
        }

        /// <summary>
        /// Вывод ресторана по id
        /// </summary>
        /// <param name="idRestaurant"></param>
        /// <returns></returns>
        [HttpGet("{idRestaurant}")]
        public IActionResult RestaurantPage(int idRestaurant)
        {
            var restaurants = _restaurantDBContext.Restaurants
                .Select(x => new RestaurantViewModel
                {
                    Name = x.RestaurantName,
                    Address = x.RestaurantAddress,
                    Phone = x.RestaurantPhone,
                    Description = x.RestaurantDescription,
                    Email = x.RestaurantEmail
                })
               ;

            return View(restaurants.ToList());
        }

        /// <summary>
        /// Добавление нового ресторана
        /// </summary>
        /// <param name="newRest"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddRestaurant(Restaurant data)
        {
            var rest = new Restaurant
            {
                RestaurantName = data.RestaurantName,
                RestaurantAddress = data.RestaurantAddress,
                RestaurantPhone = data.RestaurantPhone
            };
            _restaurantDBContext.Restaurants.Add(rest);
            _restaurantDBContext.SaveChanges();
            return Ok();
        }


        /// <summary>
        /// Удаление ресторана из списка
        /// </summary>
        /// <param name="id">Идентификатор ресторана</param>
        [HttpDelete("{id}/delete")]
        public IActionResult DeleteRestaurant(int id)
        {
            Restaurant rest = _restaurantDBContext.Restaurants.Find(id);
            try
            {
                _restaurantDBContext.Remove(rest);
                _restaurantDBContext.SaveChanges();
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Изменение данных ресторана (не работает, пока)
        /// </summary>
        /// <param name="rest"></param>
        /// <returns></returns>
        [HttpPost("updata")]
        public ActionResult EditBook(Restaurant rest)
        {
            _restaurantDBContext.Entry(rest).State = EntityState.Modified;
            _restaurantDBContext.SaveChanges();
            return Ok();
        }
    }
}