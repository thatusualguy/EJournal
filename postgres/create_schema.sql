DROP SCHEMA IF EXISTS "ejournal" CASCADE;
CREATE SCHEMA "ejournal";

CREATE TABLE "ejournal"."accounts"
(
    "id"       INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "email"    varchar,
    "phone"    varchar,
    "password" varchar,
    "role"     int,
    "user_id"  int
);

CREATE TABLE "ejournal"."user_roles"
(
    "id"   INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "name" varchar
);

CREATE TABLE "ejournal"."students"
(
    "id"          INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "first_name"  varchar,
    "middle_name" varchar,
    "last_name"   varchar,
    "group_id"    int,
    "budget"      bool,
    "birthday"    date
);

CREATE TABLE "ejournal"."groups"
(
    "id"             INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "admission_year" int,
    "name"           varchar,
    "specialty"      int
);

CREATE TABLE "ejournal"."specialties"
(
    "id"   INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "code" varchar,
    "name" varchar
);

CREATE TABLE "ejournal"."teachers"
(
    "id"           INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "first_name"   varchar,
    "middle_name"  varchar,
    "last_name"    varchar,
    "contact_info" varchar,
    "birthday"     date
);

CREATE TABLE "ejournal"."subjects"
(
    "id"         INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "course_id"  int,
    "teacher_id" int,
    "group_id"   int
);

CREATE TABLE "ejournal"."courses"
(
    "id"         INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "name"       varchar,
    "hour_count" int,
    "end_mark"   int,
    "semester"   int
);

CREATE TABLE "ejournal"."mark_types"
(
    "id"   INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "name" varchar
);

CREATE TABLE "ejournal"."lessons"
(
    "id"         INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "subject_id" int,
    "date"       date,
    "note"       varchar
);

CREATE TABLE "ejournal"."marks"
(
    "student_id" int,
    "lesson_id"  int,
    "mark"       int,
    PRIMARY KEY ("student_id", "lesson_id")
);

CREATE TABLE "ejournal"."imports"
(
    "group_id"    int,
    "year"        int,
    "link"        varchar,
    "last_import" timestamp,
    PRIMARY KEY ("group_id", "year")
);

ALTER TABLE "ejournal"."accounts"
    ADD FOREIGN KEY ("role") REFERENCES "ejournal"."user_roles" ("id");

ALTER TABLE "ejournal"."students"
    ADD FOREIGN KEY ("group_id") REFERENCES "ejournal"."groups" ("id");

ALTER TABLE "ejournal"."groups"
    ADD FOREIGN KEY ("specialty") REFERENCES "ejournal"."specialties" ("id");

ALTER TABLE "ejournal"."subjects"
    ADD FOREIGN KEY ("course_id") REFERENCES "ejournal"."courses" ("id");

ALTER TABLE "ejournal"."subjects"
    ADD FOREIGN KEY ("teacher_id") REFERENCES "ejournal"."teachers" ("id");

ALTER TABLE "ejournal"."subjects"
    ADD FOREIGN KEY ("group_id") REFERENCES "ejournal"."groups" ("id");

ALTER TABLE "ejournal"."courses"
    ADD FOREIGN KEY ("end_mark") REFERENCES "ejournal"."mark_types" ("id");

ALTER TABLE "ejournal"."lessons"
    ADD FOREIGN KEY ("subject_id") REFERENCES "ejournal"."subjects" ("id");

ALTER TABLE "ejournal"."marks"
    ADD FOREIGN KEY ("student_id") REFERENCES "ejournal"."students" ("id");

ALTER TABLE "ejournal"."marks"
    ADD FOREIGN KEY ("lesson_id") REFERENCES "ejournal"."lessons" ("id");

ALTER TABLE "ejournal"."imports"
    ADD FOREIGN KEY ("group_id") REFERENCES "ejournal"."groups" ("id");