namespace Faith.Persistence.Data;

public class FaithDataInitializer
{
    private readonly FaithDbContext _dbContext;

    public FaithDataInitializer(FaithDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedData()
    {
        //_dbContext.Database.EnsureDeleted();
        if (_dbContext.Database.EnsureCreated())
        {
            
        }
    }


}