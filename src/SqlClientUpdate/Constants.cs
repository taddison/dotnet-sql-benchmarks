namespace SqlClientUpdate
{
  public static class Constants
  {
    public static string ConnectionString = "server=localhost;initial catalog=master;integrated security=SSPI";
    // https://dba.stackexchange.com/a/152536
    public static string BaseQuery = @"
  with
L0 as (select 1 as c union all select 1),
L1 as (select 1 as c from L0 A cross join L0 B),
L2 as (select 1 as c from L1 A cross join L1 B),
L3 as (select 1 as c from L2 A cross join L2 B),
L4 as (select 1 as c from L3 A cross join L3),
L5 as (select 1 as c from L4 A cross join L4),
nums as (select 1 as NUM from L5)   
select top {rowcount}
  cast(row_number() over (order by (select null)) AS int) id
, cast(t.rand_value AS int) random_int
, cast(t.rand_value AS varchar(100)) random_string
, cast(getutcdate() AS datetime) fixed_date
from nums cross join (select round(1000 * rand(checksum(newid())), 0) rand_value) t;
  ";
    public static string Query_10 = BaseQuery.Replace("{rowcount}", "10");
    public static string Query_100_000 = BaseQuery.Replace("{rowcount}", "100000");

    public static string BaseQuery_ManyColumns = @"
  with
L0 as (select 1 as c union all select 1),
L1 as (select 1 as c from L0 A cross join L0 B),
L2 as (select 1 as c from L1 A cross join L1 B),
L3 as (select 1 as c from L2 A cross join L2 B),
L4 as (select 1 as c from L3 A cross join L3),
L5 as (select 1 as c from L4 A cross join L4),
nums as (select 1 as NUM from L5)   
select top {rowcount}
  cast(row_number() over (order by (select null)) AS int) id
, cast(t.rand_value AS int) random_int
, cast(t.rand_value AS int) random_int_1
, cast(t.rand_value AS int) random_int_2
, cast(t.rand_value AS int) random_int_3
, cast(t.rand_value AS int) random_int_4
, cast(t.rand_value AS int) random_int_5
, cast(t.rand_value AS int) random_int_6
, cast(t.rand_value AS int) random_int_7
, cast(t.rand_value AS int) random_int_8
, cast(t.rand_value AS int) random_int_9
, cast(t.rand_value AS varchar(100)) random_string
, cast(t.rand_value AS varchar(100)) random_string_1
, cast(t.rand_value AS varchar(100)) random_string_2
, cast(t.rand_value AS varchar(100)) random_string_3
, cast(t.rand_value AS varchar(100)) random_string_4
, cast(t.rand_value AS varchar(100)) random_string_5
, cast(t.rand_value AS varchar(100)) random_string_6
, cast(t.rand_value AS varchar(100)) random_string_7
, cast(t.rand_value AS varchar(100)) random_string_8
, cast(t.rand_value AS varchar(100)) random_string_9
, cast(getutcdate() AS datetime) fixed_date
, cast(getutcdate() AS datetime) fixed_date_1
, cast(getutcdate() AS datetime) fixed_date_2
, cast(getutcdate() AS datetime) fixed_date_3
, cast(getutcdate() AS datetime) fixed_date_4
, cast(getutcdate() AS datetime) fixed_date_5
, cast(getutcdate() AS datetime) fixed_date_6
, cast(getutcdate() AS datetime) fixed_date_7
, cast(getutcdate() AS datetime) fixed_date_8
, cast(getutcdate() AS datetime) fixed_date_9
from nums cross join (select round(1000 * rand(checksum(newid())), 0) rand_value) t;
  ";
    public static string Query_ManyColumns_10 = BaseQuery_ManyColumns.Replace("{rowcount}", "10");
    public static string Query_ManyColumns_100_000 = BaseQuery_ManyColumns.Replace("{rowcount}", "100000");
  }
}