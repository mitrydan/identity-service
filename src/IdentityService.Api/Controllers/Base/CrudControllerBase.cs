using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IdentityService.Api.Controllers.Base
{
    public abstract class CrudControllerBase<TDbContext, TEntity> : ControllerBase
        where TDbContext : DbContext
        where TEntity : class
    {
        protected TDbContext DbContext { get; private set; }

        protected CrudControllerBase(TDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            var result = await DbContext.Set<TEntity>().ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> GetByIdAsync([Required] int id)
        {
            var entity = await DbContext.Set<TEntity>().FindAsync(id);
            if (entity == default)
                return NotFound();

            return Ok(entity);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> UpdateByIdAsync([Required] int id, [FromBody] TEntity updatedEntity)
        {
            var entity = await DbContext.Set<TEntity>().FindAsync(id);
            if (entity == default)
                return NotFound();

            DbContext.Attach(updatedEntity).State = EntityState.Modified;
            var result = await DbContext.SaveChangesAsync();
            return Ok(result == 1);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> DeleteByIdAsync([Required] int id)
        {
            var entity = await DbContext.Set<TEntity>().FindAsync(id);
            if (entity == default)
                return NotFound();

            DbContext.Set<TEntity>().Remove(entity);
            var result = await DbContext.SaveChangesAsync();
            return Ok(result == 1);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> AddAsync([FromBody] TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
            var result = await DbContext.SaveChangesAsync();
            return Ok(result == 1);
        }
    }
}
