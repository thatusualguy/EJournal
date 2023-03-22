using OfficeOpenXml;

namespace EJournal.Reader
{
	internal static class ExcelReaderHelpers
	{

		public static void ReadStudentNameFromRow(ExcelWorksheet subjectSheet, int row, out string firstName, out string middleName, out string lastName)
		{
			var studentName = subjectSheet.Cells[row, ExcelOffsets.NAME_COLUMN].Text.Trim().Split(new char[] { ' ', '.' });
			firstName = studentName[1];
			middleName = studentName[2];
			lastName = studentName[0];
		}

		public static DateOnly? ReadExcelDate(object value)
		{
			double? excelDate = value as double?;
			if (excelDate is null)
				return null;
			DateOnly lesson_date = new DateOnly(1900, 1, 1).AddDays((int)excelDate - 2);
			return lesson_date;
		}

		internal static int? ReadMark(string s)
		{
			foreach (char c in s)
				if (char.IsDigit(c))
				{
					int mark = int.Parse(c.ToString());
					if (mark >= 1 && mark <= 5)
						return mark;
				}
			return null;
		}

		internal static bool? ReadAttendance(string cell_text)
		{
			//if (cell_text is null) return null;
			//cell_text.
			//if (cell_text.Contains("н"))
			return null;
		}
	}
}