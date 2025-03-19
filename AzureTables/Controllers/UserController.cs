using AzureTables.Data;
using Microsoft.AspNetCore.Mvc;

namespace AzureTables.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // GET: UserController
        public async Task<ActionResult> Index()
        {
            var users = await userService.GetAllAsync();
            return View(users);
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(string country, string id)
        {
            var user = await userService.GetAsync(country, id);
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
        public async Task<ActionResult> Create(UserEntity user)
        {
            try
            {
                user.PartitionKey = user.Country;
                user.RowKey = Guid.NewGuid().ToString();
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
        public async Task<ActionResult> Edit(string country, string id, UserEntity user)
        {
            try
            {
                user.PartitionKey = user.Country;
                user.RowKey = id;
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
