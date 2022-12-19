using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using WatchShop_ASPNETCOREMVC.Core.Repositories;
using WatchShop_ASPNETCOREMVC.Data;
using WatchShop_ASPNETCOREMVC.ViewModels;
using static WatchShop_ASPNETCOREMVC.Data.Constants;

namespace WatchShop_ASPNETCOREMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Index()
        {
            var users = _unitOfWork.User.GetUsers();
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = _unitOfWork.User.GetUser(id);
            var roles = _unitOfWork.Role.GetRoles();

            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);


            var roleItems = roles.Select(role =>
                new SelectListItem(
                    role.Name,
                    role.Id,
                    userRoles.Any(ur => ur.Contains(role.Name)))).ToList();

            var vm = new EditUserViewModel
            {
                User = user,
                Roles = roleItems
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(EditUserViewModel data)
        {
            var user = _unitOfWork.User.GetUser(data.User.Id);
            if (user == null)
            {
                return NotFound();
            }

            var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);

            //Loop through the roles in ViewModel 
            //Check if the Role is Assingned in DB
            //If Assigned -> Do Nothing
            //If Not Assigned -> Add Role

            var rolesToAdd = new List<string>();
            var rolesToDelete = new List<string>();
            
            foreach (var role in data.Roles)
            {
                var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Text);
                if (role.Selected)
                {
                    if (assignedInDb == null)
                    {
                        rolesToAdd.Add(role.Text);
                    }
                }
                else
                {
                    if (assignedInDb != null)
                    {
                        rolesToDelete.Add(role.Text);
                    }
                }
            }

            if (rolesToAdd.Any())
            {
                //Add role
                await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
            }
            if (rolesToDelete.Any())
            {
                //Remove role
                await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToDelete);
            }

            user.FirstName = data.User.FirstName;
            user.LastName = data.User.LastName;
            user.Email = data.User.Email;

            _unitOfWork.User.UpdateUser(user);

            return RedirectToAction("Edit", new { id = user.Id });
        }
    }
}
