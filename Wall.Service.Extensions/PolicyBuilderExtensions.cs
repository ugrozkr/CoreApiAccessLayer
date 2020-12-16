using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Wall.Service.Extensions
{
    public static class PolicyBuilderExtensions
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(5,
                    retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(1.5, retryAttempt) * 1000),
                    (_, waitingTime) =>
                    {
                        Console.WriteLine("Polly retry policy çalıştırılıyor.");
                    });
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(3, TimeSpan.FromSeconds(15));
        }
    }
}
