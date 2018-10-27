namespace WattTime.Agent.Interaction
{
    public static class InteractionModel
    {
        public static class SessionAttribute
        {
            public const string WattTimeToken = nameof(WattTimeToken);
            public const string BalancingAuthority = nameof(BalancingAuthority);
        }

        public static class Permissison
        {
            public const string CountryAndPostalCode = "read::alexa:device:all:address:country_and_postal_code";
        }

        public static class RequestType
        {
            public const string LaunchRequest = "LaunchRequest";
            public const string IntentRequest = "IntentRequest";
            public const string SessionEndedRequest = "SessionEndedRequest";
        }

        public static class Intent
        {
            public const string Stop = "AMAZON.Stop";
        }
    }
}
