truncate ejournal.students cascade;
truncate ejournal.courses cascade;
truncate ejournal.teachers cascade;
truncate ejournal.specialties cascade;
truncate ejournal.mark_types cascade;
truncate ejournal.user_roles cascade;
truncate ejournal.accounts cascade;

ALTER SEQUENCE ejournal.accounts_id_seq RESTART;
ALTER SEQUENCE ejournal.courses_id_seq RESTART;
ALTER SEQUENCE ejournal.groups_id_seq RESTART;
ALTER SEQUENCE ejournal.lessons_id_seq RESTART;
ALTER SEQUENCE ejournal.mark_types_id_seq RESTART;
ALTER SEQUENCE ejournal.specialties_id_seq RESTART;
ALTER SEQUENCE ejournal.students_id_seq RESTART;
ALTER SEQUENCE ejournal.subjects_id_seq RESTART;
ALTER SEQUENCE ejournal.teachers_id_seq RESTART;
ALTER SEQUENCE ejournal.user_roles_id_seq RESTART;


