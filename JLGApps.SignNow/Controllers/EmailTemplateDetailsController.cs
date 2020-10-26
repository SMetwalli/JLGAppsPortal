using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JLGApps.SignNow.Models;

namespace JLGApps.SignNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTemplateDetailsController : ControllerBase
    {
        private readonly EmailTemplateDetailsContext _context;

        public EmailTemplateDetailsController(EmailTemplateDetailsContext context)
        {
            _context = context;
        }

        // GET: api/EmailTemplateDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmailTemplateDetail>>> GetEmailTemplateDetails()
        {
            return await _context.EmailTemplateDetails.ToListAsync();
        }

        // GET: api/EmailTemplateDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmailTemplateDetail>> GetEmailTemplateDetail(int id)
        {
            var emailTemplateDetail = await _context.EmailTemplateDetails.FindAsync(id);

            if (emailTemplateDetail == null)
            {
                return NotFound();
            }

            return emailTemplateDetail;
        }

        // PUT: api/EmailTemplateDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmailTemplateDetail(int id, EmailTemplateDetail emailTemplateDetail)
        {
            if (id != emailTemplateDetail.TemplateId)
            {
                return BadRequest();
            }

            _context.Entry(emailTemplateDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailTemplateDetailExists(id))
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

        // POST: api/EmailTemplateDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EmailTemplateDetail>> PostEmailTemplateDetail(EmailTemplateDetail emailTemplateDetail)
        {
            _context.EmailTemplateDetails.Add(emailTemplateDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmailTemplateDetail", new { id = emailTemplateDetail.TemplateId }, emailTemplateDetail);
        }

        // DELETE: api/EmailTemplateDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmailTemplateDetail>> DeleteEmailTemplateDetail(int id)
        {
            var emailTemplateDetail = await _context.EmailTemplateDetails.FindAsync(id);
            if (emailTemplateDetail == null)
            {
                return NotFound();
            }

            _context.EmailTemplateDetails.Remove(emailTemplateDetail);
            await _context.SaveChangesAsync();

            return emailTemplateDetail;
        }

        private bool EmailTemplateDetailExists(int id)
        {
            return _context.EmailTemplateDetails.Any(e => e.TemplateId == id);
        }
    }
}
