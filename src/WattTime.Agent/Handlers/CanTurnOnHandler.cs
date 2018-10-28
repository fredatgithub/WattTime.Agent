using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using WattTime.Agent.Interaction;

namespace WattTime.Agent.Handlers
{
    public class CanTurnOnHandler : IRequestHandler
    {
        public bool CanHandle(IServiceProvider context, SkillRequest request) =>
            request.Request.Type == InteractionModel.RequestType.IntentRequest
            && request.Request is IntentRequest intentRequest
            && StringComparer.OrdinalIgnoreCase.Equals(intentRequest.Intent.Name, InteractionModel.Intent.CanTurnOn);

        public Task<SkillResponse> Handle(IServiceProvider context, SkillRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
