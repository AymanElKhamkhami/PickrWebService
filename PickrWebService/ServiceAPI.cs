using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace JSONWebService
{
    public class ServiceAPI : IServiceAPI
    {
        SqlConnection dbConnection;


        public ServiceAPI()
        {
            dbConnection = DBConnect.getConnection();

        }


        public bool CreateUser(string Email, string Username, string Password, string FirstName, string Surname, DateTime Birth, string Gender, string Mobile, string Picture, string Address)
        {
            bool created = false;



            if (!CheckUserExistence(Email))
            {
                if (dbConnection.State.ToString() == "Closed")
                {
                    dbConnection.Open();
                }

                string query = "INSERT INTO UserDetails(Email, Username, Password, FirstName, Surname, Birth, Gender, MemberSince, Mobile, Picture, Address, Mode) VALUES ('" + Email + "','" + Username + "','" + Password + "','" + FirstName + "','" + Surname + "','" + Birth.ToString("yyyy-MM-dd HH:mm") + "','" + Gender + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "','" + Mobile + "','" + Picture + "','" + Address + "','passenger');";
                SqlCommand command = new SqlCommand(query, dbConnection);
                int affected = command.ExecuteNonQuery();

                dbConnection.Close();

                if (affected > 0)
                {
                    created = true;
                    int userId = 0;
                    query = "SELECT IdUser FROM UserDetails WHERE Email='" + Email + "';";

                    dbConnection.Open();
                    command = new SqlCommand(query, dbConnection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        userId = Convert.ToInt32(reader["IdUser"]);

                    dbConnection.Close();

                    CreateMenuNotification(userId, "welcome", "logo", DateTime.Now);
                }
                    
            }

            

            return created;
        }


        public bool CheckUserExistence(string Email)
        {
            bool exists = false;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT IdUser FROM UserDetails WHERE Email='" + Email + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                exists = true;
            }

            reader.Close();
            dbConnection.Close();

            return exists;
        }


        public DataTable GetUser(string Email)
        {
            DataTable userDetailsTable = new DataTable();
            DataTable userPreferencesTable = new DataTable();

            userDetailsTable.Columns.Add(new DataColumn("Email", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Username", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Password", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Reputation", typeof(int)));
            userDetailsTable.Columns.Add(new DataColumn("CarModel", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("FirstName", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Surname", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Birth", typeof(DateTime)));
            userDetailsTable.Columns.Add(new DataColumn("Gender", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("MemberSince", typeof(DateTime)));
            userDetailsTable.Columns.Add(new DataColumn("Mobile", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Picture", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Address", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Mode", typeof(string)));

            userPreferencesTable.Columns.Add(new DataColumn("Smoking", typeof(bool)));
            userPreferencesTable.Columns.Add(new DataColumn("Music", typeof(bool)));
            userPreferencesTable.Columns.Add(new DataColumn("Pets", typeof(bool)));
            userPreferencesTable.Columns.Add(new DataColumn("Talking", typeof(int)));

            userDetailsTable.Columns.Add(new DataColumn("Preferences", typeof(object)));


            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT UserDetails.Email, UserDetails.Username, UserDetails.Password, UserDetails.Reputation, UserDetails.CarModel, UserDetails.FirstName, UserDetails.Surname, UserDetails.Birth, UserDetails.Gender, UserDetails.MemberSince, UserDetails.Mobile, UserDetails.Picture, UserDetails.Address, UserDetails.Mode, Preferences.Smoking, Preferences.Music, Preferences.Pets, Preferences.Talking FROM UserDetails LEFT JOIN Preferences ON (UserDetails.IdPreferences = Preferences.IdPreferences) WHERE Email='" + Email + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    userPreferencesTable.Rows.Add(reader["Smoking"], reader["Music"], reader["Pets"], reader["Talking"]);
                    userDetailsTable.Rows.Add(reader["Email"], reader["Username"], reader["Password"], reader["Reputation"], reader["CarModel"], reader["FirstName"], reader["Surname"], reader["Birth"], reader["Gender"], reader["MemberSince"], reader["Mobile"], reader["Picture"], reader["Address"], reader["Mode"], userPreferencesTable);
                }
            }

            reader.Close();
            dbConnection.Close();
            return userDetailsTable;

        }


        public DataTable GetUserPublic(int IdUser)
        {
            DataTable userDetailsTable = new DataTable();
            DataTable userPreferencesTable = new DataTable();

            userDetailsTable.Columns.Add(new DataColumn("Email", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Username", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Reputation", typeof(int)));
            userDetailsTable.Columns.Add(new DataColumn("CarModel", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("FirstName", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Surname", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Birth", typeof(DateTime)));
            userDetailsTable.Columns.Add(new DataColumn("Gender", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("MemberSince", typeof(DateTime)));
            userDetailsTable.Columns.Add(new DataColumn("Mobile", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Picture", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Address", typeof(string)));
            userDetailsTable.Columns.Add(new DataColumn("Mode", typeof(string)));

            userPreferencesTable.Columns.Add(new DataColumn("Smoking", typeof(bool)));
            userPreferencesTable.Columns.Add(new DataColumn("Music", typeof(bool)));
            userPreferencesTable.Columns.Add(new DataColumn("Pets", typeof(bool)));
            userPreferencesTable.Columns.Add(new DataColumn("Talking", typeof(int)));

            userDetailsTable.Columns.Add(new DataColumn("Preferences", typeof(object)));


            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT UserDetails.Email, UserDetails.Username, UserDetails.Reputation, UserDetails.CarModel, UserDetails.FirstName, UserDetails.Surname, UserDetails.Birth, UserDetails.Gender, UserDetails.MemberSince, UserDetails.Mobile, UserDetails.Picture, UserDetails.Address, UserDetails.Mode, Preferences.Smoking, Preferences.Music, Preferences.Pets, Preferences.Talking FROM UserDetails LEFT JOIN Preferences ON (UserDetails.IdPreferences = Preferences.IdPreferences) WHERE IdUser=" + IdUser + ";";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    userPreferencesTable.Rows.Add(reader["Smoking"], reader["Music"], reader["Pets"], reader["Talking"]);
                    userDetailsTable.Rows.Add(reader["Email"], reader["Username"], reader["Reputation"], reader["CarModel"], reader["FirstName"], reader["Surname"], reader["Birth"], reader["Gender"], reader["MemberSince"], reader["Mobile"], reader["Picture"], reader["Address"], reader["Mode"], userPreferencesTable);
                }
            }

            reader.Close();
            dbConnection.Close();
            return userDetailsTable;

        }


        public bool UserAuthentication(string Email, string Password)
        {
            bool auth = false;

            CheckOffersExpiration();
            CheckRequestsExpiration();

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT IdUser FROM UserDetails WHERE Email='" + Email + "' AND Password='" + Password + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                auth = true;
            }

            reader.Close();
            dbConnection.Close();

            return auth;
        }


        public bool UpdateUser(string Email, string FirstName, string Surname, DateTime Birth, string Gender, string Mobile, string Picture, string Address, string Mode, string CarModel)
        {
            bool updated = false;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "UPDATE UserDetails SET ";
            query += !String.IsNullOrEmpty(FirstName) ? ("FirstName = '" + FirstName + "' , ") : "";
            query += !String.IsNullOrEmpty(Surname) ? ("Surname = '" + Surname + "' , ") : "";
            query += ((Birth != null) && (Birth != DateTime.MinValue)) ? ("Birth = '" + Birth.ToString("yyyy-MM-dd HH:mm") + "' , ") : "";
            query += !String.IsNullOrEmpty(Gender) ? ("Gender = '" + Gender + "' , ") : "";
            query += !String.IsNullOrEmpty(Mobile) ? ("Mobile = '" + Mobile + "' , ") : "";
            query += !String.IsNullOrEmpty(Picture) ? ("Picture = '" + Picture + "' , ") : "";
            query += !String.IsNullOrEmpty(Address) ? ("Address = '" + Address + "' , ") : "";
            query += !String.IsNullOrEmpty(Mode) ? ("Mode = '" + Mode + "' , ") : "";
            string lastColumn = !String.IsNullOrEmpty(CarModel) ? ("CarModel = '" + CarModel + "'") : "";

            if (!String.IsNullOrEmpty(lastColumn)) query += lastColumn;
            else query = query.Substring(0, query.Length - 2);

            query += " WHERE Email ='" + Email + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            int affected = command.ExecuteNonQuery();

            if (affected > 0)
                updated = true;

            dbConnection.Close();

            return updated;
        }


        public bool SetUserPreferences(string Email, bool Smoking, bool Music, bool Pets, int Talking)
        {
            bool updated = false;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT IdPreferences FROM UserDetails WHERE Email='" + Email + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();
            int idPreferences;

            while (reader.Read())
            {
                if (reader["IdPreferences"] != DBNull.Value)
                {
                    idPreferences = reader.GetInt32(0);
                    dbConnection.Close();
                    updated = UpdatePreferences(idPreferences, Smoking, Music, Pets, Talking);
                    break;
                }

                else
                {
                    dbConnection.Close();
                    idPreferences = CreatePreferences(Smoking, Music, Pets, Talking);

                    if (idPreferences > 0)
                    {
                        dbConnection.Close();
                        dbConnection.Open();
                        query = "UPDATE UserDetails SET IdPreferences = " + idPreferences + " WHERE Email ='" + Email + "';";

                        command = new SqlCommand(query, dbConnection);
                        int affected = command.ExecuteNonQuery();

                        if (affected > 0)
                            updated = true;
                    }
                    break;
                }
            }




            return updated;

        }


        public int CreatePreferences(bool Smoking, bool Music, bool Pets, int Talking)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }
            string query = "INSERT INTO Preferences(Smoking, Music, Pets, Talking) VALUES ('" + Smoking + "','" + Music + "','" + Pets + "'," + Talking + ") SELECT CAST(SCOPE_IDENTITY() AS INT);";
            SqlCommand command = new SqlCommand(query, dbConnection);
            int rowId = (int)command.ExecuteScalar();

            dbConnection.Close();

            return rowId;
        }


        public bool UpdatePreferences(int idPreferences, bool Smoking, bool Music, bool Pets, int Talking)
        {
            bool updated = false;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "UPDATE Preferences SET Smoking = '" + Smoking + "' , Music = '" + Music + "' , Pets = '" + Pets + "' , Talking = " + Talking + " WHERE IdPreferences ='" + idPreferences + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            int affected = command.ExecuteNonQuery();

            dbConnection.Close();

            if (affected > 0)
                updated = true;

            return updated;
        }


        public bool CreateOffer(string Email, double StartLat, double StartLng, double DestinationLat, double DestinationLng, List<List<double>> Waypoints, List<List<double>> RoutePoints, List<List<double>> RangePolygon, DateTime Departure, object Arrival, int Seats, double Price, int Radius, double Distance, bool Active)
        {
            bool created = false;
            int UserId;
            DateTime arr;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }
            
            string query = "SELECT IdUser FROM UserDetails WHERE Email = '" + Email + "';";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                UserId = reader.GetInt32(0);

                dbConnection.Close();
                dbConnection.Open();

                DateTime.TryParse(Arrival.ToString(), out arr);
                string arrTime = (String.IsNullOrEmpty(Arrival.ToString())) ? "" : arr.ToString("yyyy-MM-dd HH:mm");

                query = "INSERT INTO Offer(IdUser, StartLatitude, StartLongitude, DestinationLatitude, DestinationLongitude, Departure, Arrival, Seats, ReservedSeats, Price, Radius, Distance, Active) VALUES (" + UserId + "," + StartLat + "," + StartLng + "," + DestinationLat + "," + DestinationLng + ",'" + Departure.ToString("yyyy-MM-dd HH:mm") + "','" + arrTime + "'," + Seats + ", 0," + Price + "," + Radius + "," + Distance + ",'" + Active + "') SELECT CAST(SCOPE_IDENTITY() AS INT);";
                command = new SqlCommand(query, dbConnection);
                int offerId = (int)command.ExecuteScalar();

                bool waypointsListCreated = (Waypoints.Count == 0) ? true : CreateWaypointsList(offerId, Waypoints);
                bool routePointsListCreated = CreateRoutePointsList(offerId, RoutePoints);
                bool polygonCreated = CreatePolygon(offerId, RangePolygon);

                if ((offerId > 0) && polygonCreated && waypointsListCreated && routePointsListCreated)
                    created = true;


                //else delete what was created..
                else
                {
                    DeleteOffer(offerId);
                }
            }


            dbConnection.Close();

            return created;
        }


        public DataTable GetOffersList(string Email)
        {

            DataTable offersListTable = new DataTable();
            offersListTable.Columns.Add(new DataColumn("Offers", typeof(object)));


            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }


            string query = "SELECT * FROM Offer WHERE IdUser= (SELECT IdUser FROM UserDetails WHERE Email = '" + Email + "') ORDER BY Departure ASC;";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DataTable offerDetailsTable = new DataTable();
                    offerDetailsTable.Columns.Add(new DataColumn("IdOffer", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("IdUser", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("StartLatitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("StartLongitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("DestinationLatitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("DestinationLongitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("Departure", typeof(DateTime)));
                    offerDetailsTable.Columns.Add(new DataColumn("Arrival", typeof(DateTime)));
                    offerDetailsTable.Columns.Add(new DataColumn("Seats", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("ReservedSeats", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("Price", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("Radius", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("Distance", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("Active", typeof(bool)));

                    offerDetailsTable.Columns.Add(new DataColumn("Waypoints", typeof(object)));
                    offerDetailsTable.Columns.Add(new DataColumn("RoutePoints", typeof(object)));
                    offerDetailsTable.Columns.Add(new DataColumn("RangeIndices", typeof(object)));

                    int OfferId = Convert.ToInt32(reader["IdOffer"]);
                    offerDetailsTable.Rows.Add(reader["IdOffer"], reader["IdUser"], reader["StartLatitude"], reader["StartLongitude"], reader["DestinationLatitude"], reader["DestinationLongitude"], reader["Departure"], reader["Arrival"], reader["Seats"], reader["ReservedSeats"], reader["Price"], reader["Radius"], reader["Distance"], reader["Active"], GetWaypointsList(OfferId), GetRoutePointsList(OfferId), GetRangeIndices(OfferId));
                    offersListTable.Rows.Add(offerDetailsTable);
                }
            }

            reader.Close();
            dbConnection.Close();
            return offersListTable;

        }


        public DataTable GetRequestedOffersList(List<int> OfferIds)
        {

            DataTable offersListTable = new DataTable();
            offersListTable.Columns.Add(new DataColumn("Offers", typeof(object)));


            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string ids = "";
            foreach (var i in OfferIds)
            {
                if (i.Equals(OfferIds.Last())) ids += i.ToString();
                else ids += i + ", ";
            }
            string query = "SELECT * FROM Offer WHERE IdOffer IN (" + ids + ");";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DataTable offerDetailsTable = new DataTable();
                    offerDetailsTable.Columns.Add(new DataColumn("IdOffer", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("IdUser", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("StartLatitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("StartLongitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("DestinationLatitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("DestinationLongitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("Departure", typeof(DateTime)));
                    offerDetailsTable.Columns.Add(new DataColumn("Arrival", typeof(DateTime)));
                    offerDetailsTable.Columns.Add(new DataColumn("Seats", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("ReservedSeats", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("Price", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("Radius", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("Distance", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("Active", typeof(bool)));

                    offerDetailsTable.Columns.Add(new DataColumn("Waypoints", typeof(object)));
                    offerDetailsTable.Columns.Add(new DataColumn("RoutePoints", typeof(object)));
                    offerDetailsTable.Columns.Add(new DataColumn("RangeIndices", typeof(object)));

                    int OfferId = Convert.ToInt32(reader["IdOffer"]);
                    offerDetailsTable.Rows.Add(reader["IdOffer"], reader["IdUser"], reader["StartLatitude"], reader["StartLongitude"], reader["DestinationLatitude"], reader["DestinationLongitude"], reader["Departure"], reader["Arrival"], reader["Seats"], reader["ReservedSeats"], reader["Price"], reader["Radius"], reader["Distance"], reader["Active"], GetWaypointsList(OfferId), GetRoutePointsList(OfferId), GetRangeIndices(OfferId));
                    offersListTable.Rows.Add(offerDetailsTable);
                }
            }

            reader.Close();
            dbConnection.Close();
            return offersListTable;

        }


        public bool UpdateOffer(int IdOffer, object StartLat, object StartLng, object DestinationLat, object DestinationLng, List<List<double>> Waypoints, List<List<double>> RoutePoints, List<List<double>> RangePolygon, object Departure, object Arrival, object Seats, object ReservedSeats, object Price, object Radius, object Distance, object Active)
        {
            bool updated = false;
            DateTime dep, arr;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "UPDATE Offer SET ";
            query += (!String.IsNullOrEmpty(StartLat.ToString())) ? ("StartLatitude = " + (double)StartLat + " , ") : "";
            query += (!String.IsNullOrEmpty(StartLng.ToString())) ? ("StartLongitude = " + (double)StartLng + " , ") : "";
            query += (!String.IsNullOrEmpty(DestinationLat.ToString())) ? ("DestinationLatitude = " + (double)DestinationLat + " , ") : "";
            query += (!String.IsNullOrEmpty(DestinationLng.ToString())) ? ("DestinationLongitude = " + (double)DestinationLng + " , ") : "";

            DateTime.TryParse(Departure.ToString(), out dep);
            DateTime.TryParse(Arrival.ToString(), out arr);

            query += (!String.IsNullOrEmpty(Departure.ToString())) ? ("Departure = '" + dep.ToString("yyyy-MM-dd HH:mm") + "' , ") : "";
            query += (!String.IsNullOrEmpty(Arrival.ToString())) ? ("Arrival = '" + arr.ToString("yyyy-MM-dd HH:mm") + "' , ") : "";
            query += (!String.IsNullOrEmpty(Seats.ToString())) ? ("Seats = " + Int32.Parse(Seats.ToString()) + " , ") : "";
            query += (!String.IsNullOrEmpty(ReservedSeats.ToString())) ? ("ReservedSeats = " + Int32.Parse(ReservedSeats.ToString()) + " , ") : "";
            query += (!String.IsNullOrEmpty(Price.ToString())) ? ("Price = " + Double.Parse(Price.ToString()) + " , ") : "";
            query += (!String.IsNullOrEmpty(Radius.ToString())) ? ("Radius = " + Int32.Parse(Radius.ToString()) + " , ") : "";
            query += (!String.IsNullOrEmpty(Distance.ToString())) ? ("Distance = " + Double.Parse(Distance.ToString()) + " , ") : "";
            string lastColumn = (!String.IsNullOrEmpty(Active.ToString())) ? ("Active = '" + Boolean.Parse(Active.ToString()) + "'") : "";

            if (!String.IsNullOrEmpty(lastColumn)) query += lastColumn;
            else query = query.Substring(0, query.Length - 2);

            query += " WHERE IdOffer =" + IdOffer + ";";

            SqlCommand command = new SqlCommand(query, dbConnection);
            int affected = command.ExecuteNonQuery();

            if (Waypoints.Count > 0)
            {
                DeleteWaypointsList(IdOffer);
                CreateWaypointsList(IdOffer, Waypoints);
            }

            if (RoutePoints.Count > 0)
            {
                DeleteRoutePointsList(IdOffer);
                CreateRoutePointsList(IdOffer, RoutePoints);
            }

            if (RangePolygon.Count > 0)
            {
                DeletePolygon(IdOffer);
                CreatePolygon(IdOffer, RangePolygon);
            }


            if (affected > 0)
                updated = true;

            dbConnection.Close();

            return updated;
        }


        public bool DeleteOffer(int IdOffer)
        {

            DeleteWaypointsList(IdOffer);
            DeleteRoutePointsList(IdOffer);
            DeletePolygon(IdOffer);

            if (dbConnection.State.ToString() == "Open")
            {
                dbConnection.Close();
            }

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "DELETE FROM Offer WHERE IdOffer = " + IdOffer + ";";
            SqlCommand command = new SqlCommand(query, dbConnection);
            int affected = command.ExecuteNonQuery();

            dbConnection.Close();
            dbConnection.Open();

            query = "UPDATE Request SET Rejected = 'true' WHERE IdOffer = " + IdOffer + ";";
            command = new SqlCommand(query, dbConnection);
            command.ExecuteNonQuery();

            dbConnection.Close();

            return (affected > 0);

        }


        public bool CreateWaypointsList(int IdOffer, List<List<double>> Waypoints)
        {
            bool created = false;

            foreach (var index in Waypoints)
            {

                if (dbConnection.State.ToString() == "Open")
                {
                    dbConnection.Close();
                }

                if (dbConnection.State.ToString() == "Closed")
                {
                    dbConnection.Open();
                }

                string query = "INSERT INTO Waypoint(IdOffer, Latitude, Longitude) VALUES (" + IdOffer + "," + index[0] + "," + index[1] + ");";
                SqlCommand command = new SqlCommand(query, dbConnection);
                int affected = command.ExecuteNonQuery();

                if (affected > 0)
                    created = true;
            }




            dbConnection.Close();

            return created;
        }


        public bool CreateRoutePointsList(int IdOffer, List<List<double>> RoutePoints)
        {
            bool created = false;


            if (dbConnection.State.ToString() == "Open")
            {
                dbConnection.Close();
            }

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            DataTable routePointsTable = new DataTable();
            routePointsTable.Columns.Add(new DataColumn("IdOffer", typeof(int)));
            routePointsTable.Columns.Add(new DataColumn("Latitude", typeof(double)));
            routePointsTable.Columns.Add(new DataColumn("Longitude", typeof(double)));

            foreach (var p in RoutePoints)
            {
                routePointsTable.Rows.Add(IdOffer, p[0], p[1]);
            }


            //using (SqlConnection destinationConnection = dbConnection)
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(dbConnection))
            {
                bulkCopy.DestinationTableName = "RoutePoint";
                bulkCopy.ColumnMappings.Add("IdOffer", "IdOffer");
                bulkCopy.ColumnMappings.Add("Latitude", "Latitude");
                bulkCopy.ColumnMappings.Add("Longitude", "Longitude");
                try
                {
                    bulkCopy.WriteToServer(routePointsTable);
                    created = true;
                }
                catch (Exception e) { }
            }

            dbConnection.Close();

            return created;
        }


        public bool CreatePolygon(int IdOffer, List<List<double>> RangePolygon)
        {
            bool created = false;


            if (dbConnection.State.ToString() == "Open")
            {
                dbConnection.Close();
            }

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            DataTable rangeIndicesTable = new DataTable();
            rangeIndicesTable.Columns.Add(new DataColumn("IdOffer", typeof(int)));
            rangeIndicesTable.Columns.Add(new DataColumn("Latitude", typeof(double)));
            rangeIndicesTable.Columns.Add(new DataColumn("Longitude", typeof(double)));

            foreach (var p in RangePolygon)
            {
                rangeIndicesTable.Rows.Add(IdOffer, p[0], p[1]);
            }


            //using (SqlConnection destinationConnection = dbConnection)
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(dbConnection))
            {
                bulkCopy.DestinationTableName = "PolygonIndex";
                bulkCopy.ColumnMappings.Add("IdOffer", "IdOffer");
                bulkCopy.ColumnMappings.Add("Latitude", "Latitude");
                bulkCopy.ColumnMappings.Add("Longitude", "Longitude");
                try
                {
                    bulkCopy.WriteToServer(rangeIndicesTable);
                    created = true;
                }
                catch (Exception e) { }
            }



            dbConnection.Close();

            return created;
        }


        public DataTable GetWaypointsList(int IdOffer)
        {
            DataTable waypointsTable = new DataTable();
            waypointsTable.Columns.Add(new DataColumn("Latitude", typeof(double)));
            waypointsTable.Columns.Add(new DataColumn("Longitude", typeof(double)));

            SqlConnection dbConnection = DBConnect.getConnection();

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT Latitude, Longitude FROM Waypoint WHERE IdOffer= " + IdOffer + ";";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    waypointsTable.Rows.Add(reader["Latitude"], reader["Longitude"]);
                }
            }

            reader.Close();
            dbConnection.Close();
            return waypointsTable;
        }


        public DataTable GetRoutePointsList(int IdOffer)
        {
            DataTable routePointsTable = new DataTable();
            routePointsTable.Columns.Add(new DataColumn("Latitude", typeof(double)));
            routePointsTable.Columns.Add(new DataColumn("Longitude", typeof(double)));

            SqlConnection dbConnection = DBConnect.getConnection();

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT Latitude, Longitude FROM RoutePoint WHERE IdOffer= " + IdOffer + ";";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    routePointsTable.Rows.Add(reader["Latitude"], reader["Longitude"]);
                }
            }

            reader.Close();
            dbConnection.Close();
            return routePointsTable;
        }


        public DataTable GetRangeIndices(int IdOffer)
        {
            DataTable rangeIndicesTable = new DataTable();
            rangeIndicesTable.Columns.Add(new DataColumn("Latitude", typeof(double)));
            rangeIndicesTable.Columns.Add(new DataColumn("Longitude", typeof(double)));

            SqlConnection dbConnection = DBConnect.getConnection();

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT Latitude, Longitude FROM PolygonIndex WHERE IdOffer= " + IdOffer + ";";
            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    rangeIndicesTable.Rows.Add(reader["Latitude"], reader["Longitude"]);
                }
            }

            reader.Close();
            dbConnection.Close();
            return rangeIndicesTable;
        }


        public bool DeleteWaypointsList(int IdOffer)
        {
            bool deleted = false;



            if (dbConnection.State.ToString() == "Open")
            {
                dbConnection.Close();
            }

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "DELETE FROM Waypoint WHERE IdOffer = " + IdOffer + ";";
            SqlCommand command = new SqlCommand(query, dbConnection);
            int affected = command.ExecuteNonQuery();

            if (affected > 0)
                deleted = true;

            dbConnection.Close();

            return deleted;
        }


        public bool DeleteRoutePointsList(int IdOffer)
        {
            bool deleted = false;



            if (dbConnection.State.ToString() == "Open")
            {
                dbConnection.Close();
            }

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "DELETE FROM RoutePoint WHERE IdOffer = " + IdOffer + ";";
            SqlCommand command = new SqlCommand(query, dbConnection);
            int affected = command.ExecuteNonQuery();

            if (affected > 0)
                deleted = true;

            dbConnection.Close();

            return deleted;
        }


        public bool DeletePolygon(int IdOffer)
        {
            bool deleted = false;



            if (dbConnection.State.ToString() == "Open")
            {
                dbConnection.Close();
            }

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "DELETE FROM PolygonIndex WHERE IdOffer = " + IdOffer + ";";
            SqlCommand command = new SqlCommand(query, dbConnection);
            int affected = command.ExecuteNonQuery();

            if (affected > 0)
                deleted = true;

            dbConnection.Close();

            return deleted;
        }


        public DataTable SearchRides(DateTime ArrivalFrom, object ArrivalTo, double StartLat, double StartLng, double DestinationLat, double DestinationLng)
        {

            DateTime arr;

            DataTable offersListTable = new DataTable();
            offersListTable.Columns.Add(new DataColumn("Offers", typeof(object)));

            DateTime.TryParse(ArrivalTo.ToString(), out arr);

            //string arrivalTo = (String.IsNullOrEmpty(ArrivalTo.ToString())) ? "" : "OR (Departure < '" + arr.ToString("yyyy-MM-dd HH:mm") + "' AND Arrival >= '" + arr.ToString("yyyy-MM-dd HH:mm") + "')";
            string arrivalTo = (String.IsNullOrEmpty(ArrivalTo.ToString())) ? "" : "OR  ( NOT ( '" + ArrivalFrom.ToString("yyyy-MM-dd HH:mm") + "' < Departure AND '" + arr.ToString("yyyy-MM-dd HH:mm") + "' < Departure ) AND NOT ( '" + ArrivalFrom.ToString("yyyy-MM-dd HH:mm") + "' > Arrival AND '" + arr.ToString("yyyy-MM-dd HH:mm") + "' > Arrival ) )";
            string query = "SELECT * FROM Offer WHERE Active = 'true' AND ReservedSeats = 0 AND ( (Departure < '" + ArrivalFrom.ToString("yyyy-MM-dd HH:mm") + "' AND Arrival >= '" + ArrivalFrom.ToString("yyyy-MM-dd HH:mm") + "') " + arrivalTo + " );";

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DataTable offerDetailsTable = new DataTable();
                    offerDetailsTable.Columns.Add(new DataColumn("IdOffer", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("IdUser", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("StartLatitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("StartLongitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("DestinationLatitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("DestinationLongitude", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("Departure", typeof(DateTime)));
                    offerDetailsTable.Columns.Add(new DataColumn("Arrival", typeof(DateTime)));
                    offerDetailsTable.Columns.Add(new DataColumn("Seats", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("ReservedSeats", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("Price", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("Radius", typeof(int)));
                    offerDetailsTable.Columns.Add(new DataColumn("Distance", typeof(double)));
                    offerDetailsTable.Columns.Add(new DataColumn("Active", typeof(bool)));

                    offerDetailsTable.Columns.Add(new DataColumn("Waypoints", typeof(object)));
                    offerDetailsTable.Columns.Add(new DataColumn("RoutePoints", typeof(object)));
                    offerDetailsTable.Columns.Add(new DataColumn("RangeIndices", typeof(object)));

                    int OfferId = Convert.ToInt32(reader["IdOffer"]);
                    offerDetailsTable.Rows.Add(reader["IdOffer"], reader["IdUser"], reader["StartLatitude"], reader["StartLongitude"], reader["DestinationLatitude"], reader["DestinationLongitude"], reader["Departure"], reader["Arrival"], reader["Seats"], reader["ReservedSeats"], reader["Price"], reader["Radius"], reader["Distance"], reader["Active"], GetWaypointsList(OfferId), GetRoutePointsList(OfferId), GetRangeIndices(OfferId));
                    offersListTable.Rows.Add(offerDetailsTable);
                }
            }

            reader.Close();
            dbConnection.Close();

            return offersListTable;
        }


        public bool CreateRequest(string Email, object IdOffer, double StartLat, double StartLng, double DestinationLat, double DestinationLng, DateTime ArrivalFrom, object ArrivalTo, int Seats)
        {
            bool created = false;
            DateTime arr;


            if (!String.IsNullOrEmpty(IdOffer.ToString()))
            {
                int o = Int32.Parse(IdOffer.ToString());
                if (!CheckRequestExistence(Email, o))
                {
                    string offerId = !String.IsNullOrEmpty(IdOffer.ToString()) ? (Int32.Parse(IdOffer.ToString())) + "," : "";
                    string offerIdCol = !String.IsNullOrEmpty(IdOffer.ToString()) ? "IdOffer," : "";

                    DateTime.TryParse(ArrivalTo.ToString(), out arr);
                    string arrTo = (String.IsNullOrEmpty(ArrivalTo.ToString())) ? "" : arr.ToString("yyyy-MM-dd HH:mm");

                    if (dbConnection.State.ToString() == "Open")
                    {
                        dbConnection.Close();
                    }

                    if (dbConnection.State.ToString() == "Closed")
                    {
                        dbConnection.Open();
                    }

                    string query = "INSERT INTO Request(IdUser, " + offerIdCol + " StartLatitude, StartLongitude, DestinationLatitude, DestinationLongitude, ArrivalFrom, ArrivalTo, Seats, PickUp, Approved, Rejected) VALUES ( (SELECT IdUser FROM UserDetails WHERE Email = '" + Email + "')," + offerId + StartLat + "," + StartLng + "," + DestinationLat + "," + DestinationLng + ",'" + ArrivalFrom.ToString("yyyy-MM-dd HH:mm") + "','" + arrTo + "'," + Seats + ", '', 'false', 'false') ;";
                    SqlCommand command = new SqlCommand(query, dbConnection);
                    int affected = command.ExecuteNonQuery();
                    dbConnection.Close();

                    
                    if (affected > 0)
                    {
                        created = true;

                        query = "SELECT IdRequest FROM Request WHERE IdRequest = (SELECT MAX(IdRequest) FROM Request);";
                        dbConnection.Open();
                        command = new SqlCommand(query, dbConnection);
                        SqlDataReader reader = command.ExecuteReader();
                        int requestId = 0;

                        while (reader.Read())
                        {
                            requestId = Convert.ToInt32(reader["IdRequest"]);
                        }

                        reader.Close();
                        dbConnection.Close();

                        //To notify the web client
                        RecordTableChange(IdOffer.ToString() + "-" + requestId, "Request", "", "", "");

                        //To notify the mobile client
                        //Sender details
                        DataTable senderTable = GetUser(Email);
                        string senderName = senderTable.Rows[0]["FirstName"].ToString();
                        //Recipient details
                        if (dbConnection.State.ToString() == "Open") { dbConnection.Close(); }
                        if (dbConnection.State.ToString() == "Closed") { dbConnection.Open(); }
                        
                        query = "SELECT Email FROM UserDetails WHERE IdUser = (SELECT IdUser FROM Offer WHERE IdOffer = " + IdOffer + ");";
                        command = new SqlCommand(query, dbConnection);
                        reader = command.ExecuteReader();
                        string recepientEmail = reader.Read() ? reader.GetString(0) : "";
                        recepientEmail = recepientEmail.Replace('@', '%');
                        reader.Close();
                        dbConnection.Close();

                        sendMessageToFirebase("/topics/"+ recepientEmail, "Ride request", senderName, "request", Email, IdOffer.ToString(), ArrivalFrom.DayOfWeek.ToString() + " " + ArrivalFrom.Day + "/" + ArrivalFrom.Month + "/" + ArrivalFrom.Year);
                    }

                    //else delete what was created..

                    
                }
            }



            return created;
        }


        public DataTable GetDriverReceivedRequests(string Email, bool Approved, bool Rejected)
        {
            DataTable requestsListTable = new DataTable();
            requestsListTable.Columns.Add(new DataColumn("Requests", typeof(object)));


            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }


            string query = "SELECT * FROM Request WHERE Approved = '" + Approved + "' AND Rejected = '" + Rejected + "' AND IdOffer IN (SELECT IdOffer FROM Offer WHERE IdUser = (SELECT IdUser FROM UserDetails WHERE Email = '" + Email + "'));";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DataTable requestDetailsTable = new DataTable();
                    requestDetailsTable.Columns.Add(new DataColumn("IdRequest", typeof(int)));
                    requestDetailsTable.Columns.Add(new DataColumn("IdUser", typeof(int)));
                    requestDetailsTable.Columns.Add(new DataColumn("IdOffer", typeof(int)));
                    requestDetailsTable.Columns.Add(new DataColumn("StartLatitude", typeof(double)));
                    requestDetailsTable.Columns.Add(new DataColumn("StartLongitude", typeof(double)));
                    requestDetailsTable.Columns.Add(new DataColumn("DestinationLatitude", typeof(double)));
                    requestDetailsTable.Columns.Add(new DataColumn("DestinationLongitude", typeof(double)));
                    requestDetailsTable.Columns.Add(new DataColumn("ArrivalFrom", typeof(DateTime)));
                    requestDetailsTable.Columns.Add(new DataColumn("ArrivalTo", typeof(object)));
                    requestDetailsTable.Columns.Add(new DataColumn("Seats", typeof(int)));
                    requestDetailsTable.Columns.Add(new DataColumn("PickUp", typeof(object)));
                    requestDetailsTable.Columns.Add(new DataColumn("Approved", typeof(bool)));
                    requestDetailsTable.Columns.Add(new DataColumn("Rejected", typeof(bool)));

                    requestDetailsTable.Rows.Add(reader["IdRequest"], reader["IdUser"], reader["IdOffer"], reader["StartLatitude"], reader["StartLongitude"], reader["DestinationLatitude"], reader["DestinationLongitude"], reader["ArrivalFrom"], reader["ArrivalTo"], reader["Seats"], reader["PickUp"], reader["Approved"], reader["Rejected"]);
                    requestsListTable.Rows.Add(requestDetailsTable);
                }
            }

            reader.Close();
            dbConnection.Close();
            return requestsListTable;

        }


        public DataTable GetPassengerSentRequests(string Email)
        {
            DataTable requestsListTable = new DataTable();
            requestsListTable.Columns.Add(new DataColumn("Requests", typeof(object)));


            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }


            string query = "SELECT * FROM Request WHERE IdUser = (SELECT IdUser FROM UserDetails WHERE Email = '" + Email + "') AND IdOffer IS NOT NULL;";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DataTable requestDetailsTable = new DataTable();
                    requestDetailsTable.Columns.Add(new DataColumn("IdRequest", typeof(int)));
                    requestDetailsTable.Columns.Add(new DataColumn("IdUser", typeof(int)));
                    requestDetailsTable.Columns.Add(new DataColumn("IdOffer", typeof(int)));
                    requestDetailsTable.Columns.Add(new DataColumn("StartLatitude", typeof(double)));
                    requestDetailsTable.Columns.Add(new DataColumn("StartLongitude", typeof(double)));
                    requestDetailsTable.Columns.Add(new DataColumn("DestinationLatitude", typeof(double)));
                    requestDetailsTable.Columns.Add(new DataColumn("DestinationLongitude", typeof(double)));
                    requestDetailsTable.Columns.Add(new DataColumn("ArrivalFrom", typeof(DateTime)));
                    requestDetailsTable.Columns.Add(new DataColumn("ArrivalTo", typeof(object)));
                    requestDetailsTable.Columns.Add(new DataColumn("Seats", typeof(int)));
                    requestDetailsTable.Columns.Add(new DataColumn("PickUp", typeof(object)));
                    requestDetailsTable.Columns.Add(new DataColumn("Approved", typeof(bool)));
                    requestDetailsTable.Columns.Add(new DataColumn("Rejected", typeof(bool)));

                    requestDetailsTable.Rows.Add(reader["IdRequest"], reader["IdUser"], reader["IdOffer"], reader["StartLatitude"], reader["StartLongitude"], reader["DestinationLatitude"], reader["DestinationLongitude"], reader["ArrivalFrom"], reader["ArrivalTo"], reader["Seats"], reader["PickUp"], reader["Approved"], reader["Rejected"]);
                    requestsListTable.Rows.Add(requestDetailsTable);
                }
            }

            reader.Close();
            dbConnection.Close();
            return requestsListTable;

        }


        public bool RespondToRequest(int IdRequest, object PickUp, bool Approved)
        {
            bool updated = false;
            bool updatedSeats = false;

            DateTime pick;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "UPDATE Request SET ";

            DateTime.TryParse(PickUp.ToString(), out pick);

            query += (!String.IsNullOrEmpty(PickUp.ToString())) ? ("PickUp = '" + pick.ToString("yyyy-MM-dd HH:mm") + "' , ") : "";
            query += "Approved = '" + Boolean.Parse(Approved.ToString()) + "' ,";
            query += !Approved ? "Rejected = 'true'" : "Rejected = 'false'";
            query += " WHERE IdRequest =" + IdRequest + ";";


            SqlCommand command = new SqlCommand(query, dbConnection);
            int affected = command.ExecuteNonQuery();
            dbConnection.Close();

            if (Approved && affected > 0)
            {
                updatedSeats = UpdateReservedSeats(IdRequest);
                updated = (affected > 0 && updatedSeats);
            }

            

            //If the request was approved and the seats were not updated then undo the update changes
            if (Approved && !updatedSeats)
            {
                if (dbConnection.State.ToString() == "Closed")
                {
                    dbConnection.Open();
                }

                pick = new DateTime(1900, 1, 1, 0, 0, 0, 0);
                query = "UPDATE Request SET PickUp = '" + pick.ToString("yyyy-MM-dd HH:mm") + "', Approved = 'false', Rejected = 'false' WHERE IdRequest = " + IdRequest + ";";
                command = new SqlCommand(query, dbConnection);
                command.ExecuteNonQuery();
                dbConnection.Close();
                updated = false;
            }

            if(!Approved && affected>0)
            {
                updated = true;
            }

            //If the driver responded to the request
            if (updated)
            {
                string column = Approved ? "Approved" : "Rejected";
                RecordTableChange(IdRequest.ToString()+ "p", "Request", column, "false", "true");

                //Send notification to mobile client
                if (dbConnection.State.ToString() == "Open") { dbConnection.Close(); }
                if (dbConnection.State.ToString() == "Closed") { dbConnection.Open(); }

                string pickupString = DateTime.TryParse(PickUp.ToString(), out pick) ? pick.DayOfWeek.ToString() + " " + pick.Day + "/" + pick.Month + "/" + pick.Year : "";

                DataTable emailsTable = new DataTable();
                emailsTable.Columns.Add(new DataColumn("Recipient", typeof(string)));
                emailsTable.Columns.Add(new DataColumn("Sender", typeof(string)));
                emailsTable.Columns.Add(new DataColumn("SenderName", typeof(string)));

                query = "SELECT p.Email AS Recipient, d.Email AS Sender, d.FirstName AS SenderName FROM UserDetails p, Request JOIN UserDetails d ON (d.IdUser = (SELECT IdUser FROM Offer WHERE IdOffer = Request.IdOffer)) WHERE IdRequest =  " + IdRequest + "  AND p.IdUser = (SELECT IdUser FROM Request WHERE IdRequest =  " + IdRequest+" );";
                command = new SqlCommand(query, dbConnection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        emailsTable.Rows.Add(reader["Recipient"], reader["Sender"], reader["SenderName"]);

                string recepientEmail = emailsTable.Rows[0]["Recipient"].ToString();
                string senderEmail = emailsTable.Rows[0]["Sender"].ToString();
                string senderName = emailsTable.Rows[0]["SenderName"].ToString();
                recepientEmail = recepientEmail.Replace('@', '%');
                reader.Close();
                dbConnection.Close();

                sendMessageToFirebase("/topics/" + recepientEmail, "Feedback", senderName, Approved ? "approved" : "rejected", senderEmail, IdRequest.ToString(), pickupString);
            }


            return updated;
        }


        public bool UpdateReservedSeats(int IdRequest)
        {
            int affected = 0;
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }
            //
            try
            {
                string query = "UPDATE Offer SET ReservedSeats = (SELECT Seats FROM Request WHERE IdRequest = " + IdRequest + ") WHERE IdOffer = (SELECT IdOffer FROM Request WHERE IdRequest = " + IdRequest + ");";
                SqlCommand command = new SqlCommand(query, dbConnection);
                affected = command.ExecuteNonQuery();
                dbConnection.Close();
            }
            catch (Exception e) { }


            return (affected > 0);
        }


        public void CheckOffersExpiration()
        {
            string query = "UPDATE Offer SET Active = 'false' WHERE Arrival < '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' AND Active = 'true';";

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            SqlCommand command = new SqlCommand(query, dbConnection);
            command.ExecuteNonQuery();
            dbConnection.Close();
        }


        public void CheckRequestsExpiration()
        {

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "DELETE FROM Request WHERE ArrivalFrom < '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "';";
            SqlCommand command = new SqlCommand(query, dbConnection);
            command.ExecuteNonQuery();
            dbConnection.Close();
        }


        public bool CheckRequestExistence(string Email, int IdOffer)
        {
            bool exists = false;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT * FROM Request WHERE IdUser= (SELECT IdUser FROM UserDetails WHERE Email = '" + Email + "' ) AND IdOffer = " + IdOffer + ";";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                exists = true;
            }

            reader.Close();
            dbConnection.Close();

            return exists;
        }


        public bool CheckRequestPending(string Email, int IdOffer)
        {
            bool pending = false;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT * FROM Request WHERE IdUser= (SELECT IdUser FROM UserDetails WHERE Email = '" + Email + "' ) AND IdOffer = " + IdOffer + " AND Approved = 'false' AND Rejected = 'false';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                pending = true;
            }

            reader.Close();
            dbConnection.Close();

            return pending;
        }


        public DataTable GetPassengerNotifications(List<object> SentRequestsIds)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);

            string ids = "";

            foreach (var id in SentRequestsIds)
            {
                if (id.Equals(SentRequestsIds.Last())) ids += "'" + id.ToString() + "p'";
                else ids += "'" + id.ToString() + "p', ";
            }


            List<List<object>> updatedRequests = new List<List<object>>();

            string query = "SELECT Identifier, ColumnName FROM TableChange WHERE Identifier IN (" + ids + ") AND TableName = 'Request';";

            //if (dbConnection.State.ToString() == "Open")
            //{
            //    dbConnection.Close();
            //}

            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }

            
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    List<object> request = new List<object>() { Convert.ToUInt32(Convert.ToString(reader["Identifier"]).Replace("p", "")), Convert.ToString(reader["ColumnName"]) };
                    updatedRequests.Add(request);
                }
            }

            if (con.State.ToString() == "Open")
            {
                reader.Close();
                con.Close();
            }

            //string updatedIds = "";

            foreach (var ur in updatedRequests)
            {
                //if (ur.Equals(updatedRequests.Last())) updatedIds += "'" + ur[0].ToString() + "'";
                //else updatedIds += "'" + ur[0].ToString() + "', ";

                DeleteTableChange(ur[0].ToString()+"p", "Request", ur[1].ToString());
            }

            ids = "";

            foreach (var id in SentRequestsIds)
            {
                if (id.Equals(SentRequestsIds.Last())) ids += id.ToString();
                else ids += id.ToString() + ", ";
            }

            DataTable notifications = new DataTable();
            notifications.Columns.Add(new DataColumn("Requests", typeof(object)));

            //string query = "SELECT IdRequest, Approved, Rejected FROM Request WHERE IdRequest IN (" + ids + ");";
            query = "SELECT Request.IdRequest, Request.IdUser, Request.Approved, Request.Rejected, Request.PickUp, Request.ArrivalFrom, UserDetails.FirstName, UserDetails.Picture FROM Request JOIN UserDetails ON (UserDetails.IdUser = (SELECT IdUser FROM Offer WHERE IdOffer = Request.IdOffer)) WHERE IdRequest IN (" + ids + ");";

            //if (dbConnection.State.ToString() == "Open")
            //{
            //    dbConnection.Close();
            //}

            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }

            command = new SqlCommand(query, con);
            reader = command.ExecuteReader();

            List<Notification> notifs = new List<Notification>();

            if (reader.HasRows)
            {
                
                while (reader.Read())
                {
                    DataTable requestStatuses = new DataTable();
                    requestStatuses.Columns.Add(new DataColumn("IdRequest", typeof(int)));
                    requestStatuses.Columns.Add(new DataColumn("Status", typeof(string)));
                    requestStatuses.Columns.Add(new DataColumn("DriverName", typeof(string)));
                    requestStatuses.Columns.Add(new DataColumn("DriverPicture", typeof(string)));
                    requestStatuses.Columns.Add(new DataColumn("Time", typeof(string)));

                    
                    foreach (var r in updatedRequests)
                    {

                        int id = int.Parse(r[0].ToString());
                        if (id == Convert.ToInt32(reader["IdRequest"]))
                        {
                            DateTime time = Convert.ToDateTime(reader["ArrivalFrom"]);
                            //If request status has changed from pending to approved
                            //if ((Convert.ToBoolean(reader["Approved"]) != Boolean.Parse(r[1].ToString())) && (Convert.ToBoolean(reader["Approved"])))
                            if (r[1].ToString().Equals("Approved"))
                            {
                                requestStatuses.Rows.Add(reader["IdRequest"], "approved", reader["FirstName"], reader["Picture"], time.ToString("dddd").Substring(0, 3) + ", " + time.ToString("dd-MM-yyyy"));
                            }

                            //If request status has changed from pending to rejected
                            //if ((Convert.ToBoolean(reader["Rejected"]) != Boolean.Parse(r[2].ToString())) && (Convert.ToBoolean(reader["Rejected"])))
                            if (r[1].ToString().Equals("Rejected"))
                            {
                                requestStatuses.Rows.Add(reader["IdRequest"], "rejected", reader["FirstName"], reader["Picture"], time.ToString("dddd").Substring(0, 3) + ", " + time.ToString("dd-MM-yyyy"));
                            }

                            notifs.Add(new Notification(Convert.ToInt32(reader["IdUser"]), "RequestId:" + Convert.ToString(reader["IdRequest"]) + ";UserName:" + Convert.ToString(reader["FirstName"]) + ";Status:" + r[1].ToString() + ";Time:" + time.ToString("dddd").Substring(0, 3) + ", " + time.ToString("dd-MM-yyyy"), Convert.ToString(reader["Picture"]), DateTime.Now));
                        }
                    }


                    if (!Convert.ToBoolean(reader["Rejected"]) && !Convert.ToBoolean(reader["Approved"]))
                    {
                        DateTime time = Convert.ToDateTime(reader["ArrivalFrom"]);
                        requestStatuses.Rows.Add(reader["IdRequest"], "pending", reader["FirstName"], reader["Picture"], time.ToString("dddd").Substring(0, 3) + ", " + time.ToString("dd-MM-yyyy"));
                        //notifs.Add(new Notification (Convert.ToInt32(reader["IdUser"]), "RequestId:" + Convert.ToString(reader["IdRequest"]) + ";UserName:" + Convert.ToString(reader["FirstName"]) + ";Status:Pending;Time:" + time.ToString("dddd").Substring(0, 3) + ", " + time.ToString("dd-MM-yyyy"), Convert.ToString(reader["Picture"]), DateTime.Now));
                    }


                    if (Convert.ToBoolean(reader["Approved"]))
                    {
                        DateTime pick = Convert.ToDateTime(reader["PickUp"]);
                        TimeSpan diff = pick.Subtract(DateTime.Now);
                        if (diff.TotalDays <= 1)
                        {
                            string remaining = "";

                            if (diff.TotalHours <= 1)
                                remaining = Convert.ToInt32(diff.TotalMinutes) + "minutes";

                            else
                                remaining = Convert.ToInt32(diff.TotalHours) + " hours";

                            requestStatuses.Rows.Add(reader["IdRequest"], "soon", reader["FirstName"], reader["Picture"], remaining);
                            notifs.Add(new Notification(Convert.ToInt32(reader["IdUser"]), "RequestId:" + Convert.ToString(reader["IdRequest"]) + ";UserName:" + Convert.ToString(reader["FirstName"]) + ";Status:Soon;Remaining:" + remaining, Convert.ToString(reader["Picture"]), DateTime.Now));
                        }

                    }
                    
                    notifications.Rows.Add(requestStatuses);
                }

                
            }

            if (con.State.ToString() == "Open")
            {
                reader.Close();
                con.Close();
            }


            foreach (var n in notifs)
            {
                CreateMenuNotification(n.IdUser, n.Data, n.Picture, DateTime.Now);
            }

            return notifications;
        }


        public DataTable GetDriverNotifications(string Email)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);

            DataTable notifications = new DataTable();
            notifications.Columns.Add(new DataColumn("Requests", typeof(object)));

            List<int> OffersIds = new List<int>();

            string query = "SELECT IdOffer FROM Offer WHERE IdUser = (SELECT IdUser FROM UserDetails WHERE Email = '" + Email + "')";
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    OffersIds.Add(Int32.Parse(Convert.ToString(reader["IdOffer"])));
                }
            }

            con.Close();


            if(OffersIds.Count>0)

            {
                string ids = "";

                foreach (var id in OffersIds)
                {
                    if (id.Equals(OffersIds.Last())) ids += "Identifier LIKE '" + id.ToString() + "-%'";
                    else ids += "Identifier LIKE '" + id.ToString() + "-%' OR ";
                    
                }

                ids += " AND ";

                List<List<object>> newRequests = new List<List<object>>();

                query = "SELECT Identifier FROM TableChange WHERE " + ids + " TableName = 'Request';";

                //if (dbConnection.State.ToString() == "Open")
                //{
                //    dbConnection.Close();
                //}

                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }

                command = new SqlCommand(query, con);
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        List<object> request = new List<object>() { Int32.Parse(Convert.ToString(reader["Identifier"]).Split('-')[0]), Int32.Parse(Convert.ToString(reader["Identifier"]).Split('-')[1]) };
                        newRequests.Add(request);
                    }
                }

                if (con.State.ToString() == "Open")
                {
                    reader.Close();
                    con.Close();
                }

                //string updatedIds = "";

                
                //if(newRequests.Count>0)
                //{
                    string ofrIds = "";
                    string rqstIds = "";

                    foreach (var ur in newRequests)
                    {
                        //if (ur.Equals(updatedRequests.Last())) updatedIds += "'" + ur[0].ToString() + "'";
                        //else updatedIds += "'" + ur[0].ToString() + "', ";

                        DeleteTableChange(ur[0].ToString() + "-" + ur[1].ToString(), "Request", "");

                        if (ur.Equals(newRequests.Last())) ofrIds += ur[0].ToString();
                        else ofrIds += ur[0].ToString() + ", ";

                        if (ur.Equals(newRequests.Last())) rqstIds += ur[1].ToString();
                        else rqstIds += ur[1].ToString() + ", ";
                    }

                    //string whereClause = "";
                    //if(newRequests.Count > 0)
                    //{
                    //    whereClause = "Request.IdRequest IN (" + rqstIds + ") AND Request.IdOffer IN(" + ofrIds + ")";
                    //}
                    

                    query = "SELECT Request.IdRequest, Request.IdOffer, Request.Approved, Request.Rejected, Offer.Departure AS Dep, Passenger.FirstName, Passenger.Picture, Driver.IdUser AS DriverId FROM Request JOIN UserDetails Passenger ON(Passenger.IdUser = Request.IdUser) JOIN UserDetails Driver ON (Driver.IdUser = (SELECT IdUser FROM UserDetails WHERE Email = '"+Email+"')) JOIN Offer ON(Offer.IdOffer = Request.IdOffer);";


                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }

                    command = new SqlCommand(query, con);
                    reader = command.ExecuteReader();

                    List<Notification> notifs = new List<Notification>();

                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            DataTable requestStatuses = new DataTable();
                            requestStatuses.Columns.Add(new DataColumn("IdRequest", typeof(int)));
                            requestStatuses.Columns.Add(new DataColumn("Status", typeof(string)));
                            requestStatuses.Columns.Add(new DataColumn("PassengerName", typeof(string)));
                            requestStatuses.Columns.Add(new DataColumn("PassengerPicture", typeof(string)));
                            requestStatuses.Columns.Add(new DataColumn("Time", typeof(string)));


                            foreach (var r in newRequests)
                            {

                                int id = int.Parse(r[0].ToString());
                                if (id == Convert.ToInt32(reader["IdOffer"]))
                                {
                                    DateTime time = Convert.ToDateTime(reader["Dep"]);

                                    requestStatuses.Rows.Add(reader["IdRequest"], "requested", reader["FirstName"], reader["Picture"], time.ToString("dddd").Substring(0, 3) + ", " + time.ToString("dd-MM-yyyy"));
                                    notifs.Add(new Notification(Convert.ToInt32(reader["DriverId"]), "RequestId:" + Convert.ToString(reader["IdRequest"]) + ";UserName:" + Convert.ToString(reader["FirstName"]) + ";Status:Requested;Time:" + time.ToString("dddd").Substring(0, 3) + ", " + time.ToString("dd-MM-yyyy"), Convert.ToString(reader["Picture"]), DateTime.Now));
                                }
                            }



                            if (Convert.ToBoolean(reader["Approved"]))
                            {
                                DateTime departure = Convert.ToDateTime(reader["Dep"]);
                                TimeSpan diff = departure.Subtract(DateTime.Now);
                                if (diff.TotalDays <= 1)
                                {
                                    string remaining = "";

                                    if (diff.TotalHours <= 1)
                                        remaining = Convert.ToInt32(diff.TotalMinutes) + "minutes";

                                    else
                                        remaining = Convert.ToInt32(diff.TotalHours) + " hours";

                                    requestStatuses.Rows.Add(reader["IdRequest"], "soon", reader["FirstName"], reader["Picture"], remaining);
                                    notifs.Add(new Notification(Convert.ToInt32(reader["DriverId"]), "RequestId:" + Convert.ToString(reader["IdRequest"]) + ";UserName:" + Convert.ToString(reader["FirstName"]) + ";Status:Soon;Remaining:" + remaining, Convert.ToString(reader["Picture"]), DateTime.Now));
                                }

                            }

                            notifications.Rows.Add(requestStatuses);
                        }


                    }

                    if (con.State.ToString() == "Open")
                    {
                        reader.Close();
                        con.Close();
                    }


                    foreach (var n in notifs)
                    {
                        CreateMenuNotification(n.IdUser, n.Data, n.Picture, DateTime.Now);
                    }
                //}
                


            }// End if Offers.Count > 0


            
            return notifications;
        }


        public void RecordTableChange(string identifier, string table, string column, string oldValue, string newValue)
        {

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }


            string query = "INSERT INTO TableChange (Identifier, TableName, ColumnName, OldValue, NewValue) VALUES('" + identifier + "', '" + table + "', '" + column + "', '" + oldValue + "', '" + newValue + "');";
            SqlCommand command = new SqlCommand(query, dbConnection);
            command.ExecuteNonQuery();

            dbConnection.Close();
        }


        public void DeleteTableChange(string identifier, string table, string column)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "DELETE FROM TableChange WHERE Identifier = '" + identifier + "' AND TableName = '" + table + "' AND ColumnName = '" + column + "';";
            SqlCommand command = new SqlCommand(query, dbConnection);
            command.ExecuteNonQuery();
            dbConnection.Close();
        }


        public bool CreateMenuNotification(int IdUser, string Data, string Picture, DateTime Time)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);
            bool created = false;
            bool duplicate = false;

            string soonClause = "";
            string[] splittedData = Data.Split(';');
            if (splittedData.Length >= 4 && splittedData[3].Split(':')[0].Equals("Remaining"))
            {
                soonClause = "OR Data LIKE '" + splittedData[0] + ";" + splittedData[1] + ";" + splittedData[2] + ";" + splittedData[3].Split(':')[0] + "%'";
            }

            string query = "SELECT * FROM Notification WHERE IdUser = "+IdUser+" AND (Data = '" + Data + "' "+soonClause+") AND Picture = '"+Picture+"'";

            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }

            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            
            if(reader.HasRows)
            {
                duplicate = true;
            }

            con.Close();

            if(!duplicate)
            {
                query = "WHILE (SELECT COUNT(*) FROM Notification WHERE IdUser = " + IdUser + ") > 5 BEGIN " +
                            "DELETE FROM Notification WHERE IdUser = " + IdUser + " AND Time = (SELECT MIN(Time) FROM Notification) " +
                            " END " +
                            "INSERT INTO Notification(IdUser, Data, Picture, Time, Seen) VALUES ( " + IdUser + ",'" + Data + "','" + Picture + "','" + Time.ToString("yyyy-MM-dd HH:mm:ss") + "', 'false');";



                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }

                command = new SqlCommand(query, con);
                int affected = command.ExecuteNonQuery();

                con.Close();

                created = affected > 0;
            }
            

            return created;
        }


        public DataTable GetMenuNotifications(string Email)
        {
            DataTable notifications = new DataTable();
            notifications.Columns.Add(new DataColumn("Data", typeof(string)));
            notifications.Columns.Add(new DataColumn("Picture", typeof(string)));
            notifications.Columns.Add(new DataColumn("Time", typeof(DateTime)));
            notifications.Columns.Add(new DataColumn("Seen", typeof(bool)));

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);

            string query = "SELECT * FROM Notification WHERE IdUser = (SELECT IdUser FROM UserDetails WHERE Email = '" + Email + "') ORDER BY IdNotification DESC;";

            //if (dbConnection.State.ToString() == "Open")
            //{
            //    dbConnection.Close();
            //}

            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }

            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();

            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    notifications.Rows.Add(reader["Data"], reader["Picture"], reader["Time"], reader["Seen"]);
                }
            }

            if (con.State.ToString() == "Open")
            {
                reader.Close();
                con.Close();
            }

            return notifications;
        }


        public bool MarkNotificationsAsSeen(string Email)
        {
            string query = "UPDATE Notification SET Seen = 'true' WHERE IdUser = (SELECT IdUser FROM UserDetails WHERE Email = '" + Email + "' AND Seen = 'false');";

            //if (dbConnection.State.ToString() == "Open")
            //{
            //    dbConnection.Close();
            //}

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);

            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }

            SqlCommand command = new SqlCommand(query, con);
            int affected = command.ExecuteNonQuery();

            if (con.State.ToString() == "Open")
            {
                con.Close();
            }

            return affected > 0;
        }


        //public bool PointInPolygon(double Lat, double Lng, List<List<double>> rangeIndices)
        //{
        //    //Ray-cast algorithm is here onward
        //    int k, j = rangeIndices.Count - 1;
        //    bool oddNodes = false; //to check whether number of intersections is odd
        //    Coordinates location = new Coordinates(Lat, Lng);

        //    for (k = 0; k < rangeIndices.Count; k++)
        //    {
        //        //fetch adjucent points of the polygon
        //        Coordinates polyK = new Coordinates(rangeIndices[k][0], rangeIndices[k][1]);
        //        Coordinates polyJ = new Coordinates(rangeIndices[j][0], rangeIndices[j][1]);

        //        //check the intersections
        //        if (((polyK.lng > location.lng) != (polyJ.lng > location.lng)) &&
        //         (location.lat < (polyJ.lat - polyK.lat) * (location.lng - polyK.lng) / (polyJ.lng - polyK.lng) + polyK.lat))
        //            oddNodes = !oddNodes; //switch between odd and even
        //        j = k;
        //    }

        //    return oddNodes;
        //}


        //public bool DestinationInSameDirection(double startLat, double startLng, double destinationLat, double destinationLng, List<List<double>> routePoints)
        //{
        //    int startIndex = ClosestPointIndex(startLat, startLng, routePoints);
        //    int destIndex = ClosestPointIndex(destinationLat, destinationLng, routePoints);
        //    bool b = false;

        //    if (destIndex > startIndex)
        //        b = true;

        //    return b;
        //}


        //public int ClosestPointIndex(double Lat, double Lng, List<List<double>> routePoints)
        //{
        //    double distance = (Math.Sqrt(Math.Pow(Math.Abs(Lat - routePoints[0][0]), 2) + Math.Pow(Math.Abs(Lng - routePoints[0][1]), 2)));
        //    int index = 0;

        //    for (int i = 1; i < routePoints.Count; i++)
        //    {
        //        double tempDist;
        //        tempDist = (Math.Sqrt(Math.Pow(Math.Abs(Lat - routePoints[i][0]), 2) + Math.Pow(Math.Abs(Lng - routePoints[i][1]), 2)));

        //        if (tempDist < distance)
        //        {
        //            distance = tempDist;
        //            index = i;
        //        }
        //    }

        //    return index;
        //}


        //public class Coordinates
        //{
        //    public double lat { get; set; }
        //    public double lng { get; set; }

        //    public Coordinates() { }

        //    public Coordinates(double x, double y)
        //    {
        //        lat = x;
        //        lng = y;
        //    }

        //    public Coordinates(string x, string y)
        //    {
        //        lat = double.Parse(x, CultureInfo.InvariantCulture);
        //        lng = double.Parse(y, CultureInfo.InvariantCulture);
        //    }
        //}


        public void sendMessageToFirebase(string To, string Title, string Body, string Type, string SenderEmail, string OfferId, string Date)
        {
            const string AUTH = "AIzaSyAzmJHrw3WeLnVVqjVWemfLg7edmWS7nnA";
            const string SENDERID = "160495830166";
            
            try
            {
                
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                var message = new
                {
                    to = To,
                    priority = "high",
                    //content-available = "true",
                    data = new

                    {
                        title = Title,
                        text = Body,
                        type = Type,
                        sender = SenderEmail,
                        offer = OfferId,
                        date = Date
                    }
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(message);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", AUTH));
                //tRequest.Headers.Add(string.Format("Sender: id={0}", SENDERID));
                tRequest.ContentLength = byteArray.Length;
                
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {

                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;

                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                string str = ex.Message;
            }
            
        }


        public class Notification
        {
            public int IdUser { get; set; }
            public string Data { get; set; }
            public string Picture { get; set; }
            public DateTime Time { get; set; }

            public Notification() { }

            public Notification(int idUser, string data, string picture, DateTime time)
            {
                IdUser = idUser;
                Data = data;
                Picture = picture;
                Time = time;
            }
        }

    }
}