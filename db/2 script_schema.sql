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
/*
resource: 개체를 생성, 변경, 제거할 수 있는 권한 (DDL, DML 사용 가능)
connect: db에 연결할 수 있는 권한
DBA: db 관리자 권한
*/
grant resource, connect to note;


--------------------------------------------------------
-- TABLE NOTEBOOK
--------------------------------------------------------

DROP TABLE "NOTEBOOK" CASCADE CONSTRAINTS PURGE;

CREATE TABLE "NOTEBOOK" (
    "NOTEBOOKID"    NUMBER
        NOT NULL ENABLE
   ,"NAME"          VARCHAR2(50)
        NOT NULL ENABLE
   ,"ISDEFAULT"     VARCHAR2(1) DEFAULT 0
        NOT NULL ENABLE
   ,"ISSHORTCUT"    NUMBER DEFAULT 0
        NOT NULL ENABLE
   ,"ISDELETED"     NUMBER(1,0) DEFAULT 0
        NOT NULL ENABLE
   ,"CREATEDDATE"   DATE DEFAULT SYSDATE
        NOT NULL ENABLE
   ,CONSTRAINT "PK_NOTEBOOK" PRIMARY KEY ( "NOTEBOOKID" ) ENABLE
);

--------------------------------------------------------
-- SEQUENCE NOTEBOOK_SEQ
--------------------------------------------------------
DROP SEQUENCE "NOTEBOOK_SEQ";

CREATE SEQUENCE "NOTEBOOK_SEQ" MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 NOCACHE NOORDER NOCYCLE;

--------------------------------------------------------
-- TABLE NOTE
--------------------------------------------------------
DROP TABLE "NOTE" CASCADE CONSTRAINTS PURGE;

CREATE TABLE "NOTE" (
    "NOTEID"         NUMBER
        NOT NULL ENABLE
   ,"TITLE"          VARCHAR2(50)
        NOT NULL ENABLE
   ,"CREATEDDATE"    DATE DEFAULT SYSDATE
        NOT NULL ENABLE
   ,"ISDELETED"      NUMBER(1,0) DEFAULT 0
        NOT NULL ENABLE
   ,"NOTEDATE"       DATE
        NOT NULL ENABLE
   ,"NOTEBOOKID"     NUMBER
        NOT NULL ENABLE
   ,"ISDEFAULT"      VARCHAR2(1) DEFAULT 0
   ,"ISSHORTCUT"     NUMBER
        NOT NULL ENABLE
   ,"NOTECONTENTS"   CLOB
   ,CONSTRAINT "PK_NOTE" PRIMARY KEY ( "NOTEID" ) ENABLE
   ,CONSTRAINT "FK_NOTE_NOTEBOOK" FOREIGN KEY ( "NOTEBOOKID" )
        REFERENCES "NOTEBOOK" ( "NOTEBOOKID" )
    ENABLE
);

--------------------------------------------------------
-- SEQUENCE NOTE_SEQ
--------------------------------------------------------

DROP SEQUENCE "NOTE_SEQ";

CREATE SEQUENCE "NOTE_SEQ" MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 NOCACHE NOORDER NOCYCLE;


--------------------------------------------------------
-- TABLE SHORTCUT
--------------------------------------------------------
DROP TABLE "SHORTCUT" CASCADE CONSTRAINTS PURGE;

CREATE TABLE "SHORTCUT" (
    "SHORTCUTID"    NUMBER
        NOT NULL ENABLE
   ,"TYPE"          VARCHAR2(1)
        NOT NULL ENABLE
   ,"NOTEID"        NUMBER
   ,"NOTEBOOKID"    NUMBER
   ,"ORDER"         NUMBER
        NOT NULL ENABLE
   ,"CREATEDDATE"   DATE DEFAULT SYSDATE
        NOT NULL ENABLE
   ,CONSTRAINT "PK_SHORCUT" PRIMARY KEY ( "SHORTCUTID" ) ENABLE
   ,CONSTRAINT "FK_SHORTCUT_NOTE" FOREIGN KEY ( "NOTEID" )
        REFERENCES "NOTE" ( "NOTEID" )
    ENABLE
   ,CONSTRAINT "FK_SHORTCUT_NOTEBOOK" FOREIGN KEY ( "NOTEBOOKID" )
        REFERENCES "NOTEBOOK" ( "NOTEBOOKID" )
    ENABLE
);

--------------------------------------------------------
-- SEQUENCE SHORTCUT_SEQ
--------------------------------------------------------
DROP SEQUENCE "SHORTCUT_SEQ";

CREATE SEQUENCE "SHORTCUT_SEQ" MINVALUE 1 MAXVALUE 9999999999999999999999999999 INCREMENT BY 1 START WITH 1 NOCACHE NOORDER NOCYCLE;
