using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using System;
using System.Threading.Tasks;
using WattTime.Agent.Interaction;

namespace WattTime.Agent.Handlers
{
    public class StopIntentHandler : IRequestHandler
    {
        public bool CanHandle(IServiceProvider context, SkillRequest request) =>
            request.Request.Type == InteractionModel.RequestType.IntentRequest
            && request.Request is IntentRequest intentRequest
            && (StringComparer.OrdinalIgnoreCase.Equals(intentRequest.Intent.Name, InteractionModel.Intent.Stop)
                || StringComparer.OrdinalIgnoreCase.Equals(intentRequest.Intent.Name, InteractionModel.Intent.Cancel));

        public Task<SkillResponse> Handle(IServiceProvider context, SkillRequest request)
        {
            return Task.FromResult(new SkillResponse()
            {
                Response = new ResponseBody()
                {
                    ShouldEndSession = true,
                    OutputSpeech = new PlainTextOutputSpeech()
                    {
                        Text = "Thank you for caring for our planet. Goodbye."
                    }
                }
            });
        }
    }
}
