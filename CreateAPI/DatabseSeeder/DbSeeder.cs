using CreateAPI.DataAccess;

public class DbSeeder
{
    private readonly StudentDataContext _dbContext;

    public DbSeeder(StudentDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedDatabase()
    {
        if (!_dbContext.Students.Any())
        {
            // Read data from the CSV file and seed the database
            // Logic to read CSV file and populate the Students table
            // ...

            // Save changes to the database
            _dbContext.SaveChanges();
        }
    }
}
