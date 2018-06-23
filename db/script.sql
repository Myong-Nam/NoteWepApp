/*
	사용자 관리는 ui로도 할 수 있다.
	sql developer를 sys로 로그인
	다른 사용자위에 오른쪽 클릭 메뉴에서 사용자 생성, 삭제 할 수 있다.	
*/



-- 사용자 계정 생성 
create user note identified by note
default tablespace users
temporary tablespace temp;

-- 계정 삭제
--drop user note cascade;

-- 권한 부여
grant resource, connect to note;

/*
resource: 개체를 생성, 변경, 제거할 수 있는 권한 (DDL, DML 사용 가능)
connect: db에 연결할 수 있는 권한
DBA: db 관리자 권한
*/

-- 테이블 생성

/* NOTE */
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

/* NOTEBOOK */
CREATE SEQUENCE NOTEBOOK_SEQ 
INCREMENT BY 1 
START WITH 1
MAXVALUE 10000000000000000000000000000000000000
MINVALUE 1
NOCACHE  
NOCYCLE
NOORDER
;



CREATE TABLE NOTEBOOK
(
	NoteBookId               NUMBER NOT NULL ,
	Name                VARCHAR2(50) NOT NULL ,
	CREATEDDATE             DATE,
	CONSTRAINT  PK_Notebook PRIMARY KEY (NoteBookId)
);

ALTER TABLE NOTE ADD (NOTEBOOKID NUMBER NULL); -- 이미 데이터가 들어가 있어서 not null로 함. 나중에 not null로 변경.

ALTER TABLE NOTE ADD CONSTRAINT FK_NOTE_NOTEBOOK FOREIGN KEY (NOTEBOOKID) REFERENCES NOTEBOOK (  NOTEBOOKID );


-- commit;
