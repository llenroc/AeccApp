﻿using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels.Popups
{
    public class RequestSentPopupViewModel : ClosablePopupViewModelBase
    {
        protected override async Task OnClosePopupCommandAsync()
        {
            await NavigationService.HidePopupAsync();
            await NavigationService.NavigateToAsync<DashboardViewModel>();
        }

        public override bool OnBackButtonPressed()
        {
            OnClosePopupCommandAsync();
            return base.OnBackButtonPressed();
        }
    }
}
