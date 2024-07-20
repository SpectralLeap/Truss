using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Truss.Testing.SharedDependencies;

namespace Truss.Testing.Services;

internal sealed class SharedDependencyManager : IAsyncDisposable
{
    public IServiceCollection SharedDependencyAdapters { get; private set; } 
        = new ServiceCollection();
    
    private readonly List<Assembly> _assemblies;
    private List<ISharedDependency>? _sharedDependencies;

    public SharedDependencyManager(IEnumerable<Assembly> assemblies)
    {
        _assemblies = assemblies.ToList();
    }

    public async Task Start()
    {
        FindAllSharedDependencies();
        
        await StartTheInstances(); 
        
        RegisterAdapters();
    }

    private void FindAllSharedDependencies()
    {
        _sharedDependencies = _assemblies
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(type => !type.IsAbstract
                                   && !type.IsInterface
                                   && typeof(ISharedDependency).IsAssignableFrom(type)))
                .Select(type => Activator.CreateInstance(type))
                .Cast<ISharedDependency>()
                .ToList()
            ;
    }

    private async Task StartTheInstances()
    {
        var startTasks = _sharedDependencies
            .Select(dependency => dependency.StartAsync());
                    
        // need to get the adapters after the
        // initialization so they are added
        await Task.WhenAll(startTasks);
         
    }

    private void RegisterAdapters()
    {
        var adapterServices = new ServiceCollection();
        
        foreach (var dependency in _sharedDependencies!)
        {
            var fieldsWithAttribute = dependency.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => Attribute.IsDefined(f, typeof(SharedDependencyAdapterAttribute)));
            
            foreach (var field in fieldsWithAttribute)
            {
                var adapterInstance = field.GetValue(dependency);
                if (adapterInstance != null)
                {
                    adapterServices.AddSingleton(field.FieldType, adapterInstance);
                }
            }
            
        }

        SharedDependencyAdapters = adapterServices;
    }

    public async ValueTask DisposeAsync()
    {
        var asyncDisposeTasks = _sharedDependencies!
            .Select(dependency => dependency.DisposeAsync().AsTask());
                
        await Task.WhenAll(asyncDisposeTasks);
    }

}