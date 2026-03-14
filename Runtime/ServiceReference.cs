// namespace O2.Flux {
//     /// <summary>
//     /// A struct that holds a reference to a service of type TService.
//     /// It allows you to get the service instance without having to call Service.Get() every time.
//     /// </summary>
//     /// <typeparam name="TService"></typeparam>
//     public struct ServiceReference<TService> where TService : class {
//         public TService Service => Service<TService>.Get();
//         public static implicit operator TService(ServiceReference<TService> reference) => reference.Service;
//     }
// }