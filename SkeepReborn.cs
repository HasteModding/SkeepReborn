using Landfall.Modding;
using UnityEngine;
using UnityEngine.SceneManagement; // Required

namespace SkipStartup; // Renamed for clarity

[LandfallPlugin]
public class SkipStartupPlugin
{
    private static bool _sceneLoadInitiated = false; // Flag to ensure it only runs once

    static SkipStartupPlugin()
    {
        // This is the earliest point our code runs.
        if (_sceneLoadInitiated)
            return; // Should be unnecessary in static ctor, but safe practice
        _sceneLoadInitiated = true;

        Debug.Log(
            "SkipStartupMod: Attempting to load 'FullHub' directly from static constructor..."
        );

        try
        {
            // Attempt to load the scene immediately.
            SceneManager.LoadScene("FullHub");
            Debug.Log(
                "SkipStartupMod: LoadScene('FullHub') called successfully from static constructor."
            );
        }
        catch (System.Exception e)
        {
            // Log any errors if SceneManager isn't ready or something else fails.
            Debug.LogError(
                $"SkipStartupMod: FAILED to load 'FullHub' from static constructor: {e}"
            );
            // If it fails here, the game will likely continue its normal startup (loading splash/menu)
            // unless another hook intercepts later.
            _sceneLoadInitiated = false; // Reset flag if failed, allowing other potential hooks (if any)
        }
    }

    // No other hooks needed if the static constructor works.
    // If it FAILS, you'd need a fallback hook like the previous examples
    // (e.g., On.MainMenuMainPage.Start or On.SomeEarlyManager.Awake)
}
