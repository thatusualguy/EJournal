using EJournal.Database;

using OfficeOpenXml;

namespace EJournal.Reader
{
	public class ExcelReader
	{
		private readonly EJournalContext _context;

		public ExcelReader(EJournalContext context)
		{
			_context = context;
			SetLicense();
		}

		private void SetLicense()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

		public void UpdateGroupJournal(string filename)
		{
			using var myExcelFile = new ExcelPackage(filename);
			myExcelFile.Compatibility.IsWorksheets1Based = false;

			var indexSheet = myExcelFile.Workbook.Worksheets[0];
			Group group = GetGroup(indexSheet);

			var firstSheet = myExcelFile.Workbook.Worksheets[1];
			List<Student> students = GetStudents(firstSheet, group);

			(Subject subject, int sheet)[] subjectIndexPairs = GetSubjects(indexSheet, group);

			foreach (var subject_id_pair in subjectIndexPairs)
			{
				var subjectSheet = myExcelFile.Workbook.Worksheets[subject_id_pair.sheet];
				var lessons = getLessons(subjectSheet, subject_id_pair.subject);
				ImportMarks(subjectSheet, subject_id_pair.subject);
			}
		}

		private void ImportMarks(ExcelWorksheet subjectSheet, Subject subject)
		{
			subjectSheet.Calculate();

			for (int column = ExcelOffsets.FIRST_LESSON_COLUMN; column <= ExcelOffsets.FIRST_LESSON_COLUMN + subject.Lessons.Count; column++)
			{
				DateOnly? lesson_date = ExcelReaderHelpers.ReadExcelDate(subjectSheet.Cells[ExcelOffsets.FIRST_NAME_ROW - 1, column].Value);
				if (lesson_date is null) { continue; }

				var lesson = subject.Lessons.SingleOrDefault(l => l.Date == lesson_date);
				if (lesson is null) { continue; }

				for (int row = ExcelOffsets.FIRST_NAME_ROW; subjectSheet.Cells[row, ExcelOffsets.NAME_COLUMN].Text.Trim().Length != 0; row++)
				{
					string firstName, middleName, lastName;
					ExcelReaderHelpers.ReadStudentNameFromRow(subjectSheet, row, out firstName, out middleName, out lastName);

					Student student = subject.Group.Students
						.Single(s => s.FirstName.StartsWith(firstName)
											&& s.MiddleName.StartsWith(middleName)
											&& s.LastName.StartsWith(lastName));


					var cell_text = subjectSheet.Cells[row, column].Text.Trim();

					int? mark = ExcelReaderHelpers.ReadMark(cell_text);
					if (mark.HasValue)
						AddMark(lesson, student, mark);

					bool? attended = ExcelReaderHelpers.ReadAttendance(cell_text);
					if (attended.HasValue && !attended.Value)
						AddMissingAttendance(lesson, student);
				}
			}
		}

		private void AddMissingAttendance(Lesson lesson, Student student)
		{
			//var attendance_entry = 
			throw new NotImplementedException();
		}

		private void AddMark(Lesson? lesson, Student student, int? mark)
		{
			var mark_entry = _context.Marks.SingleOrDefault(m => m.Lesson == lesson && m.Student == student);
			if (mark_entry is null)
			{
				mark_entry = new Mark() { Student = student, Lesson = lesson, Mark1 = mark };
				_context.Add(mark_entry);
			}
			else
			{
				mark_entry.Mark1 = mark;
				_context.Update(mark_entry);
			}
			_context.SaveChanges();
		}

		private List<Lesson> getLessons(ExcelWorksheet subjectSheet, Subject subject)
		{
			var existing_lessons = subject.Lessons.ToList();

			int last_name_row = ExcelOffsets.FIRST_NAME_ROW;
			while (subjectSheet.Cells[last_name_row, ExcelOffsets.NAME_COLUMN].Text.Length != 0)
				last_name_row++;

			for (int row = last_name_row + ExcelOffsets.FIRST_LESSON_ROW_OFFSET;
				subjectSheet.Cells[row, ExcelOffsets.LESSON_DATE_COLUMN].Text.Length != 0;
				row++)
			{

				DateOnly? lesson_date = ExcelReaderHelpers.ReadExcelDate(subjectSheet.Cells[row, ExcelOffsets.LESSON_DATE_COLUMN].Value);
				if (lesson_date is null)
					continue;

				var lesson = _context.Lessons.SingleOrDefault(l => l.Date == lesson_date && l.Subject == subject);

				if (lesson is null)
				{
					string lesson_theme = subjectSheet.Cells[row, ExcelOffsets.LESSON_THEME_COLUMN].Text.Trim();
					string lesson_note = subjectSheet.Cells[row, ExcelOffsets.LESSON_THEME_COLUMN + 6].Text.Trim();
					lesson = new Lesson()
					{
						Date = lesson_date,
						Subject = subject,
						Theme = lesson_theme,
						Note = lesson_note
					};

					_context.Add(lesson);
					_context.SaveChanges();
				}
			}

			_context.SaveChanges();
			return subject.Lessons.ToList();
		}

