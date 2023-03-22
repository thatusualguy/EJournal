using EJournal.Database;
using EJournal.Reader;

namespace EJournalGrpcService.Services
{
	public class ExcelReaderService : IExcelReaderService
	{
		private readonly EJournalContext _context;
		private readonly ExcelReader excelReader;

		public ExcelReaderService(EJournalContext context)
		{
			_context = context;
			excelReader = new ExcelReader(context);
		}

		public void ReimportGroups()
		{
			throw new NotImplementedException();
		}
	}
}
