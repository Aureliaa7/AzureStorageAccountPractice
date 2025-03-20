using AzureTables.Data;
using AzureTables.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AzureTables.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IBlobStorageService blobStorageService;

        public UserController(IUserService userService, IBlobStorageService blobStorageService)
        {
            this.userService = userService;
            this.blobStorageService = blobStorageService;
        }

        // GET: UserController
        public async Task<ActionResult> Index()
        {
            var users = await userService.GetAllAsync();
            foreach (var user in users)
            {
                user.ImageName = await blobStorageService.GetUrlAsync(user.ImageName);
            }
            return View(users);
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(string country, string id)
        {
            var user = await userService.GetAsync(country, id);
            user.ImageName = await blobStorageService.GetUrlAsync(user.ImageName);

            return View(user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserEntity user, IFormFile formFile)
        {
            try
            {
                var id = Guid.NewGuid().ToString();
                user.PartitionKey = user.Country;
                user.RowKey = id;

                if (formFile?.Length > 0)
                {
                    user.ImageName = await blobStorageService.UploadAsync(formFile, id);
                }

                await userService.UpsertAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string country, string id)
        {
            var user = await userService.GetAsync(country, id);
            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string country, string id, UserEntity user, IFormFile formFile)
        {
            try
            {
                user.PartitionKey = user.Country;
                user.RowKey = id;
                if (formFile?.Length > 0)
                {
                    user.ImageName = await blobStorageService.UploadAsync(formFile, id);
                }
                await userService.UpsertAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string country, string id)
        {
            try
            {
                var item = await userService.GetAsync(country, id);
                await blobStorageService.RemoveAsync(item.ImageName);
                await userService.DeleteAsync(country, id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
