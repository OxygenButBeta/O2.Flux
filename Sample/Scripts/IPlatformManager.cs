
using O2.Flux;


/// <summary>
/// I Service is the base interface for all services in the O2.Flux system.
/// It doesn't define any members itself, but it serves as a marker interface to identify service types and allows for type constraints in service management.
/// </summary>
public interface IPlatformManager : IService
{
    void Initialize();
}
