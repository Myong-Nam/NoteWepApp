
DROP TABLE Note CASCADE CONSTRAINTS PURGE;
DROP SEQUENCE NOTE_SEQ ;

CREATE SEQUENCE NOTE_SEQ 
INCREMENT BY 1 
START WITH 1
MAXVALUE 10000000000000000000000000000000000000
MINVALUE 1
NOCACHE  
NOCYCLE
NOORDER
;

CREATE TABLE Note
(
	NoteId               NUMBER NOT NULL ,
	Title                VARCHAR2(50) NOT NULL ,
	Contents             VARCHAR2(2000) NULL ,
	CreatedDate          DATE NOT NULL ,
	CONSTRAINT  PK_Note PRIMARY KEY (NoteId)
);


insert into note (noteid, title, contents, createddate) values (note_seq.nextval, '타이틀', '내용입니다.', sysdate );
insert into note (noteid, title, contents, createddate) values (note_seq.nextval, '타이틀2', '내용입니다.2', sysdate );
insert into note (noteid, title, contents, createddate) values (note_seq.nextval, '타이틀3', '내용입니다.3', sysdate );
insert into note (noteid, title, contents, createddate) values (note_seq.nextval, '타이틀4', '내용입니다.4', sysdate );
insert into note (noteid, title, contents, createddate) values (note_seq.nextval, '타이틀5', '내용입니다.5', sysdate );
insert into note (noteid, title, contents, createddate) values (note_seq.nextval, '타이틀6', '내용입니다.6', sysdate );
insert into note (noteid, title, contents, createddate) values (note_seq.nextval, '타이틀7', '내용입니다.7', sysdate );
insert into note (noteid, title, contents, createddate) values (note_seq.nextval, '타이틀8', '내용입니다.8', sysdate );
insert into note (noteid, title, contents, createddate) values (note_seq.nextval, '타이틀9', '내용입니다.9', sysdate );

CREATE SEQUENCE NOTEBOOK_SEQ 
INCREMENT BY 1 
START WITH 1
MAXVALUE 10000000000000000000000000000000000000
MINVALUE 1
NOCACHE  
NOCYCLE
NOORDER
;

CREATE TABLE Notebook
(
	NoteBookId               NUMBER NOT NULL ,
	Name                VARCHAR2(50) NOT NULL ,
	CreatedDate          DATE NOT NULL ,
    IsDeleted           NUMBER(1) NOT NULL,
	CONSTRAINT  PK_NoteBook PRIMARY KEY (NoteBookId)
);

ALTER TABLE NOTE ADD (NOTEBOOKID NUMBER NOT NULL); 

ALTER TABLE NOTE ADD CONSTRAINT FK_NOTE_NOTEBOOK FOREIGN KEY (NOTEBOOKID) REFERENCES NOTEBOOK ( NOTEBOOKID );

ALTER TABLE NOTE ADD(ISDEFAULT VARCHAR2(1));

alter table NOTE  modify (ISDEFAULT default 0);

alter table NOTEBOOK add(ISDEFAULT VARCHAR2(1) default 0);

CREATE TABLE Shorcut
(
	Type				VARCHAR2(1),
	NoteBookId			NUMBER,
	Order				NUMBER NOT NULL,      
	CreatedDate         DATE NOT NULL,
	CONSTRAINT  PK_Shorcut PRIMARY KEY (ShorcutId)
);

ALTER TABLE NOTE ADD (ISSHORTCUT NUMBER NOT NULL); 

ALTER TABLE NOTEBOOK ADD (ISSHORTCUT NUMBER NOT NULL); 

ALTER TABLE SHORTCUT ADD CONSTRAINT FK_SHORTCUT_NOTE FOREIGN KEY (NOTEID) REFERENCES NOTE ( NOTEID );

ALTER TABLE SHORTCUT ADD CONSTRAINT FK_SHORTCUT_NOTEBOOK FOREIGN KEY (NOTEBOOKID) REFERENCES NOTEBOOK ( NOTEBOOKID );

ALTER TABLE NOTE ADD (NOTECONTENTS CLOB);
UPDATE NOTE SET NOTECONTENTS = CONTENTS;
ALTER TABLE NOTE DROP COLUMN CONTENTS;

CREATE SEQUENCE SHORTCUT_SEQ 
INCREMENT BY 1 
START WITH 1
MAXVALUE 10000000000000000000000000000000000000
MINVALUE 1
NOCACHE  
NOCYCLE
NOORDER
;

commit;
