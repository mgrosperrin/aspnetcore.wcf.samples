using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Xml;

namespace Aspnetcore.Wcf.Samples
{
    public class ServiceBodyWriter : BodyWriter
    {
        private readonly string _envelopeName;
        private readonly object _result;
        private readonly string _resultName;
        private readonly string _serviceNamespace;

        public ServiceBodyWriter(string serviceNamespace, string envelopeName, string resultName, object result)
            : base(true)
        {
            _serviceNamespace = serviceNamespace;
            _envelopeName = envelopeName;
            _resultName = resultName;
            _result = result;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement(_envelopeName, _serviceNamespace);
            var serializer = new DataContractSerializer(_result.GetType(), _resultName, _serviceNamespace);
            serializer.WriteObject(writer, _result);
            writer.WriteEndElement();
        }
    }
}