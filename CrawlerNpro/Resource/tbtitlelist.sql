--SELECT id,barname,url,type,hot,createtime,updatetime FROM barname
select * from tbtitlelist
SELECT id FROM tbtitlelist ORDER BY id DESC LIMIT 0,1;

SELECT * FROM tbtitlelist ORDER BY id AND visiable=1 limit 50 offset 0; 
SELECT * FROM tbtitlelist ORDER BY id AND visiable=1 limit 50 offset 50; 
--delete from tbtitlelist
--'11','212','222','212','212','212','212'

--查询总数
select count(*)from tbtitlelist

SELECT COUNT(*)+1 FROM tbtitlelist
--插入
INSERT INTO tbtitlelist(id,title,url,uname,uid,replies,createcode,createtime，updatetime) VALUES(5,'11','212','null','null','null','null','null');
select last_insert_rowid();

SELECT id FROM tbtitlelist WHERE icode=1
select last_insert_rowid();

SELECT id,title,url,uname,uid,replies,createcode,createtime,updatetime FROM tbtitlelist where visiable=1 LIMIT 50 offset 0;

--分页查询
select* from tbtitlelist  where id=1 limit 2 offset 0; 

select id from tbtitlelist order by id desc limit 0,1;
SELECT id FROM tbtitlelist ORDER BY id DESC LIMIT 0,1;
--删除
DELETE FROM tbtitlelist WHERE id = 7;

--实例
SELECT id,title,url,uname,uid,replies,createCode,createtime,updatetime FROM tbtitlelist order by id limit 50 offset 20
SELECT id,title,url,uname,uid,replies,createcode,createtime,updatetime FROM tbtitlelist ORDER BY id  LIMIT 50 OFFSET 0

UPDATE tbtitlelist SET visiable='1' WHERE ID=1

///////////////////////////内容查询//////////////////////////////
select * from tbcontext 

SELECT id,content,titlecreatetime,replytime,floor,replynum,pagenum,createcode,createtime,updatetime FROM tbcontext WHERE visiable=1

