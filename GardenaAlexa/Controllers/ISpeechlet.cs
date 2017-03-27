using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlexaSkillsKit.Speechlet;

namespace GardenaAlexa.Controllers
{
    public interface ISpeechlet
    {
        SpeechletResponse OnIntent(IntentRequest intentRequest, Session session);
        SpeechletResponse OnLaunch(LaunchRequest launchRequest, Session session);
        void OnSessionStarted(SessionStartedRequest sessionStartedRequest, Session session);
        void OnSessionEnded(SessionEndedRequest sessionEndedRequest, Session session);
    }
}