namespace WebApiCore.Infrastructure.Data
{
    public class DapperUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly DapperDbContext _context;

        public DapperUnitOfWorkFactory(DapperDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_context);
        }
    }
}