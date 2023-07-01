using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.WebUI.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using shopapp.WebUI.Extensions;
using Microsoft.AspNetCore.Identity;
using shopapp.WebUI.Identity;

namespace shopapp.WebUI.Controllers
{
    //hayrettin, zeynep, deniz => admin
    //muratdal=> customer
    [Authorize(Roles ="admin")]
    public class AdminController: Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public AdminController(
            IProductService productService,
            ICategoryService categoryService, 
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager= userManager;
        } 
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user!=null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i=>i.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailModel(){
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("~/admin/user/list");
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailModel model, string[] selectedRoles)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if(user!=null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName= model.LastName;
                    user.UserName= model.UserName;
                    user.Email= model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);
                    if(result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles??new string[]{};
                        await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());

                        return Redirect("/admin/user/list");
                    }
                }
                return Redirect("/admin/user/list");
            }
            return View(model);
        }

        public IActionResult UserList(){
            return View(_userManager.Users);
        }
        public async Task<IActionResult> RoleEdit(string id)
        {
            var role  = await _roleManager.FindByIdAsync(id);
            var members = new List<User>();
            var nonMembers = new List<User>();
            
            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name)?members:nonMembers;
                list.Add(user); 
            }
            var model =new RoleDetails()
            {
                Role = role, 
                Members = members,
                NonMembers = nonMembers
            };
            
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if(ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user!=null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if(!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }     

                foreach (var userId in model.IdsToDelete ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user!=null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if(!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                } 
            }

            return Redirect("/admin/role/"+model.RoleId);
        }
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if(result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            } 
            return View(model);
        }
        public IActionResult ProductList()
        {
            return View(new ProductListViewModel(){
                Products = _productService.GetAll()
            });
        }

        public IActionResult CategoryList()
        {
            return View(new CategoryListViewModel(){
                Categories = _categoryService.GetAll()
            });
        }

        [HttpGet]
        public IActionResult CreateProduct(){
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            if(ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name=model.Name,
                    Url=model.Url,
                    Price= (double)model.Price,
                    Description=model.Description,
                    ImageUrl=model.ImageUrl,
                    IsApproved=false,
                    IsHome=false
                    
                }; 

                if(_productService.Create(entity))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Kayıt Eklendi",
                        Message = "Kayıt başarıyla eklendi.",
                        AlertType = "success"
                    });
                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Hata",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
                return View(model);
            } 
            return View(model);
        }

        [HttpGet]
        public IActionResult ProductEdit(int? id){
            if(id==null)
            {
                return NotFound();
            }
            var entity =_productService.GetByIdWithCategories((int)id);
            if(entity==null)
            {
                return NotFound();

            }
            var model = new ProductModel()
            {
                ProductId=entity.ProductId,
                Name=entity.Name,
                Url=entity.Url,
                ImageUrl=entity.ImageUrl,
                Price=entity.Price,
                Description=entity.Description,
                IsApproved=entity.IsApproved,
                IsHome=entity.IsHome,
                SelectedCategories=entity.ProductCategories.Select(i=>i.Category).ToList() 
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryId, IFormFile file)
        {
            if(ModelState.IsValid)
            {
                var entity = _productService.GetById(model.ProductId);
                if(entity==null){
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Price = (double)model.Price;
                entity.Url = model.Url;
                entity.Description = model.Description;
                entity.IsApproved = model.IsApproved;
                entity.IsHome= model.IsHome;

                if(file!=null)
                {
                    entity.ImageUrl = file.Name;
                    var extention = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", file.FileName);
                    
                    using(var stream = new FileStream(path,FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                if(_productService.Update(entity, categoryId))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Güncelleme başarılı",
                        Message = "Kayıt başarıyla güncellendi",
                        AlertType = "success"
                    });
                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Hata",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
                
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);

            if(entity!=null){
                _productService.Delete(entity);
            }

            TempData.Put("message", new AlertMessage()
            {
                Title = "Silinme başarılı",
                Message = $"{entity.Name} isimli ürün silindi.",
                AlertType = "danger"
            });
            
            return RedirectToAction("ProductList");
        }


        //category Funtions

        [HttpGet]
        public IActionResult CreateCategory(){
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            if(ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name=model.Name,
                    Url=model.Url,
                    
                };  
                _categoryService.Create(entity);

                TempData.Put("message", new AlertMessage()
                {
                    Title = "Kategori eklendi",
                    Message = $"{entity.Name} isimli kategori eklendi.",
                    AlertType = "success"
                });
                
                return RedirectToAction("CategoryList");
            }
            
            return View(model);
        }
        [HttpGet]
        public IActionResult CategoryEdit(int? id){
            if(id==null)
            {
                return NotFound();
            }
            var entity =_categoryService.GetByIdWithProducts((int)id);
            if(entity==null)
            {
                return NotFound();
            }
            var model = new CategoryModel(){
                CategoryId=entity.CategoryId,
                Name=entity.Name,
                Url=entity.Url,
                Products = entity.ProductCategories.Select(p=>p.Product).ToList()
                
            };
            
            return View(model);
        }

        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            if(ModelState.IsValid)
            {
                var entity = _categoryService.GetById(model.CategoryId);

                if(entity==null){
                    return NotFound();
                }

                entity.Name =model.Name;
                entity.Url =model.Url;

                _categoryService.Update(entity);
                
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Kategori güncellendi",
                    Message = $"{entity.Name} isimli kategori güncellendi.",
                    AlertType = "success"
                });
               
                return RedirectToAction("CategoryList");
            }

            return View(model);
        }
      
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);
            if(entity!=null){
                _categoryService.Delete(entity);
            }
            
            TempData.Put("message", new AlertMessage()
            {
                Title = "Kategori silindi",
                Message = $"{entity.Name} isimli kategori silindi.",
                AlertType = "danger"
            });
               
            
            return RedirectToAction("CategoryList");
        }
        
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            TempData.Put("message", new AlertMessage()
            {
                Title = "Kategoriden silindi",
                Message = $"Ürün kategoriden silindi.",
                AlertType = "warning"
            });
               
            
            return Redirect("/admin/categories/"+categoryId);
        }

        
    }
}