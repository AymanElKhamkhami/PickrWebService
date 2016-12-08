using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace JSONWebService
{
    public interface IServiceAPI
    {
        bool CreateUser(string Email, string Username, string Password, string FirstName, string Surname, DateTime Birth, string Gender, string Mobile, string Picture, string Address);

        bool CheckUserExistence(string Email);

        bool UserAuthentication(string Email, string Password);

        DataTable GetUser(string Email);

        DataTable GetUserPublic(int IdUser);

        bool UpdateUser(string Email, string FirstName, string Surname, DateTime Birth, string Gender, string Mobile, string Picture, string Address, string Mode, string CarModel);

        bool SetUserPreferences(string Email, bool Smoking, bool Music, bool Pets, int Talking);

        int CreatePreferences(bool Smoking, bool Music, bool Pets, int Talking);

        bool UpdatePreferences(int idPreferences, bool Smoking, bool Music, bool Pets, int Talking);

        bool CreateOffer(string Email, double StartLat, double StartLng, double DestinationLat, double DestinationLng, List<List<double>> Waypoints, List<List<double>> RoutePoints, List<List<double>> RangePolygon, DateTime Departure, object Arrival, int Seats, double Price, int Radius, double Distance, bool Active);

        DataTable GetOffersList(string Email);

        DataTable GetRequestedOffersList(List<int> OfferIds);

        bool UpdateOffer(int IdOffer, object StartLat, object StartLng, object DestinationLat, object DestinationLng, List<List<double>> Waypoints, List<List<double>> RoutePoints, List<List<double>> RangePolygon, object Departure, object Arrival, object Seats, object ReservedSeats, object Price, object Radius, object Distance, object Active);

        bool DeleteOffer(int IdOffer);

        bool CreateWaypointsList(int IdOffer, List<List<double>> Waypoints);

        bool CreateRoutePointsList(int IdOffer, List<List<double>> RoutePoints);

        bool CreatePolygon(int IdOffer, List<List<double>> RangePolygon);

        DataTable GetWaypointsList(int IdOffer);

        DataTable GetRoutePointsList(int IdOffer);

        DataTable GetRangeIndices(int IdOffer);

        bool DeleteWaypointsList(int IdOffer);

        bool DeletePolygon(int IdOffer);

        bool DeleteRoutePointsList(int IdOffer);

        DataTable SearchRides(DateTime ArrivalFrom, object ArrivalTo, double StartLat, double StartLng, double DestinationLat, double DestinationLng);

        bool CreateRequest(string Email, object IdOffer, double StartLat, double StartLng, double DestinationLat, double DestinationLng, DateTime ArrivalFrom, object ArrivalTo, int Seats);

        DataTable GetDriverReceivedRequests(string Email, bool Approved, bool Rejected);

        DataTable GetPassengerSentRequests(string Email);

        bool RespondToRequest(int IdRequest, object PickUp, bool Approved);

        bool CheckRequestExistence(string Email, int IdOffer);

        bool CheckRequestPending(string Email, int IdOffer);

        DataTable GetPassengerNotifications(List<object> SentRequestsIds);

        DataTable GetDriverNotifications(string Email);

        DataTable GetMenuNotifications(string Email);

        bool MarkNotificationsAsSeen(string Email);

    }
}