using Business.Interfaces;
using Business.Models;
using Dapper;
using Dapper.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Z.Dapper.Plus;

namespace Business.Services
{
    public class BaseService : IBaseService
    {
        protected IDbConnection _DbConnection = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="factory"></param>
        public BaseService(IDbConnectionFactory factory)
        {
            this._DbConnection = factory.GetConnection();
            SessionFactory.AddDataSource(new DataSource()
            {
                Name = "SqlServer",
                Source = () => this._DbConnection,
                SourceType = DataSourceType.SQLSERVER,
                UseProxy = true//使用Session的静态代理实现，记录日志，执行耗时,线上环境建议关闭代理
            });
        }

        #region 查询 
        /// <summary>
        /// 查询所有的数据，慎用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Query<T>() where T : class
        {
            string sql = CustomSqlCache<T>.GetQuerySql();
            return _DbConnection.Query<T>(sql, null).ToList();

        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> Query<T>(string sql) where T : class
        {
            return _DbConnection.Query<T>(sql, true, null, true, 10, CommandType.Text).ToList();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class
        {
            ISession session = SessionFactory.GetSession("SqlServer");
            Dapper.Extension.IQueryable<T> dataQuery = session.From<T>();
            if (expression != null)
            {
                dataQuery = session.From<T>().Where(expression);
            }
            using (session)
            {
                return dataQuery.Select();
            }
        }

        /// <summary>
        /// 通过Id查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<T>(int id) where T : BaseModel
        {
            string sql = CustomSqlCache<T>.GetFindByIdSql(id);
            return _DbConnection.QueryFirstOrDefault<T>(sql);
        }

        /// <summary>
        ///  //　Nuget：Install-Package Dapper.Common -Version 1.5.0   扩展 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public PageResult<T> GetPageList<T, S>(int pageIndex, int pageSize, Expression<Func<T, bool>> where, Expression<Func<T, S>> orderby) where T : class
        {
            //　Nuget：Install-Package Dapper.Common -Version 1.5.0   扩展 
            PageResult<T> pageResult = new PageResult<T>(pageIndex, pageSize);
            ISession session = SessionFactory.GetSession("SqlServer");
            Dapper.Extension.IQueryable<T> dataQuery = session.From<T>();
            if (where != null)
            {
                dataQuery = session.From<T>().Where(where);
            }
            if (orderby != null)
            {
                dataQuery = session.From<T>().OrderBy(orderby);
            }
            using (session)
            {
                pageResult.DataList = dataQuery.Page(pageIndex, pageSize, out long totalCount, true).Select().ToList();
                pageResult.TotalCount = totalCount;
            }
            return pageResult;
        }
        #endregion

        #region 增加 nuget引入:Z.Dapper.Plus

        /// <summary>
        /// 增加实体 
        /// Z.Dapper.Plus
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Insert<T>(T t) where T : BaseModel
        {
            DapperPlusManager.Entity<T>().Table(typeof(T).Name).Identity(c => c.Id);
            DapperPlusActionSet<T> retsult = _DbConnection.BulkInsert(t);
            return retsult.CurrentItem;
        }

        /// <summary>
        /// 批量添加
        /// Z.Dapper.Plus
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<T> InsertBatch<T>(List<T> list) where T : BaseModel
        {
            DapperPlusManager.Entity<T>().Table(typeof(T).Name).Identity(c => c.Id);
            DapperPlusActionSet<List<T>> retsult = _DbConnection.BulkInsert(list);
            return retsult.CurrentItem;
        }
        #endregion

        #region 修改 增加 nuget引入:Z.Dapper.Plus
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Update<T>(T t) where T : BaseModel
        {
            DapperPlusActionSet<T> uResult = _DbConnection.BulkUpdate(t);
            return uResult.CurrentItem;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public IEnumerable<T> UpdateInsertBatch<T>(IEnumerable<T> list) where T : BaseModel
        {
            DapperPlusActionSet<T> result = _DbConnection.BulkUpdate(list);
            return result.Current;
        }

        #endregion

        #region 删除 增加 nuget引入:Z.Dapper.Plus

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<T>(int id) where T : BaseModel
        {
            ISession session = SessionFactory.GetSession("SqlServer");
            return session.From<T>()
               .Where(c => c.Id == id)
               .Delete() > 0;
        }


        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Delete<T>(Expression<Func<T, bool>> where) where T : BaseModel
        {
            ISession session = SessionFactory.GetSession("SqlServer");
            return session.From<T>()
               .Where(where)
               .Delete() > 0;
        }

        #endregion 
    }
}
