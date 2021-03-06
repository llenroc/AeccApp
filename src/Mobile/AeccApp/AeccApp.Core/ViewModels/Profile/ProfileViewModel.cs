﻿using System.Threading.Tasks;
using AeccApp.Core.Services;
using Xamarin.Forms;
using System.Windows.Input;
using AeccApp.Core.ViewModels.Popups;
using System;
using AeccApp.Core.Messages;

namespace AeccApp.Core.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private IIdentityService IdentityService { get; } = ServiceLocator.IdentityService;
        private IChatService ChatService { get; } = ServiceLocator.ChatService;
  
        public override async Task ActivateAsync()
        {
            MessagingCenter.Send(new ToolbarMessage(false, LocalizationResourceManager.GetString("ChatViewViewVolunteerProfile")), string.Empty);
        }

        #region Properties
        public string Name
        {
            get { return GSetting.User?.Name; }
        }

        public string Description
        {
            get { return GSetting.User.Description; }
        }

        public string Email
        {
            get { return GSetting.User?.Email; }
        }

        public int? Age
        {
            get { return GSetting.User.Age; }
        }
        public string Gender
        {
            get { return GSetting.User.DisplayGender; }
        }

        public LogoutPopupViewModel LogoutPopupVM { get; private set; }

        #endregion

        #region Commands
        /// <summary>
        /// Show logout popup
        /// </summary>
       

        private Command _editProfileCommand;
        public ICommand EditProfileCommand
        {
            get
            {
                return _editProfileCommand ??
                    (_editProfileCommand = new Command(o => OnEditProfileAsync()));
            }
        }
        /// <summary>
        /// Opens webview to edit profile
        /// </summary>
        /// <returns></returns>
        private async Task OnEditProfileAsync()
        {
            await IdentityService.EditProfileAsync();
            NotifyPropertyChanged(nameof(Name));
            NotifyPropertyChanged(nameof(Email));
        }

        #endregion

      
    }
}
