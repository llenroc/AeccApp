﻿using Aecc.Models;
using AeccApp.Core.Models;
using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels
{
    public class NewsDetailViewModel :  ViewModelBase
    {
    
        #region Contructor & Initialize
        public override Task InitializeAsync(object navigationData)
        {
            currentNew = navigationData as NewsModel;

            if (CurrentNew != null)
            {
                CurrentNewContent = CurrentNew.Content;
                CurrentNewTitle = CurrentNew.Title;
                CurrentNewImagen = CurrentNew.Imagen;
            }

            return Task.CompletedTask;
        }
        #endregion

        #region Propeties
        private NewsModel currentNew;
        public NewsModel CurrentNew
        {
            get { return currentNew; }
            set { currentNew = value; }
        }

        private string currentNewTitle;
        public string CurrentNewTitle
        {
            get { return currentNewTitle; }
            set
            {
                currentNewTitle = value;
                NotifyPropertyChanged(nameof(CurrentNewTitle));
            }
        }
        private string currentNewImagen;

        public string CurrentNewImagen {
            get { return currentNewImagen; }
            set
            {
                currentNewImagen = value;
                NotifyPropertyChanged(nameof(CurrentNewImagen));
            }
        }

        private string currentNewContent;
        public string CurrentNewContent
        {
            get { return currentNewContent; }
            set
            {
                currentNewContent = value;
                NotifyPropertyChanged(nameof(CurrentNewContent));
            }
        }
        #endregion
    }
}
