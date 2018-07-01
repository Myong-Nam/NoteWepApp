-- "&" 등의 특수문자를 일반문자열로 처리한다.
SET DEFINE OFF; 

/* NOTEBOOK */
Insert into NOTEBOOK (NOTEBOOKID,NAME,CREATEDDATE,ISDELETED,ISDEFAULT,ISSHORTCUT) values (1,'기본노트북',sysdate,0,1,0);


rollback; -- 안전을 위하여
-- commit; -- 확실해지면 주석을 해제한다.