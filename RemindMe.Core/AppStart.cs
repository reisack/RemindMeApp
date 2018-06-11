using MvvmCross.Core.ViewModels;
using RemindMe.Core.ViewModels;

namespace RemindMe.Core
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            ShowViewModel<ReminderListViewModel>();
        }
    }
}
