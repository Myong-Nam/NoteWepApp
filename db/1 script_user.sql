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

