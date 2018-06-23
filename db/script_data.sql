SET DEFINE OFF;

Insert into NOTEBOOK (NOTEBOOKID,NAME,CREATEDDATE,ISDELETED,ISDEFAULT,ISSHORTCUT) values (1,'기본노트북',sysdate,0,1,0);


rollback; -- 안전을 위하여
-- commit;