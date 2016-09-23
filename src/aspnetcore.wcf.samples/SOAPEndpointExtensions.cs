using System.ServiceModel.Channels;
using Aspnetcore.Wcf.Samples;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SoapEndpointExtensions
    {
        public static IApplicationBuilder UseSoapEndpoint<T>(this IApplicationBuilder builder, string path,
            Binding binding)
        {
            var encoder =
                binding.CreateBindingElements()
                    .Find<MessageEncodingBindingElement>()?.CreateMessageEncoderFactory()
                    .Encoder;
            return builder.UseMiddleware<SoapEndpointMiddleware>(typeof(T), path, encoder);
        }

        public static IApplicationBuilder UseSoapEndpoint<T>(this IApplicationBuilder builder, string path,
            MessageEncoder encoder) => builder.UseMiddleware<SoapEndpointMiddleware>(typeof(T), path, encoder);
    }
}