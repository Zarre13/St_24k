using Automagazin.Data;
using Automagazin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automagazin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutomagazinController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Cars_Stock>> GetAll()
    {
        return Ok(context.Cars_Stock.AsNoTracking().ToList());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Cars_Stock> GetById(int id)
    {
        var car = context.Cars_Stock.Find(id);
        return car is null ? NotFound() : Ok(car);
    }

    [HttpPost]
    public ActionResult<Cars_Stock> Create(Cars_Stock car)
    {
        if (context.Cars_Stock.Any(existing => existing.Car_name == car.Car_name))
        {
            ModelState.AddModelError(
                nameof(car.Car_name),
                "Машина с таким названием уже существует.");
            return ValidationProblem(ModelState);
        }

        context.Cars_Stock.Add(car);
        context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Cars_Stock updated)
    {
        var car = context.Cars_Stock.Find(id);
        if (car is null)
        {
            return NotFound();
        }

        car.Car_name = updated.Car_name;
        car.Amount_of_car_in_stock = updated.Amount_of_car_in_stock;
        car.Company_name = updated.Company_name;
        car.Price = updated.Price;
        car.Production_date = updated.Production_date;

        context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var car = context.Cars_Stock.Find(id);
        if (car is null)
        {
            return NotFound();
        }

        context.Cars_Stock.Remove(car);
        context.SaveChanges();
        return NoContent();
    }

    [HttpGet("filter")]
    public ActionResult<IEnumerable<Cars_Stock>> Filter([FromQuery] Cars_Stock_Filter filter)
    {
        var query = context.Cars_Stock.AsNoTracking().AsQueryable();

        if (filter.Car_name is not null)
        {
            query = query.Where(car => car.Car_name == filter.Car_name);
        }

        if (filter.minPrice.HasValue)
        {
            query = query.Where(car => car.Price >= filter.minPrice.Value);
        }

        if (filter.maxPrice.HasValue)
        {
            query = query.Where(car => car.Price <= filter.maxPrice.Value);
        }

        return Ok(query.ToList());
    }
}
