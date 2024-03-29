using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Site.Services
{
    public class RazorViewsTemplateService : ITemplate
    {
        private IRazorViewEngine _viewEngine;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly ILogger<RazorViewsTemplateService> _logger;

        public RazorViewsTemplateService(IRazorViewEngine viewEngine, IServiceProvider serviceProvider, ITempDataProvider tempDataProvider, ILogger<RazorViewsTemplateService> logger)
        {
            _viewEngine = viewEngine;
            _serviceProvider = serviceProvider;
            _tempDataProvider = tempDataProvider;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> RenderAsync<TViewModel>(string filename, TViewModel viewModel)
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };

            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            await using var outputWriter = new StringWriter();
            var viewResult = _viewEngine.FindView(actionContext, filename, false);
            var viewDictionary = new ViewDataDictionary<TViewModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = viewModel
            };

            var tempDataDictionary = new TempDataDictionary(httpContext, _tempDataProvider);
            if (!viewResult.Success)
            {
                throw new KeyNotFoundException(
                    $"Impossible de mettre en forme la page html car le template {filename} n'éxiste pas");
            }

            try
            {
                var viewContext = new ViewContext(actionContext, viewResult.View, viewDictionary,
                    tempDataDictionary, outputWriter, new HtmlHelperOptions());

                await viewResult.View.RenderAsync(viewContext);
                return outputWriter.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Impossible de mettre en forme la page html parce que erreur");
                return string.Empty;
            }
        }
    }
}