using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Faith.Persistence.Data;

public static class DbSetExtensions
{
    public static void RemoveIf<T>(this DbSet<T> theDbSet, Expression<Func<T, bool>> thePredicate) where T : class
    {
        var entities = theDbSet.Where(thePredicate);
        theDbSet.RemoveRange(entities);
    }
}