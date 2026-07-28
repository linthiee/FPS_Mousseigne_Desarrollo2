using UnityEngine;

public static class GlobalBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeServices()
    {
        ServiceLoader.ClearServices();

        IEventBus globalEventBus = new EventBus();
        ServiceLoader.AddService<IEventBus>(globalEventBus);

        //AudioSettingsSO audioSettings = Resources.Load<AudioSettingsSO>("AudioSettings");

        Debug.Log("Initialized global services");

    }
}