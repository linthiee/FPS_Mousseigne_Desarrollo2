using UnityEngine;

public static class GlobalBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeServices()
    {
        ServiceLocator.ClearServices();

        IEventBus globalEventBus = new EventBus();
        ServiceLocator.AddService<IEventBus>(globalEventBus);

        //AudioSettingsSO audioSettings = Resources.Load<AudioSettingsSO>("AudioSettings");

        Debug.Log("Initialized global services");

    }
}