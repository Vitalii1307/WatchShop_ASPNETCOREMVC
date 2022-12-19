using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Reflection.Metadata;
using WatchShop_ASPNETCOREMVC.Data;
using WatchShop_ASPNETCOREMVC.Models;
using WatchShop_ASPNETCOREMVC.Models.Domain;
using WatchShop_ASPNETCOREMVC.ViewModels;
using Constants = WatchShop_ASPNETCOREMVC.Data.Constants;

namespace WatchShop_ASPNETCOREMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly MVCWatchesDbContext mvcWatchDbContext;
        public ShopController(MVCWatchesDbContext mvcWatchDbContext, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            this.mvcWatchDbContext = mvcWatchDbContext;
        }

        
        [HttpGet]
         
        [Authorize(Roles = $"{Constants.Roles.Administrator},{Constants.Roles.User}")]
        public async Task<IActionResult> Index(string name, int company = 0, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 3;

            IQueryable<Watch> watches = mvcWatchDbContext.Watches.Include(x => x.Company);

            if (company != 0)
            {
                watches = watches.Where(p => p.CompanyId == company);
            }
            if (!string.IsNullOrEmpty(name))
            {
                watches = watches.Where(p => p.Name!.Contains(name));
            }
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    watches = watches.OrderByDescending(s => s.Name);
                    break;
                case SortState.PriceAsc:
                    watches = watches.OrderBy(s => s.Price);
                    break;
                case SortState.PriceDesc:
                    watches = watches.OrderByDescending(s => s.Price);
                    break;
                case SortState.CompanyNameAsc:
                    watches = watches.OrderBy(s => s.CompanyName);
                    break;
                case SortState.CompanyNameDesc:
                    watches = watches.OrderByDescending(s => s.CompanyName);
                    break;
                default:
                    watches = watches.OrderBy(s => s.Name);
                    break;
            }

            var count = await watches.CountAsync();
            var items = await watches.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            IndexViewModel viewModel = new IndexViewModel
                (
                items,
                new PageViewModel(count, page, pageSize),
                new FilterViewModel(mvcWatchDbContext.Companies.ToList(), company, name),
                new SortViewModel(sortOrder)
                );
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public async Task<IActionResult> Edit(int Id)
        {
            var watch = await mvcWatchDbContext.Watches.FirstOrDefaultAsync(x => x.Id == Id);
            if (watch != null)
            {
                var viewModel = new UpdateWatchViewModel()
                {
                    Id = watch.Id,
                    Name = watch.Name,
                    ImageName = watch.ImageName,
                    Price = watch.Price,
                };
                return await Task.Run(() => View("Edit", viewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateWatchViewModel model)
        {
            ModelState.Remove("ImageName");
            ModelState.Remove("ImageFile");
            if (!ModelState.IsValid)
            {
                //ViewBag.Id = model.Id;
                return View(model);
                //return RedirectToAction("Index");
            }

            var watch = await mvcWatchDbContext.Watches.FindAsync(model.Id);

            if (watch != null)
            {
                watch.Name = model.Name;

                if (model.ImageFile != null)
                {
                    DeleetImageFromPath(watch);
                    watch.ImageFile = model.ImageFile;
                    await SaveImageToPath(watch);
                }
                watch.Price = model.Price;

                await mvcWatchDbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateWatchViewModel model)
        {
            var watch = await mvcWatchDbContext.Watches.FindAsync(model.Id);

            if (watch != null)
            {

                DeleetImageFromPath(watch);
                mvcWatchDbContext.Watches.Remove(watch);
                var watchWithBelongToCurrentCompany = mvcWatchDbContext.Watches.Where(p => p.CompanyName == watch.CompanyName).ToList();
                if (watchWithBelongToCurrentCompany.Count == 1)
                {
                    var comp = mvcWatchDbContext.Companies.Where(c => c.Name == watch.CompanyName).ToList();
                    mvcWatchDbContext.Companies.Remove(comp[0]);
                }
                await mvcWatchDbContext.SaveChangesAsync();

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddWatchViewModel addWatchRequest)
        {
            var watch = new Watch()
            {
                //Id = Guid.NewGuid(),
                Name = addWatchRequest.Name,
                Price = addWatchRequest.Price,
                ImageFile = addWatchRequest.ImageFile,
            };

            List<Company> companies = mvcWatchDbContext.Companies.ToList();
            if (companies.Count != 0)
            {
                foreach (var item in companies)
                {
                    if (item.Name == addWatchRequest.CompanyName)
                    {
                        watch.Company = await mvcWatchDbContext.Companies.FirstOrDefaultAsync(c => c.Name == addWatchRequest.CompanyName);
                        watch.CompanyName = addWatchRequest.CompanyName;
                    }
                    else
                    {
                        var newComp = new Company { Name = addWatchRequest.CompanyName };
                        watch.Company = newComp;
                        watch.CompanyName = newComp.Name;
                    }
                }
            }
            else
            {
                watch.Company = new Company { Name = addWatchRequest.CompanyName };
                watch.CompanyName = addWatchRequest.CompanyName;
            }


            if (watch.ImageFile != null)
            {
                await SaveImageToPath(watch);
            }

            await mvcWatchDbContext.Watches.AddAsync(watch);
            await mvcWatchDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task SaveImageToPath(Watch watch)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(watch.ImageFile.FileName);
            string extension = Path.GetExtension(watch.ImageFile.FileName);
            watch.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/Image/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await watch.ImageFile.CopyToAsync(fileStream);
            }
        }
        public void DeleetImageFromPath(Watch watch)
        {
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", watch.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckPrice(int price)
        {
            if (price < 0)
                return Json(false);
            return Json(true);
        }
    }
}
