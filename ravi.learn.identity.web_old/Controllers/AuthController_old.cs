//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using ravi.learn.identity.web.Models;
//using ravi.learn.identity.domain.Services;
//using ravi.learn.identity.domain.Entities;
//using System;
//using System.Security.Claims;
//using System.Collections.Generic;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.OpenIdConnect;

//namespace ravi.learn.identity.web.Controllers
//{
//    [Route("auth")]
//    public class AuthController : Controller
//    {
//        private readonly IUserService _userService;

//        public AuthController(IUserService userService)
//        {
//            _userService = userService;
//        }
//        [Route("signin")]
//        public IActionResult SignIn()
//       // public async Task<IActionResult> SignIn()
//        {
//            //return View(new SignInModel());
//            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "B2C_1_sign_in");

//            //var authResult = await HttpContext.AuthenticateAsync("Temporary");
//            //if (authResult.Succeeded)
//            //{
//            //    return RedirectToAction("Profile");
//            //}
//            //return View();           

//        }

//        [Route("signin/{provider}")]
//        public IActionResult SignIn(string provider, string returnUrl = null)
//        {
//            var redirectUrl = Url.Action("Profile");
//            redirectUrl += "?returnurl=" + (returnUrl ?? "/");
            
//            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, provider);
//        }

//        [Route("signin/Profile")]
//        public async Task<IActionResult> Profile(string returnUrl = null)
//        {
//            var authResult = await HttpContext.AuthenticateAsync("Temporary");
//            if (!authResult.Succeeded)
//            {
//                return RedirectToAction("SignIn");
//            }
//            var userId = authResult.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
//            var user = await _userService.GetUserById(userId);
//            if (user != null)
//            {
//                return await SignInUser(user, returnUrl);
//            }
//            var profileModel = new ProfileModel
//            {
//                DisplayName = authResult.Principal.Identity.Name,
//                Email = authResult.Principal.FindFirst(ClaimTypes.Email).Value
//            };
//            return View(profileModel);

//        }

//        [HttpPost]
//        [Route("signin/Profile")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Profile(ProfileModel profile, string returnUrl = null)
//        {
//            var authResult = await HttpContext.AuthenticateAsync("Temporary");
//            if (!authResult.Succeeded)
//            {
//                return RedirectToAction("SignIn");
//            }
//            if (ModelState.IsValid)
//            {
//                var user = await _userService.AddUser(authResult.Principal.FindFirst(ClaimTypes.NameIdentifier).Value, profile.DisplayName, profile.Email);
//                return await SignInUser(user, returnUrl);
//            }
//            return View(profile);
//        }

//        /* First chapter
//        [Route("signin")]
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> SignIn(SignInModel signInModel, string returnUrl = null)
//        {
//            if (ModelState.IsValid)
//            {
//                User user = null;
//                if (await _userService.ValidateCredentials(signInModel.UserName, signInModel.Password, out user))
//                {
//                    await SignInUser(user.UserName);
//                    if (returnUrl != null)
//                    {
//                        return Redirect(returnUrl);
//                    }
//                    return RedirectToAction("Index", "Home");
//                }

//            }
//            return View(signInModel);
//        }
//        */

//        [Route("signup")]        
//        public  IActionResult SignUp()
//        {
//            //return View(new SignUpModel());
           
//                var challengeResult = Challenge(new AuthenticationProperties { RedirectUri = "/" }, "B2C_1_sign_up");
//            return challengeResult;
//        }

//        [Route("editprofile")]
//        public IActionResult EditProfile()
//        {
//            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "B2C_1_edit_profile");
//        }


//        //[Route("signup")]
//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> SignUp(SignUpModel signUpModel)
//        //{
//        //    if (ModelState.IsValid)
//        //    {
//        //        if (await _userService.AddUser(signUpModel.UserName, signUpModel.Password))
//        //        {
//        //            await SignInUser(signUpModel.UserName);
//        //            return RedirectToAction("Index", "Home");
//        //        }
//        //        ModelState.AddModelError("Error", "Username exists. Please choose another one");
//        //    }
//        //    return View(signUpModel);
//        //}

//        [Route("signout")]
//        [HttpPost]
//        //public async Task<IActionResult> SignOut()
//        public async Task SignOut()
//        {
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            var scheme = User.FindFirst("tfp").Value;
//            await HttpContext.SignOutAsync(scheme);
//          //  await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
//            //await HttpContext.SignOutAsync("Temporary");
//            //return RedirectToAction("Index", "Home");
//        }

//       // private async Task SignInUser(string userName, string returnUrl)
//        private async Task<IActionResult> SignInUser(User user, string returnUrl)
//        {
//            await HttpContext.SignOutAsync("Temporary");
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.Id),
//                new Claim(ClaimTypes.Name, user.DisplayName),
//                new Claim(ClaimTypes.Email, user.Email)
//            };

//            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, null);
//            var principal = new ClaimsPrincipal(identity);
//            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);

//            return Redirect(returnUrl ?? "/");

//        }
//    }
//}