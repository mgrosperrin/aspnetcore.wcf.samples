using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;

namespace Aspnetcore.Wcf.Samples
{
    public class ContractDescription
    {
        private readonly Lazy<IEnumerable<OperationDescription>> _operationsLazy;

        public ContractDescription(ServiceDescription service, Type contractType, ServiceContractAttribute attribute)
        {
            Service = service;
            ContractType = contractType;
            Namespace = attribute.Namespace ?? "http://tempuri.org/"; // Namespace defaults to http://tempuri.org/
            Name = attribute.Name ?? ContractType.Name; // Name defaults to the type name
            _operationsLazy = new Lazy<IEnumerable<OperationDescription>>(ExtractOperationDescriptions);
        }

        public ServiceDescription Service { get; }
        public string Name { get; }
        public string Namespace { get; }
        public Type ContractType { get; }
        public IEnumerable<OperationDescription> Operations => _operationsLazy.Value;

        private IEnumerable<OperationDescription> ExtractOperationDescriptions()
        {
            var operations = new List<OperationDescription>();
            foreach (var operationMethodInfo in ContractType.GetTypeInfo().DeclaredMethods)
            {
                var operationDescriptionForMethodIndo = ExtractOperationDescriptionsFromOperationMethod(operationMethodInfo);
                operations.AddRange(operationDescriptionForMethodIndo);
            }
            return operations;
        }

        private IEnumerable<OperationDescription> ExtractOperationDescriptionsFromOperationMethod(
            MethodInfo operationMethodInfo)
        {
            foreach (var operationContract in operationMethodInfo.GetCustomAttributes<OperationContractAttribute>())
            {
                yield return new OperationDescription(this, operationMethodInfo, operationContract);
            }
        }
    }
}