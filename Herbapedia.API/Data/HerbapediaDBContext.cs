using Herbapedia.Model;
using Microsoft.EntityFrameworkCore;

namespace Herbapedia.API.Data
{

    public class HerbapediaDBContext : DbContext
    {
        public HerbapediaDBContext(DbContextOptions<HerbapediaDBContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<PlantTypeModel> PlantTypes { get; set; }
        public DbSet<PlantLogModel> PlantLogs { get; set; }
        public DbSet<PlantModel> Plants { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<CommentModel> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

    
        } 

    }
}
