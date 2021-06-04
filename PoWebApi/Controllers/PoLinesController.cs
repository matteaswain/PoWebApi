using System;
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
    public class PoLinesController : ControllerBase
    {
        private readonly PoContext _context;

        private async Task RecalculatePoTotal(int poid)
        { 
            var po = await _context.PurchaseOrders.FindAsync(poid);
// checks to ensure po id passed in is a vaild id 
            if (po == null) throw new Exception("FATAL: Po is not found to recalculate!");

            var grandtotal = (from l in _context.PoLines // selects poline table
                    join i in _context.Items // joins item table
                    on l.ItemId equals i.Id // foreign keys 
                    where l.PurchaseOrderId == poid // selecting the poid that was passed through url
                    select new {LineTotal = l.Quantity * i.Price })
                    .Sum(x => x.LineTotal);
      //select new // creating a new column Linetotal that is quantity times item price. 
      //Wrap in () to ensure it processes first then .Sum is added to calculate the grand total 


            po.Total = grandtotal;
            await _context.SaveChangesAsync();          

        }

        public PoLinesController(PoContext context)
        {
            _context = context;
        }

        // GET: api/PoLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PoLine>>> GetPoLines()
        {
            return await _context.PoLines.ToListAsync();
        }

        // GET: api/PoLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PoLine>> GetPoLine(int id)
        {
            var poLine = await _context.PoLines.FindAsync(id);

            if (poLine == null)
            {
                return NotFound();
            }

            return poLine;
        }

        // PUT: api/PoLines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoLine(int id, PoLine poLine)
        {
            if (id != poLine.Id)
            {
                return BadRequest();
            }

            _context.Entry(poLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await RecalculatePoTotal(poLine.PurchaseOrderId); // recalculates total /ADDED
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoLineExists(id))
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

        // POST: api/PoLines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PoLine>> PostPoLine(PoLine poLine)
        {
            _context.PoLines.Add(poLine);
            await RecalculatePoTotal(poLine.PurchaseOrderId);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPoLine", new { id = poLine.Id }, poLine);
        }

        // DELETE: api/PoLines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PoLine>> DeletePoLine(int id)
        {
            var poLine = await _context.PoLines.FindAsync(id);
            if (poLine == null)
            {
                return NotFound();
            }

            _context.PoLines.Remove(poLine);
            await RecalculatePoTotal(poLine.PurchaseOrderId);
            await _context.SaveChangesAsync();

            return poLine;
        }

        private bool PoLineExists(int id)
        {
            return _context.PoLines.Any(e => e.Id == id);
        }
    }
}
