using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Сatalog.Models;
//using Сatalog.Models;

namespace Сatalog.Controllers
{

    public class HomeController : Controller
    {



        public IActionResult Index()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

            // TODO: ...
            string connect = "Host=127.0.0.1;Port=5432;Username=postgres;Password=RrTtYy1739;Database=company_directory;";
            string connect1 = @"Driver={PostgreSQL};Server=127.0.0.1;Port=5432;Database=company_directory;Uid=root;Pwd=RrTtYy1739;";
            string connect2 = @"Server=(localdb)\postgresqllocaldb;Database=company_directory;Trusted_Connection=True;";
            string connect3 = @"Server=Npgsql.EntityFrameworkCore.PostgreSQL;Database=company_directory;Trusted_Connection=True;";

            var options = optionsBuilder
                    .UseSqlServer(connect3)
                    .Options;


            using (ApplicationContext db = new ApplicationContext())
            {
                //return PartialView(@"~/Views/DeptView.cshtml", db.Depts.ToList().Where(x => x.ParentId == 2)); // View(db.Workers.ToList());
                return PartialView(@"~/Views/Home/DeptTreeTestView.cshtml", GetViewModel(db)); // View(db.Workers.ToList());
            }
        }


        [HttpGet]
        public ActionResult AddRootDept()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRootDept(string name)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Dept dept = new Dept
                {
                    ParentId = null,
                    Name = name
                };

                db.Depts.Add(dept);
                db.SaveChanges();

