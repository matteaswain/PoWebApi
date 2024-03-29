﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoWebApi.Data;
using PoWebApi.Models;

namespace PoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
// inheritance from ControllerBase// 
    {
  // this is your context/ Readonly means you can only set this data through the constructor
        private readonly PoContext _context;

//This is your constructor
        public EmployeesController(PoContext context)
        {
            _context = context;
        }

// Get All generated method

        // GET: api/Employees
        [HttpGet]
        // << is showing the return method type>> instance of a type Task 
        //ActionResult is a generic class and can return multiple used mostly for errors. EX: "Iteam not found"
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            return await _context.Employee.ToListAsync();  // creating a list of all employees
        }
//

        // GET: api/Employees/5
        [HttpGet("{id}")] // id is a route variables but doesnt declare a type 
        public async Task<ActionResult<Employee>> GetEmployee(int id)// this id must match route variable id
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee; // returns just the data 
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
// keeps track of modification history of cached data in entityframework// allows for update 
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync(); // await is needed for async 

// return function CreateAtAction( reads the database and returns the created employee with generated id value 
            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }


// http parameters must match your methods parameter
// GET: api/Employees/mimi/password
        [HttpGet("{login}/{password}")] // route variables 
        public async Task<ActionResult<Employee>> Login(string login, string password)// type and vars that match route vars
        {
            var empl = await _context.Employee
                .SingleOrDefaultAsync(e => e.Login == login && e.Password == password);
// if employee does not exist
            if(empl == null)
            {
                return NotFound();
            }
// if employee does exist, returns Ok
            return Ok(empl);

        }
    }
}
