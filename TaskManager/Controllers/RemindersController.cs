using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IMapper _mapper;

        public RemindersController(TaskManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Reminders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReminderDto>>> GetReminders()
        {
            var reminders = await _context.Reminders
                .Include(r => r.Goal)
                .ToListAsync();
            var result = _mapper.Map<IEnumerable<ReminderDto>>(reminders);
            return Ok(result);
        }

        // GET: api/Reminders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReminderDto>> GetReminder(int id)
        {
            var reminder = await _context.Reminders
                .Include(r => r.Goal)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reminder == null)
            {
                return NotFound();
            }

            return _mapper.Map<ReminderDto>(reminder);
        }

        // GET: api/Reminders/goal/5
        [HttpGet("goal/{goalId}")]
        public async Task<ActionResult<IEnumerable<ReminderDto>>> GetGoalReminders(int goalId)
        {
            var reminders = await _context.Reminders
                .Where(r => r.GoalId == goalId)
                .ToListAsync();
            var result = _mapper.Map<IEnumerable<ReminderDto>>(reminders);
            return Ok(result);
        }

        // POST: api/Reminders
        [HttpPost]
        public async Task<ActionResult<ReminderDto>> CreateReminder(CreateReminderDto createReminderDto)
        {
            var reminder = _mapper.Map<Reminder>(createReminderDto);
            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();

            var reminderDto = _mapper.Map<ReminderDto>(reminder);
            return CreatedAtAction(nameof(GetReminder), new { id = reminder.Id }, reminderDto);
        }

        // PUT: api/Reminders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReminder(int id, UpdateReminderDto updateReminderDto)
        {
            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            _mapper.Map(updateReminderDto, reminder);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReminderExists(id))
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

        // DELETE: api/Reminders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReminder(int id)
        {
            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Reminders/5/mark-sent
        [HttpPut("{id}/mark-sent")]
        public async Task<IActionResult> MarkReminderAsSent(int id)
        {
            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            reminder.IsSent = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReminderExists(int id)
        {
            return _context.Reminders.Any(e => e.Id == id);
        }
    }
} 