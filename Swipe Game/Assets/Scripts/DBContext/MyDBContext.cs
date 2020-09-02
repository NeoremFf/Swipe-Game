using System.Data.Entity;

public class MyDBContext : DbContext
{
    public MyDBContext() : base("DbConnection")
    { }

    public DbSet<GameResources> ResourcesDBSet { get; set; }
}
