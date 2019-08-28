using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using OdeToFood.Core;

namespace OdeToFood.Data
{
    public class OnDiskRestaurantData : IRestaurantData
    {
        private const string Output = "Data.json";
        List<Restaurant> restaurants;

        public OnDiskRestaurantData()
        {
            if (File.Exists(Output))
            {
                var json = File.ReadAllText(Output);
                restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(json);
            }
            else
            {
                restaurants = new List<Restaurant>()
                {
                    new Restaurant {Id = 1, Name = "Scotts Pizza", Location = "MaryLand", Cuisine = CuisineType.Italian },
                    new Restaurant { Id = 2, Name = "Melanas", Location = "Utah", Cuisine = CuisineType.Mexican},
                    new Restaurant {Id = 3, Name = "Bombay House", Location = "California", Cuisine = CuisineType.Indian}
                };
            }
        }

        public Restaurant GetById(int id)
        {
            return restaurants.SingleOrDefault(r => r.Id == id);
        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            restaurants.Add(newRestaurant);
            newRestaurant.Id = restaurants.Max(r => r.Id) + 1;
            return newRestaurant;
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var restaurant = restaurants.SingleOrDefault(r => r.Id == updatedRestaurant.Id);
            if (restaurant != null)
            {
                restaurant.Name = updatedRestaurant.Name;
                restaurant.Location = updatedRestaurant.Location;
                restaurant.Cuisine = updatedRestaurant.Cuisine;
            }

            return restaurant;
        }

        public int Commit()
        {
            var json = JsonConvert.SerializeObject(restaurants);
            File.WriteAllText(Output, json);
            return 0;
        }
        public IEnumerable<Restaurant> GetRestaurantsByName(string name = null)
        {
            return from r in restaurants
                   where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
                   orderby r.Name
                   select r;
        }

        public Restaurant Delete(int id)
        {
            var restaurant = restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant != null)
            {
                restaurants.Remove(restaurant);
            }
            return restaurant;
        }

        public int GetCountOfRestaurants()
        {
            return restaurants.Count();
        }
    }
}
