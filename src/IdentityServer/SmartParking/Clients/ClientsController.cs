using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerHost.SmartParking.UI
{
    public class ClientsController : Controller
    {
        private readonly ConfigurationDbContext dbContext;

        public ClientsController(ConfigurationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // GET: ClientsController
        public async Task<ActionResult> Index()
        {
            var clients = await dbContext.Clients
                .Include(x=>x.AllowedScopes)
                .Include (x=>x.AllowedGrantTypes)
                .Include (x=> x.AllowedCorsOrigins)
                .Include (x=>x.RedirectUris)
                .ToListAsync();
            var vm = BuildViewModelAsync(clients);
            return View(vm);
        }

        private IEnumerable<ClientsViewModel> BuildViewModelAsync(List<Client> clients)
        {
            //IdentityServer4.Models.Client client = new IdentityServer4.Models.Client();
            return clients.Select(x => new ClientsViewModel
            {
                ClientId = x.ClientId,
                ClientName = x.ClientName,
                AllowedGrantTypes = x.AllowedGrantTypes?.Select(y => y.GrantType) ?? new List<string>(),
                AccessTokenLifetime = x.AccessTokenLifetime,
                AllowedCorsOrigins = x.AllowedCorsOrigins?.Select(y => y.Origin) ?? new List<string>(),
                RedirectUris = x.RedirectUris?.Select(y => y.RedirectUri) ?? new List<string>(),
                AllowedScopes = x.AllowedScopes?.Select(y => y.Scope) ?? new List<string>(),
                RequirePkce = x.RequirePkce,
                RequireClientSecret = x.RequireClientSecret,
            });
        }

        // GET: ClientsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }


        // POST: ClientsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClientsViewModel clientsViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = BuildModel(clientsViewModel);
                    await dbContext.Clients.AddAsync(client.ToEntity());
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View();
        }

        private static IdentityServer4.Models.Client BuildModel(ClientsViewModel clientsViewModel)
        {
            return new IdentityServer4.Models.Client()
            {
                AccessTokenLifetime = clientsViewModel.AccessTokenLifetime,
                AllowAccessTokensViaBrowser = true,
                AllowedCorsOrigins = clientsViewModel.AllowedCorsOrigins.ToList(),
                AllowedGrantTypes = clientsViewModel.AllowedGrantTypes.ToList(),
                AllowedScopes = clientsViewModel.AllowedScopes.ToList(),
                ClientName = clientsViewModel.ClientName,
                ClientId = clientsViewModel.ClientId,
                PostLogoutRedirectUris = clientsViewModel.PostLogoutRedirectUris.ToList(),
                RedirectUris = clientsViewModel.RedirectUris.ToList(),
                RequirePkce = clientsViewModel.RequirePkce,
                RequireClientSecret = clientsViewModel.RequireClientSecret,
                RequireConsent = clientsViewModel.RequireConsent,
            };
        }

        // GET: ClientsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClientsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
