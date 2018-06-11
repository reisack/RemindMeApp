using System.Threading.Tasks;

namespace RemindMe.Core.Interfaces
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonText);
        Task<bool> ShowConfirmAsync(string message, string title, string yesButton, string noButton);
    }
}
