namespace Truss.Modeling.Infrastructure.Configuration;

public sealed class EmptyConfigurationInteractionException() : Exception("The configuration is empty. If a configuration is needed provide a configuration to the Truss service");