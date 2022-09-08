using Advanced.WebApplication.Models;
using Business.Interfaces;
using Business.Models;
using Business.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Advanced.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IBaseService _IBaseService = null;

        public HomeController(ILogger<HomeController> logger, IBaseService baseService)
        {
            _logger = logger;
            this._IBaseService = baseService;
        }

        public IActionResult Index()
        {
            #region Query 
            {
                List<Company> companylist = _IBaseService.Query<Company>();
                string sql = @"SELECT TOP (1000) [Id]
                                              ,[Name]
                                              ,[CreateTime]
                                              ,[CreatorId]
                                              ,[LastModifierId]
                                              ,[LastModifyTime]
                                          FROM [CustomerDB].[dbo].[Company]";
                List<Company> companylist1 = _IBaseService.Query<Company>(sql);
                List<Company> companylist2 = _IBaseService.Query<Company>(c => c.Id < 11).ToList();
                Company Company1 = _IBaseService.Find<Company>(1);
                PageResult<Company> page = _IBaseService.GetPageList<Company, int>(1, 10, null, null);
            }
            #endregion

            #region Add 
            {
                Company company = _IBaseService.Insert<Company>(new Company()
                {
                    Name = "Test001",
                    CreateTime = DateTime.Now,
                    CreatorId = 1,
                    LastModifierId = 1,
                    LastModifyTime = DateTime.Now
                });

                List<Company> companyList = _IBaseService.InsertBatch<Company>(new List<Company>()
                {
                    new Company(){
                        Name = "Test002",
                        CreateTime = DateTime.Now,
                        CreatorId = 1,
                        LastModifierId = 1,
                        LastModifyTime = DateTime.Now
                    },
                    new Company(){
                        Name = "Test003",
                        CreateTime = DateTime.Now,
                        CreatorId = 1,
                        LastModifierId = 1,
                        LastModifyTime = DateTime.Now
                    },
                    new Company(){
                        Name = "Test004",
                        CreateTime = DateTime.Now,
                        CreatorId = 1,
                        LastModifierId = 1,
                        LastModifyTime = DateTime.Now
                    }
                });


            }
            #endregion

            #region Update
            {

                Company company = _IBaseService.Find<Company>(54);
                company.Name = "Richard你好xxx";
                var bResult = _IBaseService.Update(company);
                IEnumerable<Company> companiesUpdaetlist = _IBaseService.Query<Company>(c => c.Id < 60);
                foreach (var item in companiesUpdaetlist)
                {
                    item.Name = "2022 你好";
                }
                var bResult1 = _IBaseService.UpdateInsertBatch(companiesUpdaetlist.ToList());
            }
            #endregion

            #region Delete
            {
                bool bResult = _IBaseService.Delete<Company>(49);
                bool bResult1 = _IBaseService.Delete<Company>(c => c.Id >= 49 && c.Id <= 53);
            }
            #endregion 
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