                return PartialView(@"~/Views/Home/DeptTreeTestView.cshtml", GetViewModel(db));
            }
        }

        [HttpGet]
        public ActionResult AddDept(int? Id)
        {
            Dept dept = new Dept
            {
                ParentId = Id,
                Name = ""
            };

            return View(dept);
        }

        [HttpPost]
        public ActionResult AddDept(int? parentId, string name)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Dept dept = new Dept
                {
                    ParentId = parentId,
                    Name = name
                };

                db.Depts.Add(dept);
                db.SaveChanges();

                return PartialView(@"~/Views/Home/DeptTreeTestView.cshtml", GetViewModel(db));
            }
        }


        

        [HttpGet]
        public ActionResult EditDept(int Id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {

                Dept dept = db.Depts.ToList().Where(x => x.Id == Id).FirstOrDefault();

                if (dept == null)
                    return View();  // TODO: or error

                // Получаем список дочерних элементов и отдела редактирования.
                List<Dept> deptAndChiles = new List<Dept>();
                deptAndChiles.Add(dept);
                for (int i = 0; i < deptAndChiles.Count; i++)
                {
                    IsGetChildsDept2(db, deptAndChiles[i], deptAndChiles);
                }

                // Необходимо и достаточно исключить из списка допустимых изменений родительского
                // отдела, сам отдел и его потомков.
                List<Dept> deptsValid = db.Depts.ToList();
                
                foreach (Dept deptNotValid in deptAndChiles)
                {
                    deptsValid.Remove(deptNotValid);
                }

                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (Dept deptValid in deptsValid)
                {
                    selectListItems.Add(new SelectListItem { Value = deptValid.Id.ToString(), Text = deptValid.Name });
                }

                DeptsValidEditListModel deptsValidEditListModel = new DeptsValidEditListModel()
                {
                    DeptEdit = dept,
                    DeptsValid = deptsValid,
                    DeptsValidItem = selectListItems

                };

                return View(deptsValidEditListModel);
            }

        }

        
        private void IsGetChildsDept2(ApplicationContext db, Dept dept, List<Dept> deptsList)
        {

            List<Dept> childsDept = db.Depts.Where(x => x.ParentId == dept.Id).ToList();
            deptsList.AddRange(childsDept);

        }


        [HttpPost]
        public ActionResult EditDept(int Id, int? parentId, string name)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Dept dept = db.Depts.ToList().Where(x => x.Id == Id).FirstOrDefault();

                dept.ParentId = parentId;
                dept.Name = name;

                db.SaveChanges();

                return PartialView(@"~/Views/Home/DeptTreeTestView.cshtml", GetViewModel(db));
            }
        }
        
        [HttpGet]
        public JsonResult CheckNameDept(string name)
        {

            using (ApplicationContext db = new ApplicationContext())
            {
                bool result = false;

                if (db.Depts.Where(x => x.Name == name).FirstOrDefault() == null)
                    result = true;

                return Json(result);
            }
        }

        [HttpGet]
        public ActionResult IsDeleteDept(int Id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var dept = db.Depts.SingleOrDefault(x => x.Id == Id);

                if (dept == null)
                    return View();

                return View(dept);
            }
        }
        
        // TODO: rename
        [HttpPost]
        public ActionResult DeleteDept(int Id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var deptsToRemove = db.Depts.SingleOrDefault(x => x.Id == Id);

                if (deptsToRemove != null)
                {
                    _DeptAndChildsToRemove.Clear();
                    _DeptAndChildsToRemove.Add(deptsToRemove);

                    // Получение всех дочерних подразделений.
                    for (int i = 0; i < _DeptAndChildsToRemove.Count; i++)
                    {
                        if (!IsGetChildsDept(db, _DeptAndChildsToRemove[i], _DeptAndChildsToRemove))
                            break;
                    }

                    // Получение всех удаляемых работников в данных подразделениях.
                    List<Worker> workersToRemove = db.Workers.Where(x => _DeptAndChildsToRemove.Where(y => y.Id == x.DeptId).Count() > 0).ToList();

                    foreach(Worker worker in workersToRemove)
                        db.Workers.Remove(worker);

                    foreach (Dept dept in _DeptAndChildsToRemove)
                        db.Depts.Remove(dept);

                    db.SaveChanges();
                }
                // TODO: else{ error }

                return PartialView(@"~/Views/Home/DeptTreeTestView.cshtml", GetViewModel(db));
            }
        }

        // Вспомогательная функция для поиска всех первоуровневых дочерних элементов указанного отдела.
        private List<Dept> _DeptAndChildsToRemove = new List<Dept>();
        private bool IsGetChildsDept(ApplicationContext db, Dept dept, List<Dept> deptsList)
        {

            List<Dept> childsDept = db.Depts.Where(x => x.ParentId == dept.Id).ToList();
            if (childsDept.Count == 0)
                return false;

            deptsList.AddRange(childsDept);
            return true;

        }


        // Обработчик нажатия на "Показать данные Вложенных узлов".
        [HttpGet]
        public ActionResult ShowWorkersByDeptId(int deptId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                _deptHost = db.Depts.Where(x => x.Id == deptId).FirstOrDefault();

                return PartialView(@"~/Views/Home/DeptTreeTestView.cshtml", GetViewModel(db));
            }
        }

        [HttpGet]
        public ActionResult AddWorker(int deptId)
        {
            Worker worker = new Worker
            {
                DeptId = deptId,
            };

            return View(worker);
        }

        [HttpPost]
        public ActionResult AddWorker(int deptId, string fullName, int positionId, string telephoneNumber, string email, int genderType)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Worker worker = new Worker
                {
                    DeptId = deptId,
                    FullName = fullName,
                    PositionId = positionId,
                    TelephoneNumber = telephoneNumber,
                    Email = email,
                    GenderType = genderType

                };

                db.Workers.Add(worker);
                db.SaveChanges();

                return PartialView(@"~/Views/Home/DeptTreeTestView.cshtml", GetViewModel(db));
            }
        }
        

        [HttpGet]
        public ActionResult EditWorker(int Id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {

                Worker worker = db.Workers.ToList().Where(x => x.Id == Id).FirstOrDefault();

                if (worker == null)
                    return View();

                return View(worker);
            }

        }

        [HttpPost]
        public ActionResult EditWorker(int id, int deptId, string fullName, int positionId, string telephoneNumber, string email, int genderType)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Worker worker = db.Workers.ToList().Where(x => x.Id == id).FirstOrDefault();

                worker.DeptId = deptId;
                worker.FullName = fullName;
                worker.PositionId = positionId;
                worker.TelephoneNumber = telephoneNumber;
                worker.Email = email;
                worker.GenderType = genderType;

                db.SaveChanges();

                return PartialView(@"~/Views/Home/DeptTreeTestView.cshtml", GetViewModel(db));
            }
        }



        [HttpGet]
        public ActionResult IsDeleteWorker(int Id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var worker = db.Workers.SingleOrDefault(x => x.Id == Id);

                if (worker == null)
                    return View();

                return View(worker);
            }
        }
        
        [HttpPost]
        public ActionResult DeleteWorker(int Id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {

                var workerToRemove = db.Workers.SingleOrDefault(x => x.Id == Id);

                if (workerToRemove != null)
                {
                    db.Workers.Remove(workerToRemove);
                    db.SaveChanges();
                }

                return PartialView(@"~/Views/Home/DeptTreeTestView.cshtml", GetViewModel(db));
            }
        }

        private bool _isShowDataDepthNodes = false;
        private Dept _deptHost = null;

        [HttpPost]
        public ActionResult SetShowDataDepthNodes(bool IsShowDataDepthNodes)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                _isShowDataDepthNodes = IsShowDataDepthNodes;

                return PartialView(@"~/Views/Home/DeptTreeTestView.cshtml", GetViewModel(db));
            }
                
        }

        Models.DeptsListModel GetViewModel(ApplicationContext db)
        {

            // Выводим только подразделения.
            if (_deptHost == null)
            {
                Models.DeptsListModel modelAllWorker = new Models.DeptsListModel()
                {
                    Depts = db.Depts.ToArray()
                };

                return modelAllWorker;
            }


            // Выводим работников подразделения в отделе.
            List<Worker> workers = new List<Worker>();
            
            if (_isShowDataDepthNodes)
            {
                // Получаем работников указанного отдела и дочерних.
                List<Dept> deptAndChilds = new List<Dept>();
                deptAndChilds.Add(_deptHost);
                for (int i = 0; i < deptAndChilds.Count; i++)
                {
                    if (!IsGetChildsDept(db, deptAndChilds[i], deptAndChilds))
                        break;
                }

                foreach(Dept dept in deptAndChilds)
                {
                    workers.AddRange(db.Workers.Where(x => x.DeptId == dept.Id).ToList());
                }
                
            }
            else
            {
                // Получаем работников указанного отдела.
                workers = db.Workers.Where(x => x.DeptId == _deptHost.Id).ToList();
            }
            
            Models.DeptsListModel modelWorkerIdDept = new Models.DeptsListModel()
            {
                IsShowDataDepthNodes = _isShowDataDepthNodes,
                Workers = workers,
                Depts = db.Depts.ToArray()
            };

            return modelWorkerIdDept;
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
