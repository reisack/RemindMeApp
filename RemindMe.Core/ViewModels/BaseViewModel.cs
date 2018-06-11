using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemindMe.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel, IDisposable
    {
        public BaseViewModel()
        {
            
        }

        public IMvxLanguageBinder TextSource =>
            new MvxLanguageBinder("", GetType().Name);

        protected async Task ReloadDataAsync()
        {
            try
            {
                await InitializeAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected virtual Task InitializeAsync()
        {
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            
        }
    }
}
