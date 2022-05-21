using Microsoft.AspNetCore.Mvc;
using Car_API.Models;

namespace Car_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        CarAPIContext context = new CarAPIContext();

        [HttpGet("FullCarList")]
        public List<Car> GetCars()
        {
            return context.Cars.ToList();
        }

        [HttpGet("GetCarAtId/{id}")]
        public Car GetCar(int id)
        {
            try
            {
                if (id > 0 && id <= context.Cars.Count())
                {
                    Car output = context.Cars.Find(id);
                    return output;
                }
                else
                {
                    Car c = new Car();
                    c.Make = $"No Car with id {id} was found. Please input an id between 1 and {context.Cars.Count()}";
                    return c;
                }
            }
            catch (Exception ex)
            {
                Car c = new Car();
                c.Make = ex.Message;
                return c;
            }
        }

        [HttpGet("SearchByMake/{make}")]
        public List<Car> SearchByMake(string make)
        {
            List<Car> results = context.Cars.Where(c => c.Make == make).ToList();
            return results;
        }

        [HttpGet("SearchByModel/{model}")]
        public List<Car> SearchByModel(string model)
        {
            List<Car> results = context.Cars.Where(c => c.Model == model).ToList();
            return results;
        }

        [HttpGet("SearchByYear/{year}")]
        public List<Car> SearchByYear(int year)
        {
            List<Car> results = context.Cars.Where(c => c.Year == year).ToList();
            return results;
        }

        [HttpGet("SearchByColor/{color}")]
        public List<Car> SearchByColor(string color)
        {
            List<Car> results = context.Cars.Where(c => c.Color == color).ToList();
            return results;
        }

        //[HttpGet("SearchByAny/{?+=?}")]
        //public List<Car> SearcyByAny(string input)
        //{
        //    List<Car> results = context.Cars.Where(c => c.Make == input || c.Model == input || c.Year == input || c.Color == input).ToList();
        //    return results;
        //}

        //==========CRUD FUNCTIONS BELOW==========
        [HttpPost("Create")]
        public void CreateCar(Car input)
        {
            context.Cars.Add(input);
            context.SaveChanges();
        }

        [HttpDelete("Delete/{id}")]
        public string DeleteCar(int id)
        {
            int initialCount = context.Cars.Count();

            Car toDelete = GetCar(id);
            try
            {
                context.Cars.Remove(toDelete);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                string errorOutput = e.Message;
                errorOutput += "\nNo changes were made to the database.";
                return errorOutput;
            }

            int finalCount = context.Cars.Count();

            return $"Initial Count: {initialCount}, Final Count: {finalCount}";
        }

        [HttpPut("Update/{id}")]
        public string UpdateCar(int id, Car updatedCar)
        {
            Car car = GetCar(id);

            car.Id = updatedCar.Id;
            car.Make = updatedCar.Make;
            car.Model = updatedCar.Model;
            car.Year = updatedCar.Year;
            car.Color = updatedCar.Color;

            context.Cars.Update(car);
            context.SaveChanges();

            return $"{updatedCar.Make} {updatedCar.Model} at id {updatedCar.Id} has been updated.";
        }

    }
}
