using Application.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
    private readonly CleanDBContext _context;
    private readonly string _keyPropertyName;

    public GenericRepository(CleanDBContext context, string keyPropertyName = "Id")
    {
        _context = context;
        _keyPropertyName = keyPropertyName;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        var keyProperty = typeof(TEntity).GetProperty(_keyPropertyName);
        if (keyProperty == null)
        {
            throw new InvalidOperationException($"Entity does not have a key property '{_keyPropertyName}'.");
        }

        // Menggunakan LINQ untuk mengambil entitas berdasarkan kunci
        return await _context.Set<TEntity>().FirstOrDefaultAsync(e =>
            EF.Property<int>(e, _keyPropertyName) == id);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        var keyProperty = typeof(TEntity).GetProperty(_keyPropertyName);
        if (keyProperty == null)
        {
            throw new InvalidOperationException($"Entity does not have a key property '{_keyPropertyName}'.");
        }

        var keyValue = keyProperty.GetValue(entity);

        if (keyValue == null)
        {
            throw new InvalidOperationException("Key value cannot be null.");
        }

        var existingEntity = await _context.Set<TEntity>().FindAsync(keyValue);

        if (existingEntity == null)
        {
            throw new DbUpdateConcurrencyException($"The entity with key '{keyValue}' does not exist or has been deleted.");
        }

        // Perbarui nilai properti entitas
        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}