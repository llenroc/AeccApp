﻿using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace AeccApp.Core.Services
{
    public static class ServiceLocator
    {
        public static void RegisterDependencies()
        {
            // Services
            IocContainer.RegisterAsSingleton<INavigationService, NavigationService>();
            IocContainer.RegisterAsSingleton<IChatService, ChatService>();
            IocContainer.RegisterAsSingleton<IIdentityService, IdentityService>();

            IocContainer.RegisterAsSingleton<IHospitalRequestService, HospitalRequestService>();
            IocContainer.RegisterAsSingleton<IMapPinsDataService, MapPinsDataService>();
            IocContainer.RegisterAsSingleton<IHomeAddressesDataService, HomeAddressesDataService>();
            IocContainer.RegisterAsSingleton<IUserService, UserService>();
            IocContainer.RegisterAsSingleton<IHomeRequestService, HomeRequestService>();
            IocContainer.RegisterAsSingleton<IHomeAddressesDataService, HomeAddressesDataService>();
            IocContainer.RegisterAsSingleton<IGoogleMapsService, GoogleMapsService>();
            IocContainer.RegisterAsSingleton<IGoogleMapsService, GoogleMapsService>();
            IocContainer.Register<IHttpRequestProvider, HttpRequestProvider>();
        }

        static INavigationService _navigationService;
        public static INavigationService NavigationService
        {
            set { _navigationService = value; }
            get
            {
                return (_navigationService != null) ?
                  _navigationService :
                  Resolve<INavigationService>();
            }
        }

        public static IChatService ChatService
        {
            get { return Resolve<IChatService>(); }
        }

        public static IIdentityService IdentityService
        {
            get { return Resolve<IIdentityService>(); }
        }

        public static IHomeAddressesDataService HomeAddressesDataService
        {
            get { return Resolve<IHomeAddressesDataService>(); }
        }

        public static IHospitalRequestService HospitalRequestService
        {
            get { return Resolve<IHospitalRequestService>(); }
        }
        public static IMapPinsDataService MapPinsDataService
        {
            get { return Resolve<IMapPinsDataService>(); }
        }

        static IHomeRequestService _homeRequestService;
        public static IHomeRequestService HomeRequestService
        {
            set { _homeRequestService = value; }
            get
            {
                return (_homeRequestService != null) ?
                  _homeRequestService :
                  Resolve<IHomeRequestService>();
            }
        }
        public static IGeolocator GeolocatorService
        {
            get { return CrossGeolocator.Current; }
        }

        public static IPermissions PermissionsService
        {
            get { return CrossPermissions.Current; }
        }

        public static IGoogleMapsService GoogleMapsService
        {
            get { return Resolve<IGoogleMapsService>(); }
        }

        public static IFileProvider FileProvider
        {
            get { return DependencyService.Get<IFileProvider>(); }
        }

        private static T Resolve<T>()
        {
            return IocContainer.Resolve<T>();
        }
    }
}
