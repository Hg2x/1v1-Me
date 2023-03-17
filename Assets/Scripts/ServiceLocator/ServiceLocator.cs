using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ICKT.Services
{
	public class ServiceLocator : MonoBehaviour
	{
		private static readonly Dictionary<Type, object> Services = new();

		public void Initialize()
		{
			foreach (var serviceType in GetAllAutoRegisteredServices())
			{
				if (IsRegistered(serviceType)) continue;

				if (serviceType.IsTypeOf<MonoBehaviour>())
				{
					FindOrCreateMonoService(serviceType);
				}
				else
				{
					RegisterNewInstance(serviceType);
				}
			}
		}

		public static IEnumerable<Type> GetAllAutoRegisteredServices()
		{
			return AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypesWithCustomAttribute<AutoRegisteredService>())
				.Where(service => typeof(IRegisterable).IsAssignableFrom(service));
		}

		public static void Register<TService>(TService service, bool safe = true) where TService : IRegisterable, new()
		{
			var serviceType = typeof(TService);
			if (IsRegistered<TService>() && safe)
			{
				throw new ServiceLocatorException($"{serviceType.Name} has been already registered.");
			}

			Services[typeof(TService)] = service;
		}

		public static void Unregister<TService>(TService service) where TService : IRegisterable
		{
			var serviceType = typeof(TService);
			if (!IsRegistered<TService>())
			{
				throw new ServiceLocatorException($"{serviceType.Name} hasn't been registered.");
			}

			var registeredService = Services[serviceType];
			if (registeredService.Equals(service))
			{
				Services.Remove(serviceType);
			}
			else
			{
				throw new ServiceLocatorException($"The registered instance of {serviceType.Name} doesn't match the provided instance.");
			}
		}

		public static TService Get<TService>(bool forced = false) where TService : IRegisterable, new()
		{
			var serviceType = typeof(TService);
			if (IsRegistered<TService>())
			{
				return (TService)Services[serviceType];
			}
			if (!forced)
				throw new ServiceLocatorException($"{serviceType.Name} hasn't been registered.");

			var service = serviceType.IsTypeOf<MonoBehaviour>() ?
				(TService)FindOrCreateMonoService(serviceType) : new TService();

			Register(service);
			return service;
		}

		public static bool IsRegistered(Type t)
		{
			return Services.ContainsKey(t);
		}

		public static bool IsRegistered<TService>()
		{
			return IsRegistered(typeof(TService));
		}

		private static void RegisterNewInstance(Type serviceType)
		{
			Services[serviceType] = Activator.CreateInstance(serviceType);
		}

		private static object FindOrCreateMonoService(Type serviceType)
		{
			var inGameService = FindObjectOfType(serviceType);
			if (inGameService == null)
			{
				var newObject = new GameObject();
				newObject.AddComponent(serviceType);
				newObject.name = serviceType.Name;

				inGameService = newObject.GetComponent(serviceType);

				if (inGameService is IRegisterable registerable && registerable.IsPersistent())
				{
					DontDestroyOnLoad(newObject);
				}
			}
			Services[serviceType] = inGameService;
			return inGameService;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class AutoRegisteredService : Attribute { }

	public class ServiceLocatorException : Exception
	{
		public ServiceLocatorException(string message) : base(message) { }
	}

}
