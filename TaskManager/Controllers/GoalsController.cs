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
    public class GoalsController : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IMapper _mapper;

        public GoalsController(TaskManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Goals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GoalDto>>> GetGoals()
        {
            var goals = await _context.Goals
                .Include(g => g.User)
                .Include(g => g.GoalProgresses)
                .Include(g => g.Reminders)
                .ToListAsync();
            var result = _mapper.Map<IEnumerable<GoalDto>>(goals);
            return Ok(result);
        }

        // GET: api/Goals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GoalDto>> GetGoal(int id)
        {
            var goal = await _context.Goals
                .Include(g => g.User)
                .Include(g => g.GoalProgresses)
                .Include(g => g.Reminders)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (goal == null)
            {
                return NotFound();
            }

            return _mapper.Map<GoalDto>(goal);
        }

        // GET: api/Goals/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<GoalDto>>> GetUserGoals(int userId)
        {
            var goals = await _context.Goals
                .Include(g => g.GoalProgresses)
                .Include(g => g.Reminders)
                .Where(g => g.UserId == userId)
                .ToListAsync();
            var result = _mapper.Map<IEnumerable<GoalDto>>(goals);
            return Ok(result);
        }

        // POST: api/Goals
        [HttpPost]
        public async Task<ActionResult<GoalDto>> CreateGoal(CreateGoalDto createGoalDto)
        {
            var goal = _mapper.Map<Goal>(createGoalDto);
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();

            var goalDto = _mapper.Map<GoalDto>(goal);
            return CreatedAtAction(nameof(GetGoal), new { id = goal.Id }, goalDto);
        }

        // PUT: api/Goals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGoal(int id, UpdateGoalDto updateGoalDto)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
            {
                return NotFound();
            }

            _mapper.Map(updateGoalDto, goal);
            goal.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoalExists(id))
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

        // DELETE: api/Goals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(int id)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
            {
                return NotFound();
            }

            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Goals/5/progress
        [HttpPut("{id}/progress")]
        public async Task<IActionResult> UpdateGoalProgress(int id, [FromBody] int progressPercentage)
        {
            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
            {
                return NotFound();
            }

            if (progressPercentage < 0 || progressPercentage > 100)
            {
                return BadRequest("Progress percentage must be between 0 and 100");
            }

            goal.ProgressPercentage = progressPercentage;
            goal.UpdatedAt = DateTime.UtcNow;

            // Create a new progress record
            var progress = new GoalProgress
            {
                GoalId = id,
                ProgressValue = progressPercentage,
                StatusChange = goal.Status,
                UpdatedAt = DateTime.UtcNow
            };

            _context.GoalProgresses.Add(progress);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GoalExists(int id)
        {
            return _context.Goals.Any(e => e.Id == id);
        }
    }
} 