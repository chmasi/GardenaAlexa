using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gardena.NET;
using Gardena.NET.Models;
using System.Net;
using GardenaAlexa.Properties;
using NLog;

namespace GardenaAlexa.Gardena
{
    public class Mower
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        readonly GardenaAPI GardenaApi = new GardenaAPI();

        string mowerName = string.Empty;
        string message = String.Format(format: "Enschuldigung, leider ist ein Fehler mit der Verbindung zum Rasenmäher aufgetreten, bitte prüfe die Gateway Verbindung zum Rasenmäher und versuche es noch einmal.");

        public string GetMowerStatus(string email, string password, string command)
        {
            Log.Info("GetMowerStatus email={0}, password={1}, command={2}", email, password, command);

            Login myGardenaLogin = GardenaApi.GardenaLogin(Settings.Default.GardenaUser, Settings.Default.GardenaUserPassword);
            Log.Info("GetMowerStatus myGadenaLogin={0}", myGardenaLogin);

            if (myGardenaLogin.sessions.status_message.ToUpper() == "OK")
            {
                Locations myGardenaLocations = GardenaApi.GetGardenaLocation(myGardenaLogin.sessions.user_id, myGardenaLogin.sessions.token);
                if (myGardenaLocations.status_message.ToUpper() == "OK")
                {
                    foreach (var location in myGardenaLocations.locations)
                    {
                        Devices myGardenaDevices = GardenaApi.GetGardenaDevices(location.id, myGardenaLogin.sessions.token);

                        if (myGardenaDevices.status_message.ToUpper() == "OK")
                        {
                            foreach (var device in myGardenaDevices.devices)
                            {
                                if (device.category.ToLower() == "mower")
                                {
                                    mowerName = device.name;
                                    if (command == "status")
                                    {
                                        
                                        string statusMower = device.abilities[4].properties[1].value.ToString();
                                        string timestampMowerStatus = device.abilities[4].properties[1].timestamp.ToString();

                                        Log.Info("GetMowerStatus statusMower={0}", statusMower);
                                        switch (statusMower)
                                        {
                                            case "paused":
                                                message = String.Format("{0} macht gerade Pause.", mowerName);
                                                break;
                                            case "ok_cutting":
                                                message = String.Format("{0} mäht gerade.", mowerName);
                                                break;
                                            case "ok_cutting_timer_overridden":
                                                message = String.Format("{0} mäht gerade.", mowerName);
                                                break;
                                            case "og_searching":
                                                message = String.Format("{0} ist gerade auf der suche.", mowerName);
                                                break;
                                            case "ok_charging":
                                                message = String.Format("{0} ist am aufladen.", mowerName);
                                                break;
                                            case "ok_leaving":
                                                message = String.Format("{0} macht sich auf den weg.", mowerName);
                                                break;
                                            case "wait_updating":
                                                message = String.Format("{0} wartet auf ein update.", mowerName);
                                                break;
                                            case "wait_power_up":
                                                message = String.Format("{0} wird aufgeladen.", mowerName);
                                                break;
                                            case "parked_timer":
                                                message = String.Format("{0} ist laut Zeitplan geparkt.", mowerName);
                                                break;
                                            case "parked_park_selected":
                                                message = String.Format("{0} ist bis auf weiteres geparkt.", mowerName);
                                                break;
                                            case "off_disabled":
                                                message = String.Format("{0} ist ausgeschaltet.", mowerName);
                                                break;
                                            case "off_hatch_open":
                                                message = String.Format("{0}'s Abdeckung ist offen.", mowerName);
                                                break;
                                            case "unknown":
                                                message = String.Format("{0} weis nicht wie der Status gerade ist.", mowerName);
                                                break;
                                            case "error":
                                                message = String.Format("{0} hat einen Fehler registriert.", mowerName);
                                                break;
                                            case "error_at_power_up":
                                                message = String.Format("{0} hat einen Fehler beim Einschalten festgestellt.", mowerName);
                                                break;
                                            case "off_hatch_closed":
                                                message = String.Format("{0} ist deaktiviert und muss manuell gestartet werden.", mowerName);
                                                break;
                                            default:
                                                message = String.Format("{0} hat einen unbekannten Status.", mowerName);
                                                break;
                                        }
                                        break;
                                    }
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            return message;
        }

        public string SendMowerCommand(string email, string password, string command, int commandParameter = 0, string entity = "")
        {
            Log.Info("SendMowerCommand email={0}, password={1}, command={2}, commandParameter={3}", email, password, command, commandParameter);
            Login myGardenaLogin = GardenaApi.GardenaLogin(Settings.Default.GardenaUser, Settings.Default.GardenaUserPassword);
            if (myGardenaLogin.sessions.status_message.ToUpper() == "OK")
            {
                Locations myGardenaLocations = GardenaApi.GetGardenaLocation(myGardenaLogin.sessions.user_id, myGardenaLogin.sessions.token);
                if (myGardenaLocations.status_message.ToUpper() == "OK")
                {
                    foreach (var location in myGardenaLocations.locations)
                    {
                        Devices myGardenaDevices = GardenaApi.GetGardenaDevices(location.id, myGardenaLogin.sessions.token);

                        if (myGardenaDevices.status_message.ToUpper() == "OK")
                        {
                            foreach (var device in myGardenaDevices.devices)
                            {
                                if (device.category.ToLower() == "mower")
                                {
                                    mowerName = device.name;
                                    if (command == "status")
                                    {
                                        string statusMower = device.abilities[4].properties[1].value.ToString();
                                        string timestampMowerStatus = device.abilities[4].properties[1].timestamp.ToString();

                                        switch (statusMower)
                                        {
                                            case "paused":
                                                message = String.Format("{0} macht gerade Pause.", mowerName);
                                                break;
                                            case "ok_cutting":
                                                message = String.Format("{0} mäht gerade.", mowerName);
                                                break;
                                            case "og_searching":
                                                message = String.Format("{0} ist gerade auf der suche.", mowerName);
                                                break;
                                            case "ok_charging":
                                                message = String.Format("{0} ist am aufladen.", mowerName);
                                                break;
                                            case "ok_leaving":
                                                message = String.Format("{0} macht sich auf den weg.", mowerName);
                                                break;
                                            case "wait_updating":
                                                message = String.Format("{0} wartet auf ein update.", mowerName);
                                                break;
                                            case "wait_power_up":
                                                message = String.Format("{0} wird aufgeladen.", mowerName);
                                                break;
                                            case "parked_timer":
                                                message = String.Format("{0} ist laut Zeitplan geparkt.", mowerName);
                                                break;
                                            case "parked_park_selected":
                                                message = String.Format("{0} ist bis auf weiteres geparkt.", mowerName);
                                                break;
                                            case "off_disabled":
                                                message = String.Format("{0} ist ausgeschaltet.", mowerName);
                                                break;
                                            case "off_hatch_open":
                                                message = String.Format("{0}'s Abdeckung ist offen.", mowerName);
                                                break;
                                            case "unknown":
                                                message = String.Format("{0} weis nicht wie der Status gerade ist.", mowerName);
                                                break;
                                            case "error":
                                                message = String.Format("{0} hat einen Fehler registriert.", mowerName);
                                                break;
                                            case "error_at_power_up":
                                                message = String.Format("{0} hat einen Fehler beim Einschalten festgestellt.", mowerName);
                                                break;
                                            case "off_hatched_closed":
                                                message = String.Format("{0}'s Abdeckung wurde geschlossen.", mowerName);
                                                break;
                                            default:
                                                break;
                                        }
                                        break;
                                    }

                                    TimeSpan span = new TimeSpan(0, commandParameter, 0);
                                    int normalizedDays = span.Days;
                                    int normalizedHours = span.Hours;
                                    int normalizedMinutes = span.Minutes;
                                    int normalizedSeconds = span.Seconds;
                                    message = String.Format("Entschuldigung, Ich konnte den Auftrag nicht verstehen.", mowerName);

                                    HttpStatusCode myGardenaCommand = GardenaApi.GardenaSendCommand(device.id, location.id, myGardenaLogin.sessions.token, command, commandParameter);
                                    if (myGardenaCommand.ToString().ToUpper() == "OK" || myGardenaCommand.ToString().ToUpper() == "NOCONTENT")
                                    {
                                        if (command == "park_until_next_timer")
                                        {
                                            message = String.Format("Ok, Ich habe {0} den Auftrag erteilt bis zum nächsten Zeitplan zu Parken.", mowerName);
                                        }
                                        if (command == "park_until_further_notice")
                                        {
                                            message = String.Format("Ok, Ich habe {0} den Auftrag zum Parken erteilt und habe den Zeitplan unterbrochen.", mowerName);
                                        }
                                        if (command == "start_resume_schedule")
                                        {
                                            message = String.Format("Ok, Ich habe {0} den Auftrag zum fortsetzen des Zeitplans erteilt.", mowerName);
                                        }
                                        if (command == "start_override_timer")
                                        {
                                            message = String.Format("Ok, Ich habe {0} den Auftrag erteilt die nächsten {1} Tage, {2} Stunden, {3} Minuten zu mähen", mowerName, normalizedDays.ToString(), normalizedHours.ToString(), normalizedMinutes.ToString());
                                        }
                                    }
                                    
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            return message;
        }

        public string CreateScheduleEntry(string email, string password, string command, int commandParameter = 0, string entity = "")
        {
            Login myGardenaLogin = GardenaApi.GardenaLogin(Settings.Default.GardenaUser, Settings.Default.GardenaUserPassword);
            if (myGardenaLogin.sessions.status_message.ToUpper() == "OK")
            {
                Locations myGardenaLocations = GardenaApi.GetGardenaLocation(myGardenaLogin.sessions.user_id, myGardenaLogin.sessions.token);
                if (myGardenaLocations.status_message.ToUpper() == "OK")
                {
                    foreach (var location in myGardenaLocations.locations)
                    {
                        Devices myGardenaDevices = GardenaApi.GetGardenaDevices(location.id, myGardenaLogin.sessions.token);

                        if (myGardenaDevices.status_message.ToUpper() == "OK")
                        {
                            foreach (var device in myGardenaDevices.devices)
                            {
                                if (device.category.ToLower() == "mower")
                                {
                                    mowerName = device.name;
                                    //var test1 = GardenaApi.GardenaDeleteSchedule(device.id, "6a3605a0-100a-41d8-aee0-c3b7df4344e1", location.id, myGardenaLogin.sessions.token);
                                    List<string> myWeekDays = new List<string>();
                                    myWeekDays.Add("Monday");
                                    myWeekDays.Add("Saturday");
                                    var test = GardenaApi.GardenaCreateSchedule(device.id, location.id, myGardenaLogin.sessions.token, "18:00","18:01","active","weekly", myWeekDays);
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            return message;
        }
    }
}