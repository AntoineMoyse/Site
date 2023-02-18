using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Site.Models.ApplicationUser;

namespace Site.Areas.Identity.Pages.Account.Manage
{
    public class Disable2faModel : PageModel
    {
        private readonly UserManager<Client> _userManager;
        private readonly ILogger<Disable2faModel> _logger;

        public Disable2faModel(
            UserManager<Client> userManager,
            ILogger<Disable2faModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de trouver l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            {
                throw new InvalidOperationException($"Impossible de désactiver la double authentification pour l'utilisateur avec l'ID '{_userManager.GetUserId(User)}' car elle n'est pas activée.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Impossible de trouver l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new InvalidOperationException($"Une erreur innatendu a eu lieu pendant la désactivation de la double authentification pour l'utilisateur avec l'ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("L'utilisateur avec l'ID '{UserId}' a désactivé la double authentification.", _userManager.GetUserId(User));
            StatusMessage = "La double authentification a été désactivé. Vous pouvez la réactiver quand vous le souhaitez avec une application de double authentification.";
            return RedirectToPage("./TwoFactorAuthentication");
        }
    }
}