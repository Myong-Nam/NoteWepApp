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


--------------------------------------------------------
--  DDL for Table NOTE
--------------------------------------------------------
DROP TABLE Note CASCADE CONSTRAINTS PURGE;

  CREATE TABLE "NOTE" 
   (	"NOTEID" NUMBER, 
	"TITLE" VARCHAR2(50), 
	"CONTENTS" VARCHAR2(2000), 
	"NOTEDATE" DATE, 
	"ISDELETED" NUMBER(1,0) DEFAULT 0
   );
   
   
--------------------------------------------------------
--  DDL for Index PK_NOTE
--------------------------------------------------------

  CREATE UNIQUE INDEX "PK_NOTE" ON "NOTE" ("NOTEID");
--------------------------------------------------------
--  Constraints for Table NOTE
--------------------------------------------------------

  ALTER TABLE "NOTE" MODIFY ("ISDELETED" NOT NULL ENABLE);
  ALTER TABLE "NOTE" ADD CONSTRAINT "PK_NOTE" PRIMARY KEY ("NOTEID") ENABLE;
  ALTER TABLE "NOTE" MODIFY ("NOTEDATE" NOT NULL ENABLE);
  ALTER TABLE "NOTE" MODIFY ("TITLE" NOT NULL ENABLE);
  ALTER TABLE "NOTE" MODIFY ("NOTEID" NOT NULL ENABLE);

/* SEQUENCE NOTE_SEQ */
DROP SEQUENCE NOTE_SEQ ;
CREATE SEQUENCE  "NOTE_SEQ"  MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 NOCACHE  NOORDER  NOCYCLE ;


/* NOTEBOOK */

DROP TABLE NOTEBOOK CASCADE CONSTRAINTS PURGE;

CREATE TABLE NOTEBOOK
(
	NoteBookId               NUMBER NOT NULL ,
	Name                VARCHAR2(50) NOT NULL ,
	CREATEDDATE             DATE,
	isdeleted			NUMBER(1,0) DEFAULT 0,
	isdefault			NUMBER(1,0) DEFAULT 0,
	isshortcut			NUMBER(1,0) DEFAULT 0,
	CONSTRAINT  PK_Notebook PRIMARY KEY (NoteBookId)
);

/* FK 생성 */
ALTER TABLE NOTE ADD (NOTEBOOKID NUMBER NULL); -- 이미 데이터가 들어가 있어서 not null로 함. 나중에 not null로 변경.

ALTER TABLE NOTE ADD CONSTRAINT FK_NOTE_NOTEBOOK FOREIGN KEY (NOTEBOOKID) REFERENCES NOTEBOOK (  NOTEBOOKID );

/* SEQUENCE: NOTEBOOK_SEQ */
DROP SEQUENCE NOTEBOOK_SEQ;

CREATE SEQUENCE NOTEBOOK_SEQ 
INCREMENT BY 1 
START WITH 1
MAXVALUE 10000000000000000000000000000000000000
MINVALUE 1
NOCACHE  
NOCYCLE
NOORDER
;

/* table: SHORTCUT */
DROP TABLE SHORTCUT;

CREATE TABLE SHORTCUT
(
	NOTEBOOKID		NUMBER,
	NOTEID			NUMBER,
	ORDERS    		NUMBER
);
