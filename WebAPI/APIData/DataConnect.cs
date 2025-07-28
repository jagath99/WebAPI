namespace WebAPI.APIData;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
public class DataConnect : DbContext
{
    public DataConnect(DbContextOptions<DataConnect> options) : base(options) { }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
