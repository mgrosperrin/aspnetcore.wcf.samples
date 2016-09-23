using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace Aspnetcore.Wcf.Samples
{
    public class ServiceDescription
    {
        private readonly Lazy<IEnumerable<ContractDescription>> _contractsLazy;
        public ServiceDescription(Type serviceType)
        {
            ServiceType = serviceType;

            _contractsLazy = new Lazy<IEnumerable<ContractDescription>>(ExtractContractDescriptions);
        }

        public Type ServiceType { get; }
        public IEnumerable<ContractDescription> Contracts => _contractsLazy.Value;
        public IEnumerable<OperationDescription> Operations => Contracts.SelectMany(c => c.Operations);
        private IEnumerable<ContractDescription> ExtractContractDescriptions()
        {
            var contracts = new List<ContractDescription>();
            foreach (var contractInterfaceType in ServiceType.GetInterfaces())
            {
                var contractDescriptionsForInterface = ExtractContractDescriptionsFromContractInterfaceType(contractInterfaceType);
                contracts.AddRange(contractDescriptionsForInterface);
            }
            return contracts;
        }

        private IEnumerable<ContractDescription> ExtractContractDescriptionsFromContractInterfaceType(
            Type contractInterfaceType)
        {
            foreach (var serviceContract in contractInterfaceType.GetTypeInfo().GetCustomAttributes<ServiceContractAttribute>())
            {
                yield return new ContractDescription(this, contractInterfaceType, serviceContract);
            }
        }
    }
}