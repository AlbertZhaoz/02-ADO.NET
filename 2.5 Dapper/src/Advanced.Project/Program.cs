using Advanced.Models;
using Business.Interfaces;
using Business.Services;
using Dapper;
using Dapper.Contrib.Extensions;
using Dapper.Extension;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Z.Dapper.Plus;
using static Dapper.SqlMapper;

namespace Advanced.Project
{
    class Program
    {
        private static string ConnectionString = "Data Source=DESKTOP-T2D6ILD;Initial Catalog=CustomerDB;User ID=sa;Password=sa123";

        static void Main(string[] args)
        {
            try
            {
                //基本使用：
                {
                    string sql = @"SELECT [Id] ,[Name] ,[CreateTime] ,[CreatorId]  ,[LastModifierId] ,[LastModifyTime] FROM [CustomerDB].[dbo].[Company] Where Id=@Id";


                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        //object oResult = connection.ExecuteScalar(sql, new { Id = 1 });
                        //DataTable dataTable = connection.GetSchema();
                        int result1 = connection.Execute(sql,
                            new { Id = 1 },
                            null,
                            300,
                            CommandType.Text); //受影响的行数

                        int result2 = connection.Execute(sql,
                          new { Id = 2 },
                          null,
                          300,
                          CommandType.Text); //受影响的行数


                        IEnumerable<CompanyInfo> result3 = connection.Query<CompanyInfo>(sql,
                            new { Id = 1 },
                            null, true, 300, CommandType.Text);//返回IEnumerable<T>

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Id", 1, DbType.Int32);

                        int i = parameters.Get<int>("@Id");

                        IEnumerable<CompanyInfo> result4 = connection.Query<CompanyInfo>(sql,
                           parameters,
                           null, true, 300, CommandType.Text);//返回IEnumerable<T>

                        //dynamic result5 = connection.QueryFirst(sql, new { Id = 1 });
                        dynamic result6 = connection.QueryFirstOrDefault(sql, new { Id = 1 });
                        GridReader result7 = connection.QueryMultiple(sql, new { Id = 1 });


                    }
                }

                //链接查询
                {
                    //string sql1 = "SELECT  * FROM Company AS c INNER JOIN SysUser AS s ON c.Id = s.CompanyId";
                    //using (var connection = new SqlConnection(ConnectionString))
                    //{
                    //    var orderDictionary = new Dictionary<int, CompanyInfo>();

                    //    var list = connection.Query<CompanyInfo, SysUser, CompanyInfo>(sql1,
                    //        (company, sysuser) =>
                    //        {
                    //            CompanyInfo companyEntry;
                    //            if (!orderDictionary.TryGetValue(company.Id, out companyEntry))
                    //            {
                    //                companyEntry = company;
                    //                companyEntry.SysUsers = new List<SysUser>();
                    //                orderDictionary.Add(companyEntry.Id, companyEntry);
                    //            }
                    //            companyEntry.SysUsers.Add(sysuser);
                    //            return companyEntry;
                    //        },
                    //        splitOn: "Id")
                    //    .Distinct()
                    //    .ToList();
                    //}
                }

                //Dapper Contrib
                //Nuget引入：Dapper Contrib
                {
                    //using (SqlConnection connection = new SqlConnection(ConnectionString))
                    //{
                    //    connection.Open();

                    //    connection.Insert(new CompanyInfo()
                    //    {
                    //        Name = "Richard2022",
                    //        CreateTime = DateTime.Now,
                    //        LastModifierId = 1,
                    //        CreatorId = 1,
                    //        LastModifyTime = DateTime.Now
                    //    });


                    //    CompanyInfo company = connection.Get<CompanyInfo>(1);
                    //    IEnumerable<CompanyInfo> companyInfos = connection.GetAll<CompanyInfo>().OrderByDescending(c=>c.Id);

                    //    company.Name = "朝夕教育" + DateTime.Now.ToString();
                    //    connection.Update(company); 
                    //    connection.Delete(company);

                    //}
                }

                //事务 
                {
                    using (var connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                string sql3 = @"DELETE FROM [dbo].[SysUser]  WHERE Id=2";
                                var ImpactRows = connection.Execute(sql3, null, transaction: transaction);


                                string sql2 = @"SELECT [Id] ,[Name] ,[CreateTime] ,[CreatorId]  ,[LastModifierId] ,[LastModifyTime] FROM [CustomerDB].[dbo].[Company] Where Id=@Id";

                                var ImpactRows1 = connection.Execute(sql2, new { Id = 1 }, transaction: transaction);


                                string sql4 = @"INSERT INTO [dbo].[Company]
                                               ([Name]
                                               ,[CreateTime]
                                               ,[CreatorId]
                                               ,[LastModifierId]
                                               ,[LastModifyTime])
                                         VALUES
                                               ('你好欢迎来到.NET 高级班'
                                               ,GETDATE()
                                               ,1
                                               ,1
                                               ,GETDATE())";
                                connection.Execute(sql4, null, transaction: transaction);
                                transaction.Commit(); //这里才是提交事务
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();//如果失败就回滚回去
                            }
                        }
                    }
                }

                //Dapper.Common
                //1.Nuget引入进来~~
                {

                    SqlConnection connection = new SqlConnection(ConnectionString);
                    //这里的这个链接是链接SqlServer

                    SessionFactory.AddDataSource(new DataSource()
                    {
                        Name = "SqlServer",
                        Source = () => connection,
                        SourceType = DataSourceType.SQLSERVER,
                        UseProxy = true//使用Session的静态代理实现，记录日志，执行耗时,线上环境建议关闭代理
                    });

                    {
                        ISession session = SessionFactory.GetSession("SqlServer");
                        var list = session.From<CompanyInfo>()
                            .Page(1, 10, out long total, true)
                            .Where(c => c.Id < 500); //这句话执行的时候，并没有去数据库中去查询数据

                        var data = list.Select(c => new
                        {
                            c.Id,
                            c.Name,
                            c.LastModifyTime
                        });
                    } 
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
