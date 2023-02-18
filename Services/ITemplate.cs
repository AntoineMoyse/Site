using System.Threading.Tasks;

namespace Site.Services
{
    public interface ITemplate
    {
        Task<string> RenderAsync<TViewModel>(string templateFileName, TViewModel viewModel);
    }
}