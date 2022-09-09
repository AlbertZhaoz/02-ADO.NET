using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface IBaseService
    {
        #region 查询 
        /// <summary>
        /// 查询所有的数据，慎用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> Query<T>() where T : class;

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> Query<T>(string sql) where T : class;
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class;

        /// <summary>
        /// 通过Id查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<T>(int id) where T : BaseModel;

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
        public PageResult<T> GetPageList<T, S>(int pageIndex, int pageSize, Expression<Func<T, bool>> where, Expression<Func<T, S>> orderby) where T : class; 
        #endregion

        #region 增加 nuget引入:Z.Dapper.Plus

        /// <summary>
        /// 增加实体 
        /// Z.Dapper.Plus
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Insert<T>(T t) where T : BaseModel;

        /// <summary>
        /// 批量添加
        /// Z.Dapper.Plus
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<T> InsertBatch<T>(List<T> list) where T : BaseModel;
        #endregion

        #region 修改 增加 nuget引入:Z.Dapper.Plus
        /// <summary>
        ///  修改实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Update<T>(T t) where T : BaseModel;

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public IEnumerable<T> UpdateInsertBatch<T>(IEnumerable<T> list) where T : BaseModel;

        #endregion

        #region 删除 增加 nuget引入:Z.Dapper.Plus

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<T>(int id) where T : BaseModel;


        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Delete<T>(Expression<Func<T, bool>> where) where T : BaseModel;

        #endregion 
    }
}
