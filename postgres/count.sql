select (select count(*) from ejournal.marks)    as Marks,
       (select count(*) from ejournal.lessons)  as Lessons,
       (select count(*) from ejournal.subjects) as Subjects,
       (select count(*) from ejournal.groups)   as Groups,
       (select count(*) from ejournal.teachers) as Teachers;