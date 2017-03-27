using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlexaSkillsKit.Speechlet;

namespace GardenaAlexa.Controllers
{
    interface ISpeechletAsync
    {
        Task<SpeechletResponse> OnIntentAsync(IntentRequest intentRequest, Session session);
        Task<SpeechletResponse> OnLaunchAsync(LaunchRequest launchRequest, Session session);
        Task OnSessionStartedAsync(SessionStartedRequest sessionStartedRequest, Session session);
        Task OnSessionEndedAsync(SessionEndedRequest sessionEndedRequest, Session session);
    }
}
