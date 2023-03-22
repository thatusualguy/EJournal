using EJournal.Reader;

using EJournal.Database;

using Microsoft.EntityFrameworkCore;

var optionsBuilder = new DbContextOptionsBuilder<EJournalContext>();
var options = optionsBuilder
		.UseNpgsql(@"Persist Security Info=True;Password=admin;Username=postgres;Database=postgres;Host=localhost")
		.Options;
var dbContext = new EJournalContext(options);

var reader = new ExcelReader(dbContext);

var filename = @"C:\Users\max\Downloads\test.xlsx";
reader.UpdateGroupJournal(filename);