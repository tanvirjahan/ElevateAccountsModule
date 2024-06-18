--select * from columbusmaster 


--UPDATE [dbo].[columbusmaster]
--   SET [conm] = 'ELEVATE TOURISM L.L.C'
--      ,[coadd1] = 'Fifty one tower, 808, Marasi Street'
--      ,[coadd2] = 'Business Bay, Dubai, UAE'
--      ,[copobox] = '114456'
--      ,[cotel] = '042160600'
--      ,[cofax] = null
--      ,[coemail] = 'mohamed.sorour@elevatedmc.com'
--      ,[coweb] = 'www.elevatedmc.com'
--      ,[TRNNo] = '100481801700003'      
--      ,[companytoshow] = 'ELEVATE TOURISM L.L.C'
-- WHERE [div_code] = '01'
--GO


--UPDATE [dbo].[columbusmaster]
--   SET [conm] = 'ELEVATE TOURISM L.L.C'
--      ,[coadd1] = 'Fifty one tower, 808, Marasi Street'
--      ,[coadd2] = 'Business Bay, Dubai, UAE'
--      ,[copobox] = '114456'
--      ,[cotel] = '042160600'
--      ,[cofax] = null
--      ,[coemail] = 'mohamed.sorour@elevatedmc.com'
--      ,[coweb] = 'www.elevatedmc.com'
--      ,[TRNNo] = '100481801700003'      
--      ,[companytoshow] = 'ELEVATE TOURISM L.L.C'
-- WHERE [div_code] = '02'
--GO

--select * from division_master

--UPDATE [dbo].[division_master]
--   SET [division_master_des] = 'ELEVATE TOURISM L.L.C'
--      ,[adddate] = getdate()
--      ,[adduser] = 'admin'
--      ,[moddate] = null
--      ,[moduser] = null
--      ,[accountsmodulename] = 'Accounts Module'
--      ,[address1] = 'Fifty one tower, 808, Marasi Street, Business Bay, Dubai, UAE'
--      ,[tel] = '042160600'
--      ,[fax] = null
--      ,[email] = 'hello@elevatedmc.com'
--      ,[website] = 'www.elevatedmc.com'
--      ,[onlineURL] = null
--      ,[TRN] = '100481801700003'
--      ,[accountsEmailId] = 'mohamed.sorour@elevatedmc.com'
-- WHERE [division_master_code] = '01'
--GO

--update MenuMaster set  menu_status=1 where appid=4 and menuid in (180,185,187,189)