		private (Subject subject, int sheet)[] GetSubjects(ExcelWorksheet indexSheet, Group group)
		{
			List<(Subject subject, int sheet)> subjectSheetIndexPairs = new();
			int sheet_index = 1;

			for (int row = 4; indexSheet.Cells[row, ExcelOffsets.TEACHER_NAME_COL].Text.Trim().Length != 0; row++)
				for (int column = ExcelOffsets.LEFT_SEMESTER_COL; column <= ExcelOffsets.RIGTH_SEMESTER_COL; column++)
				{
					string subject_name = indexSheet.Cells[row, column].Text.Trim();
					if (subject_name.Length == 0)
						continue;

					int subject_semester = int.Parse(indexSheet.Cells[3, column].Text.Trim().Split()[2]);


					Course? course = _context.Courses.SingleOrDefault(c => c.Name == subject_name && c.Semester == subject_semester);

					if (course is null)
					{
						course = new Course() { Name = subject_name, Semester = subject_semester };
						_context.Courses.Add(course);
						_context.SaveChanges();
					}

					Subject? subject = _context.Subjects.SingleOrDefault(s => s.Course == course && s.Group == group);

					if (subject is null)
					{
						var teacher_name = indexSheet.Cells[row, ExcelOffsets.TEACHER_NAME_COL].Text.Trim().Split(new char[] { ' ', '.' }).Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();
						Teacher? teacher = _context.Teachers.SingleOrDefault(t => t.LastName == teacher_name[0] && t.FirstName.StartsWith(teacher_name[1]) && t.MiddleName.StartsWith(teacher_name[2]));
						if (teacher is null)
						{
							teacher = new Teacher()
							{
								LastName = teacher_name[0],
								FirstName = teacher_name[1],
								MiddleName = teacher_name[2],
							};
							_context.Add(teacher);
							_context.SaveChanges();
						}

						subject = new Subject()
						{
							Course = course,
							Group = group,
							Teacher = teacher
						};
						_context.Add(subject);
						_context.SaveChanges();
					}

					subjectSheetIndexPairs.Add((subject, sheet_index));
					sheet_index++;
				}

			_context.SaveChanges();
			return subjectSheetIndexPairs.ToArray();
		}

		private List<Student> GetStudents(ExcelWorksheet sheet, Group group)
		{
			for (int row = ExcelOffsets.FIRST_NAME_ROW; sheet.Cells[row, ExcelOffsets.NAME_COLUMN].Text.Trim().Length != 0; row++)
			{
				string firstName, middleName, lastName;
				ExcelReaderHelpers.ReadStudentNameFromRow(sheet, row, out firstName, out middleName, out lastName);

				Student? student = _context.Students
					.SingleOrDefault(s => s.FirstName.StartsWith(firstName)
										&& s.MiddleName.StartsWith(middleName)
										&& s.LastName == lastName
										&& s.Group == group);
				if (student is null)
				{
					student = new Student()
					{
						Group = group,
						FirstName = firstName,
						MiddleName = middleName,
						LastName = lastName
					};
					group.Students.Add(student);
					_context.SaveChanges();
				}
			}

			_context.SaveChanges();
			return group.Students.ToList();
		}

		private Group GetGroup(ExcelWorksheet indexSheet)
		{

			Group? group;

			string groupName = indexSheet.Cells[1, 1].Text.Trim().Split(' ')[4];
			group = _context.Groups.SingleOrDefault(g => g.Name == groupName);

			if (group is null)
			{
				string specialty_code = indexSheet.Cells[2, 1].Text.Trim().Split(" ")[1];
				string specialty_name = indexSheet.Cells[2, 1].Text.Trim().Split('"')[1];

				Specialty? specialty = _context.Specialties.SingleOrDefault(s => s.Code == specialty_code);
				if (specialty is null)
				{
					specialty = new Specialty()
					{
						Code = specialty_code,
						Name = specialty_name
					};
					_context.Specialties.Add(specialty);
				}

				group = new Group() { Name = groupName, SpecialtyNavigation = specialty };
				_context.Groups.Add(group);
			}

			_context.SaveChanges();
			return group;
		}
	}
}
