using JsonServices.Web;
using JsonServices;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Collections.Generic;
using System;
using System.Timers;
using System.Web;

namespace JSONWebService
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : JsonHandler
    {
        
        public Handler()
        {
            this.service.Name = "JSONWebService";
            this.service.Description = "Pickr JSON Web Service";
            InterfaceConfiguration IConfig = new InterfaceConfiguration("PickrWebService", typeof(IServiceAPI), typeof(ServiceAPI));
            this.service.Interfaces.Add(IConfig);


            //if(HttpContext.Current.Items["Email"] != null)
            //{
            //    string email = (string)HttpContext.Current.Items["Email"];
            //}



            //ServiceAPI s = new ServiceAPI();
            //s.GetRideDetails(34, "driver");
            //s.sendMessageToFirebase("f0mgqsQRFBE:APA91bEkTncKzEQmQ8H9zj6QRwfRg4THXSa1eTOEiw1JieebuAWdXp5IMpNjmQ9lAFaE93C37GzQnc_qdIo-8UEqmRBbBlsvVW-KswFPvjNMNWpDkfRk5exRoD0RnDKcpppZXqYvmkm7", "Feedback", "Ayman", "approved", "ayman.khm%gmail.com", "31", "12/12/2016 10:25", "https://scontent.xx.fbcdn.net/v/t1.0-1/c59.0.200.200/p200x200/10354686_10150004552801856_220367501106153455_n.jpg?oh=da4eafdad0da0d0958352977771c3fd7&oe=58BE4325");
            //s.CreateRequest("Tester1@gmail.com", 2, 51.769327118381300, 19.435501098632800, 51.765449591887500, 19.456357955932600, new DateTime(2016, 12, 9, 14, 20, 00), new DateTime(2016, 12, 9, 14, 25, 00), 1);
            //s.RespondToRequest(31, new DateTime(2016, 12, 9, 14, 23, 00), true);
            //s.sendMessageToFirebase("/topics/ayman.khm%gmail.com" , "Ride request", "Tester" + "sent you a request to join your ride on ", "request", "Tester1@gmail.com", "2");
            ///s.CreatePreferences(true, true, true, 3);
            /*TimeSpan alertTime = new TimeSpan(17, 30, 00);
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = new TimeSpan(00, 01, 00);//alertTime - current.TimeOfDay;
            if(timeToGo < TimeSpan.Zero)
            {
                return; // time already passed
            }
            System.Threading.Timer timer = new System.Threading.Timer(x =>
            {
                s.CreatePreferences(true, true, true, 10);
            }, null, timeToGo, System.Threading.Timeout.InfiniteTimeSpan);*/

        }
        

    }
}