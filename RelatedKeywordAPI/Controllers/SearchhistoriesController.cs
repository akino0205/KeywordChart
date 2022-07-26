using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RelatedKeywordLibrary.Models;

namespace RelatedKeywordAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchHistoriesController : ControllerBase
    {
        private readonly UserContext _context;

        public SearchHistoriesController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Searchhistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Searchhistory>>> GetAllSearchHistories()
        {
            if (_context.Searchhistories == null)
                return NotFound();
            return await _context.Searchhistories.ToListAsync();
        }

        // GET: api/Searchhistories/5
        [HttpGet("{userKey}")]
        public async Task<ActionResult<IEnumerable<Searchhistory>>> GetSearchHistories(int userKey)
        {
            if (_context.Searchhistories == null)
                return NotFound();

            var searchhistory = await _context.Searchhistories.Where(w => w.UserKey.Equals(userKey)).ToListAsync();

            if (searchhistory == null)
                return NotFound();

            return searchhistory;
        }
        [HttpGet("{userKey}/{keyword}")]
        public async Task<ActionResult<IEnumerable<Searchhistory>>> GetSearchHistories(int userKey, string keyword)
        {
            if (_context.Searchhistories == null)
                return NotFound();

            var searchhistory = await _context.Searchhistories.Where(w => w.UserKey.Equals(userKey) && w.Keyword.Equals(keyword)).ToListAsync();

            if (searchhistory == null)
                return NotFound();

            return searchhistory;
        }

        // PUT: api/Searchhistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkuserKey=2123754
        [HttpPut("{userKey}")]
        public async Task<IActionResult> PutSearchHistory(int userKey, Searchhistory searchhistory)
        {
            if (userKey != searchhistory.HistoryIndex)
                return BadRequest();

            _context.Entry(searchhistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SearchHistoryExists(userKey))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Searchhistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Searchhistory>> PostSearchHistory(Searchhistory searchhistory)
        {
            if (_context.Searchhistories == null)
                return Problem("Entity set 'UserContext.Searchhistories'  is null.");

            _context.Searchhistories.Add(searchhistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSearchhistory", new { userKey = searchhistory.UserKey }, searchhistory);
        }

        // DELETE: api/Searchhistories/5
        [HttpDelete("{userKey}")]
        public async Task<IActionResult> DeleteSearchHistory(int userKey)
        {
            if (_context.Searchhistories == null)
                return NotFound();
            
            var searchhistory = await _context.Searchhistories.FindAsync(userKey);
            if (searchhistory == null)
                return NotFound();

            _context.Searchhistories.Remove(searchhistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SearchHistoryExists(int userKey) => (_context.Searchhistories?.Any(e => e.UserKey == userKey)).GetValueOrDefault();

    }
}
