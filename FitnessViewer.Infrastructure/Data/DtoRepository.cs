namespace FitnessViewer.Infrastructure.Data
{
    public abstract class DtoRepository
    {
        protected ApplicationDb _context;
        public DtoRepository()
        {
            _context = new ApplicationDb();
        }

        public DtoRepository(ApplicationDb context)
        {
            _context = context;
        }
    }
}
