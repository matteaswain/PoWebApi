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
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly PoContext _context;

        public PurchaseOrdersController(PoContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPurchaseOrders()
        {
            return await _context.PurchaseOrders 
           .ToListAsync();
        }

// Method will return employee instance

        // GET: api/PurchaseOrders/empl
        [HttpGet("empl")]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPurchaseOrdersEmployee()
        {
            return await _context.PurchaseOrders
          .Include(e => e.employee) //  reads the entire instance of your employee and brings back with purchase order 
          .ToListAsync();
        }

        // GET: api/PurchaseOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
                

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return purchaseOrder;
        }


        [HttpGet("reviews")]

        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetReviewForAdmin()
        {
            return await _context.PurchaseOrders
                .Where(p => p.Status == PurchaseOrder.StatusReview)
                .Include(p => p.employee)
                .ToListAsync();           
        }


// Method will retun employee instance

        // GET: api/PurchaseOrders/5/empl
        [HttpGet("{id}/empl")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrderEmployee(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders
                .Include(e => e.employee) // brings back the instance of your employee with read request
                .SingleOrDefaultAsync(p => p.Id == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return purchaseOrder;
        }

        [HttpPut("{id}/edit")]

        public async Task<IActionResult> UpdateStatusToEdit(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);

            if(purchaseOrder == null)
            {
                return NotFound();
            }

            purchaseOrder.Status = "EDIT";

            return await PutPurchaseOrder(id, purchaseOrder);
        }


        [HttpPut("{id}/review")]

        public async Task<IActionResult> POReviewOrApproved(int id)
        {

            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);

            if(purchaseOrder == null)
            {
                return NotFound();
            }

            if(purchaseOrder.Total == 0)
            {
                return BadRequest();
            }

            purchaseOrder.Status = (purchaseOrder.Total > 0 && purchaseOrder.Total <= 100) ? "APPROVED" : "REVIEW";

            return await PutPurchaseOrder(id, purchaseOrder);
        }

        [HttpPut("{id}/rejected")]

        public async Task<IActionResult> RejectPO(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);

            if(purchaseOrder == null)
            {
                return NotFound();

            }

            if(purchaseOrder.Total > 100)
            {
                purchaseOrder.Status = "REJECTED";
            }               
                return await PutPurchaseOrder(id, purchaseOrder);
        }

        [HttpPut("{id}/approved")]

        public async Task<IActionResult> ApprovePO(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);

            if (purchaseOrder == null)
            {
                return NotFound();

            }

            if (purchaseOrder.Total <= 100)
            {
                purchaseOrder.Status = "APPROVED";
            }
            return await PutPurchaseOrder(id, purchaseOrder);
        }



        // PUT: api/PurchaseOrders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseOrder(int id, PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchaseOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderExists(id))
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

        // POST: api/PurchaseOrders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> PostPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Add(purchaseOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseOrder", new { id = purchaseOrder.Id }, purchaseOrder);
        }

        // DELETE: api/PurchaseOrders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PurchaseOrder>> DeletePurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            _context.PurchaseOrders.Remove(purchaseOrder);
            await _context.SaveChangesAsync();

            return purchaseOrder;
        }

        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrders.Any(e => e.Id == id);
        }
    }
}
