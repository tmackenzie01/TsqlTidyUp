Almost neat formatting of sqlcmd output in powershell with default 120 character width

sqlcmd query output in powershell looks like this (because each field always expands to the maximum it can display)

ID          IP               Name                                                             server_id   update_time   
          sync_time               sync2_update_time      
----------- ---------------- ---------------------------------------------------------------- ----------- --------------
--------- ----------------------- -----------------------
          1 172.150.100.20                                                                              2               
	NULL                    NULL                    NULL
          2 172.150.100.80   Root Server                                                                1               
	 NULL                    NULL 2016-08-25 14:08:42.000

(2 rows affected)

TidyUpTsql tidies up the output to be as follows

---------------------------------------------------------------------------------------------------|
 ID | IP             | Name        | server_id | update_time | sync_time | sync2_update_time       |
----|----------------|-------------|-----------|-------------|-----------|-------------------------|
 1  | 172.150.100.20 |             | 2         | NULL        | NULL      | NULL                    |
 2  | 172.150.100.80 | Root Server | 1         | NULL        | NULL      | 2016-08-25 14:08:42.000 |



