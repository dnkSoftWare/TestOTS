USE [OTSDB]
GO

/****** Object:  View [dbo].[ZnData]    Script Date: 11.12.2019 9:35:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER view [dbo].[ZnData]
as
with D as ( -- Список дат на которые выдавались наряды
 select CAST(zn.dataopen as Date) as zn_date
  from zn 
  group by CAST(zn.dataopen as Date)
)
,N as ( -- Список дат с нарядами select * from zn
 select CAST(zn.todofrom as Date) as zn_date, zn.recordid as zn_id 
  , MIN(datepart(HOUR, zn.todofrom)) as zn_from 
  , ( MIN(datepart(HOUR, zn.todotill)) + 1 ) as zn_to
  , ( MIN(datepart(HOUR, zn.todotill)) + 1 - MIN(datepart(HOUR, zn.todofrom))) as all_hours
  from zn group by CAST(zn.todofrom as Date), zn.recordid
) 
,L as ( -- Связь работников с их нарядами
 select s.recordid as staff_id, s.name as fio, CAST(zs.assign_date as Date) as st_date_assign, zs.zn_id from staff s
   join zn_staff_time zs on zs.staff_id = s.recordid
   join N on N.zn_id = zs.zn_id and N.zn_date = CAST(zs.assign_date as Date)  -- только те наряды которые указаны в расписании
  
) --select * from L
, DW as ( -- все даты со всеми работниками
 select D.zn_date, S.recordid as staff_id, s.name as fio from D
  left join staff s ON 1 = 1
) -- select * from DW
, DWS as (
select DW.*, N.zn_id , N.zn_from, N.zn_to, N.all_hours from DW
  left join L on L.staff_id = DW.staff_id 
  left join N on N.zn_id = L.zn_id and N.zn_date = DW.zn_date
) --select * from DWS 
, E as (
select DWS.zn_date, DWS.staff_id, DWS.fio, DWS.zn_id, DWS.zn_from, MAX(DWS.zn_to) as zn_to, SUM(DWS.all_hours) as all_hours  from DWS
 group by DWS.zn_date, DWS.staff_id, DWS.fio, DWS.zn_id, DWS.zn_from 
) 
select E.zn_date, E.fio, E.zn_from, E.zn_to, E.all_hours
, (select '№:'+ cast(E2.zn_id as varchar(500))+', ' --as p
  from E as E2 where E2.zn_date = E.zn_date and E2.fio = E.fio and E2.zn_from = E.zn_from 
   for xml path('') ) as zn_idss 
from E
 group by E.zn_date, E.fio, E.zn_from, E.zn_to, E.all_hours
; 
GO


