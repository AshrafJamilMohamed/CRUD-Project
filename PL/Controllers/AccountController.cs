using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PL.ViewModels;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManger;
        private readonly SignInManager<ApplicationUser> signInManager;

        // Ask CLR to inject object from UserManger Class [it's Service] To manage the User 

        public AccountController(UserManager<ApplicationUser> _UserManger,
            SignInManager<ApplicationUser> _signInManager)
        {
            userManger = _UserManger;
            signInManager = _signInManager;
        }
        // Register

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model == null) { return BadRequest(); }

            if (model.IsAgree == false)
            {
                ModelState.AddModelError(string.Empty, "must Agree on Terms");
                return View(model);
            }

            if (ModelState.IsValid) // Server Side Validation 
            {

                // Mapping
                var User = new ApplicationUser()
                {
                    FName = model.FName,
                    LName = model.LName,
                    Email = model.Email,
                    UserName = model.Email.Split('@')[0],
                    IsAgree = model.IsAgree


                };

                // add user

                var Result = await userManger.CreateAsync(User, model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        // Login

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var User = await userManger.FindByEmailAsync(model.Email);
                if (User != null)
                {
                    var Result = await userManger.CheckPasswordAsync(User, model.Password);

                    if (Result)
                    {
                        // PasswordSignInAsync => Will Generate TOKEN 
                        var LogeResult = await signInManager.PasswordSignInAsync(User, model.Password,false,false);

                        if (LogeResult.Succeeded)
                            // Login
                            return RedirectToAction("Index", "Home");

                    }
                    else
                        ModelState.AddModelError(string.Empty, "Invalid Password ");
                }
                else
                    ModelState.AddModelError(string.Empty, "Invalid Email ");
            }
            return View(model);

        }


        //Logout

        public async Task<IActionResult> Logout()
        {
           await signInManager.SignOutAsync(); // this will remove [TOKEN]

            return RedirectToAction(nameof(Login));
        }
        //Forget Password
        //Reset Password



    }
}
