using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Apis.Calendar;
using System.Threading.Tasks;

namespace CalendarApi
{
    public class CalendarClass
    {
       
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly, CalendarService.Scope.CalendarEvents, CalendarService.Scope.Calendar };
        public string HostAnAppoinment(DateTime Start, DateTime End, string[] MailIds)
        {
            string credPath = "token.json";                 //file name holds the authentication credentials
            string Eventsummary = "Appointment with CEO";   //Event summary
            string location = "Smart City Kochi";           //Location of the event held
            string timezone = "Asia/Calcutta";              //Time zone of the event
            string Org_Email = "udhaya.sgn@gadgeon.com";    // organizer's mail id
            string displayname = "Organizer";               // details of organizer

            var eventAttends = new List<EventAttendee>();
            foreach (var mail in MailIds)
            {
                eventAttends.Add(new EventAttendee
                {
                    Email = mail
                });
            }

            //List<EventAttendee>
            UserCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                //Console.WriteLine("Credential file saved to: " + credPath);
            }
            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            });

            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = Start;
            request.TimeMax = End;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            // List events.
            Events events = request.Execute();


            if (events.Items == null || events.Items.Count == 0)
            {

                Event myEvent = new Event
                {
                    Summary = Eventsummary,
                    Location = location,
                    Start = new EventDateTime()
                    {
                        DateTime = Start,
                        TimeZone = timezone
                    },
                    End = new EventDateTime()
                    {
                        DateTime = End,
                        TimeZone = timezone
                    },
                    Organizer = new Event.OrganizerData()
                    {
                        Email = Org_Email,
                        DisplayName = displayname
                    },
                    Attendees = eventAttends,
                    Reminders = new Event.RemindersData()
                    {
                        UseDefault = false,
                        Overrides = new List<EventReminder>()

                            {
                                new EventReminder()
                                {
                                    Method = "email",
                                    Minutes = 10
                                }
                            }
                    }
                };


                var hostEvent = service.Events.Insert(myEvent, "primary");
                hostEvent.SendNotifications = true;
                var calendarEventResult = hostEvent.Execute();

                return "Interview is Scheduled from " + Start.ToString() + " To " + End.ToString();
            }

            return "Sorry there is another appoinment at same time Please check your calender.";
        }
    }
}
