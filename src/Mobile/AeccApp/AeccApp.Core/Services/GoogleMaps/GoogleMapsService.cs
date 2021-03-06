﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AeccApp.Core.Models;
using System.Globalization;
using System.Threading;
using Xamarin.Forms.GoogleMaps;

namespace AeccApp.Core.Services
{
    public class GoogleMapsService : IGoogleMapsService
    {
        private readonly IHttpRequestProvider _requestProvider;
        private const string GOOGLE_MAPS_ENDPOINT = "https://maps.googleapis.com";

        private const string PROVINCE_AREA = "administrative_area_level";
        private const string NUMBER_AREA = "street_number";

        public GoogleMapsService(IHttpRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<IEnumerable<AddressModel>> FindPlacesAsync(string findText, Position position, CancellationToken cancelToken)
        {
            string query = $"input={findText}&language=es&types=address&components=country:es&key={GlobalSetting.Instance.GooglePlacesApiKey}";
            if (position.Latitude != 0)
            {
                query += $"&location={position.Latitude.ToString(CultureInfo.InvariantCulture)},{position.Longitude.ToString(CultureInfo.InvariantCulture)}";
            }

            UriBuilder uriBuilder = new UriBuilder(GOOGLE_MAPS_ENDPOINT)
            {
                Path = "maps/api/place/autocomplete/json",
                Query = query
            };

            var places = await _requestProvider.GetAsync<GooglePlacesModel>(uriBuilder.ToString(), cancelToken: cancelToken);

            if (places?.Predictions != null)
                return places.Predictions.Select(item => new AddressModel(item));
            else
                return new List<AddressModel>();
        }

        public async Task<AddressModel> FillPlaceDetailAsync(AddressModel address, CancellationToken cancelToken)
        {
            UriBuilder uriBuilder = new UriBuilder(GOOGLE_MAPS_ENDPOINT)
            {
                Path = "maps/api/place/details/json",
                Query = $"placeid={address.PlaceId}&language=es&key={GlobalSetting.Instance.GooglePlacesApiKey}"
            };

            GooglePlacesDetailModel place = await _requestProvider.GetAsync<GooglePlacesDetailModel>(uriBuilder.ToString(), cancelToken);
            FillAddressDetails(address, place);
            return address;
        }

        public void FillAddressDetails(AddressModel address, GooglePlacesDetailModel googlePlacesDetailModel)
        {
            Position addressCoordinates = new Position(googlePlacesDetailModel.Result.Geometry.Location.Lat, googlePlacesDetailModel.Result.Geometry.Location.Lng);
            address.Coordinates = addressCoordinates;
            var numberComponentValue = googlePlacesDetailModel.Result.AddressComponents.FirstOrDefault
                (c => c.Types.First() == NUMBER_AREA)?.LongName;
            if (int.TryParse(numberComponentValue, out int n))
            {
                address.Number = numberComponentValue;
            }
            address.Province = googlePlacesDetailModel.Result.AddressComponents.FirstOrDefault
                (c => c.Types.First().StartsWith(PROVINCE_AREA))?.LongName;
        }

        public async Task<Position> FindAddressGeocodingAsync(string address, CancellationToken cancelToken)
        {
            UriBuilder uriBuilder = new UriBuilder(GOOGLE_MAPS_ENDPOINT)
            {
                Path = "maps/api/geocode/json",
                Query = $"address={address}&language=es&key={GlobalSetting.Instance.GooglePlacesApiKey}"
            };

            GoogleGeocodingModel place = await _requestProvider.GetAsync<GoogleGeocodingModel>(uriBuilder.ToString(), cancelToken);
            var result = place.Results.FirstOrDefault();

            return (result != null) ?
                new Position(result.Geometry.Location.Lat, result.Geometry.Location.Lng) : new Xamarin.Forms.GoogleMaps.Position() ;
        }

        public async Task<AddressModel> FindCoordinatesGeocodingAsync(double lat, double lng, CancellationToken cancelToken)
        {
            UriBuilder uriBuilder = new UriBuilder(GOOGLE_MAPS_ENDPOINT)
            {
                Path = "maps/api/geocode/json",
                Query = $"latlng={lat.ToString(CultureInfo.InvariantCulture)},{lng.ToString(CultureInfo.InvariantCulture)}&language=es&key={GlobalSetting.Instance.GooglePlacesApiKey}"
            };

            GoogleGeocodingModel place = await _requestProvider.GetAsync<GoogleGeocodingModel>(uriBuilder.ToString(), cancelToken: cancelToken);

            var result = place.Results.FirstOrDefault();

            return (result != null) ?
                new AddressModel()
                {
                    Province = result.AddressComponents.FirstOrDefault
                        (c => c.Types.First().StartsWith(PROVINCE_AREA))?.LongName
                } : null;
        }
    }
}
