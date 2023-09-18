using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlurrySDK;

public class FlurryManager : MonoBehaviour
{
#if UNITYANDROID
    private readonly string FLURRY_API_KEY = HBGDY7NP45TNHT2C3WWQ;
#elif UNITYIPHONE
    private readonly string FLURRY_API_KEY = FLURRY_IOS_API_KEY;
#else
    private readonly string FLURRY_API_KEY = null;
#endif

    void Start()
    {
        new Flurry.Builder()
            .WithCrashReporting(true)
            .WithLogEnabled(true)
            .WithLogLevel(Flurry.LogLevel.DEBUG)
            .WithMessaging(true)
            .Build(FLURRY_API_KEY);
    }

}
