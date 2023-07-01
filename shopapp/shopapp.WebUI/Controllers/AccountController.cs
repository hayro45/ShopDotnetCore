using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.WebUI.EmailServices;
using shopapp.WebUI.Extensions;
using shopapp.WebUI.Identity;
using shopapp.WebUI.Models;

namespace shopapp.WebUI.Controllers
{
    public class AccountController:Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IMailSender _mailsender;
        private ICartService _cartService;
        public AccountController(ICartService cartService, UserManager<User> userManager, SignInManager<User> signInManager, IMailSender mailsender)
        {
            _userManager= userManager;
            _signInManager = signInManager;
            _mailsender = mailsender;
            _cartService= cartService;
        }
        public IActionResult Login(string ReturnUrl=null)
        {
            return View(new LoginModel(){
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user==null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı ile daha önce hesap oluşturulmamış.");
                return View(model);
            }
            
            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen hesabınızı email hesabınızdan gelen link ile onaylayınız.");
                return View(model);
            }

            var result =  await _signInManager.PasswordSignInAsync(user, model.Password,false, false);

            if(result.Succeeded)
            {
                return Redirect(model.ReturnUrl??"~/");
            }
            
            ModelState.AddModelError("", "Girilen parola veya kullanıcı adı yanlış.");

            return View(model);
        }


        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };         
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new {
                    userId = user.Id,
                    token = code
                });

                await _mailsender.SendEmailAsync(model.Email,"Hesabınızı onaylayınız.",$"Lütfen mail adresinizi onaylamak için linke <a href='https://localhost:5001{url}'>tıklayınız</a>.");
             
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu lütfen tekrar deneyiniz.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData.Put("message", new AlertMessage()
            {
                Title = "Çıkış Yapıldı",
                Message = "Hesabınızdan güvenli bir şekilde çıkış yapıldı.",
                AlertType = "warning"
            });

            return Redirect("~/");
        }
    
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(userId==null || token == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Geçersiz token",
                    Message = "Geçersiz token tespit edilmiştir.",
                    AlertType = "danger"
                });
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user!=null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if(result.Succeeded)
                {
                    _cartService.InitializeCart(user.Id);
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Hesabınız onaylandı",
                        Message = "Hesabınız onaylandı giriş yapabilirsiniz.",
                        AlertType = "success"
                    });
                    return View();
                }
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Hesabınız Onaylanmadı",
                Message = "Üzgünüz Bilinmeyen bir sebebten ötürü hesabınız onaylanmadı.",
                AlertType = "danger"
            });
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if(string.IsNullOrEmpty(Email))
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if(user==null)
            {
                return View();   
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            var url = Url.Action("ResetPassword", "Account", new {
                userId = user.Id,
                token = code
            });

            await _mailsender.SendEmailAsync(Email,"Reset Password.",$"Parolanızı yenilemek için linke <a href='https://localhost:5001{url}'>tıklayınız</a>.");
                
            return View();
        }

        public IActionResult ResetPassword(string userId, string token)
        {

            if(userId==null || token==null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model =  new ResetPasswordModel {Token=token};
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if(!ModelState.IsValid)  
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user==null)
            {
                return RedirectToAction("Index", "Home");

            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        public IActionResult AccesDenied()
        {
            return  View();
        }
    
    }
}