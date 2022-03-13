using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Impact_Aware_Alert
{
    
    class Program
    {

        private static bool running = true;
        private static string username = "REMOVED";
        private static string password = "REMOVED";

        public static SmtpClient createAndGetMessenger()
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true,
            };

            return smtpClient;

        }

        public class Keys
        {
            public string username = "";
            public string password = "";
        }

        public static bool LoadJson()
        {

            using (StreamReader r = new StreamReader("C:/Users/jaymi/source/repos/Impact-Aware-Alert/Impact-Aware-Alert/test.json"))
            {
                bool running = false;
                string json = r.ReadToEnd();
                dynamic items = JsonConvert.DeserializeObject(json);
                //List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
                int maxHits = 20;
                foreach (var h in items)
                {
                    if ((int)h.hits > maxHits)
                    {
                        running = false;
                        sendMessage();
                        return running;
                    }

                    else
                    {
                        running = true;
                    }
                }
            }
                return running;
        }

        public class Item
        {
            public String deviceID;
            public int hits;
            public bool connected;
            public int lastHit;
        }

        public static void sendMessage()
        {
            SmtpClient client = createAndGetMessenger();
            client.Send("Impact.Aware.Alerts@gmail.com", "jjhaver@gmu.edu", "IMPACT ALERT", 
                "You have been notified because there was an impact detected near you. " +
                "\nBecause this is just a project I have to say there is actually no danger at all and this email is purely for testing purposes.");
        }

        public static async void Wait(int time)
        {
            await Task.Delay(1000);
        }

        static void Main(string[] args)
        {
            while (running)
            {
                running = LoadJson();
                if (!running)
                {
                    break;
                }
            }
        }


    }


}
