using RemindMe.Core.Interfaces;
using System.Threading.Tasks;

namespace RemindMe.Test.Mocks
{
    public class DialogMockService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string buttonText)
        {
            return Task.CompletedTask;
        }

        public Task<bool> ShowConfirmAsync(string message, string title, string yesButton, string noButton)
        {
            return Task.Run(() => { return true; });
        }
    }
}
