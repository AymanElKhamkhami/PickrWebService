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



            ServiceAPI s = new ServiceAPI();
            //s.sendMessageToFirebase("/topics/testTopic" , "REST", "That's the first notification you receive from our webservice. Welcome onboard!", "alert");
            //s.CreateRequest("Tester1@gmail.com", 2, 51.769327118381300, 19.435501098632800, 51.765449591887500, 19.456357955932600, DateTime.Now, DateTime.Now.AddMinutes(5), 1);
            s.RespondToRequest(29, DateTime.Now.AddDays(1), false);
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