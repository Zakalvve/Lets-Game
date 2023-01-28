using Microsoft.EntityFrameworkCore;

namespace LetsGame.Data
{
    /// <summary>
    /// Class used as a helper to clear all data from a dbset. Can be useful if a clean database is required
    /// </summary>
    public static class EntityExtensions
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class {
            dbSet.RemoveRange(dbSet);
        }
    }
}