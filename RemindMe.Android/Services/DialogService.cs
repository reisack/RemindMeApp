using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using RemindMe.Core.Interfaces;

namespace RemindMe.Android.Services
{
    public class DialogService : IDialogService
    {
        protected Activity CurrentActivity =>
            Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        public Task ShowAlertAsync(string message,
            string title, string buttonText)
        {
            return Task.Run(() =>
            {
                Alert(message, title, buttonText);
            });
        }

        public Task<bool> ShowConfirmAsync(string message, string title, string yesButton, string noButton)
        {
            var tcs = new TaskCompletionSource<bool>();

            var builder = new AlertDialog.Builder(CurrentActivity);
            //builder.SetIconAttribute
            //    (Android.Resource.Attribute.AlertDialogIcon);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetPositiveButton(yesButton, (senderAlert, args) =>
            {
                tcs.SetResult(true);
            });
            builder.SetNegativeButton(noButton, (senderAlert, args) =>
            {
                tcs.SetResult(false);
            });
            builder.Create().Show();

            return tcs.Task;
        }

        private void Alert(string message, string title, string okButton)
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                var builder = new AlertDialog.Builder(CurrentActivity);
                //builder.SetIconAttribute
                //    (Android.Resource.Attribute.AlertDialogIcon);
                builder.SetTitle(title);
                builder.SetMessage(message);
                builder.SetPositiveButton(okButton, delegate { });
                builder.Create().Show();
            }, null);
        }
    }
}