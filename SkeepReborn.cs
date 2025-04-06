using Landfall.Modding;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace SkeepReborn
{
    [LandfallPlugin]
    public class SkeepReborn
    {
        static SkeepReborn()
        {
            // Subscribe to the sceneLoaded event.
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("[SkeepReborn] Subscribed to SceneManager.sceneLoaded for early launch.");
        }

        private static void OnSceneLoaded(
            UnityEngine.SceneManagement.Scene scene,
            LoadSceneMode mode
        )
        {
            if (scene.name == "LogoScene2025_April_Intro")
            {
                Debug.Log(
                    $"[SkeepReborn] Logo scene loaded: {scene.name}. Creating early launcher."
                );

                // Unsubscribe so we only react once.
                SceneManager.sceneLoaded -= OnSceneLoaded;

                // Create a temporary GameObject that will run our early launch code.
                GameObject launcherGO = new GameObject("SkeepReborn_EarlyLauncher");
                UnityEngine.Object.DontDestroyOnLoad(launcherGO);
                launcherGO.AddComponent<EarlyLauncher>();
            }
        }

        // This MonoBehaviour is marked with a very early execution order
        // so that its Start method executes before most other Start methods in the scene.
        [DefaultExecutionOrder(-1000)]
        private class EarlyLauncher : MonoBehaviour
        {
            private void Start()
            {
                Debug.Log(
                    "[SkeepReborn] EarlyLauncher Start: attempting to disable intro animations and launch game."
                );

                // ----- Disable the intro animation -----
                // Option A: Disable any Animator you expect to be playing the logo animation.
                // If you know the specific GameObject name, better target it directly.
                Animator animator = FindObjectOfType<Animator>();
                if (animator != null)
                {
                    Debug.Log(
                        $"[SkeepReborn] Disabling Animator on {animator.gameObject.name} to cancel intro animation."
                    );
                    animator.enabled = false;
                }

                // Option B (if using timeline): Stop any PlayableDirector playing an intro timeline.
                PlayableDirector director = FindObjectOfType<PlayableDirector>();
                if (director != null)
                {
                    Debug.Log(
                        $"[SkeepReborn] Stopping PlayableDirector on {director.gameObject.name} to cancel timeline."
                    );
                    director.Stop();
                }
                // ----------------------------------------

                // Now, call PlayFromMenu() before the intro animation gets a chance to run.
                try
                {
                    GameHandler.PlayFromMenu();
                }
                catch (Exception ex)
                {
                    Debug.LogError("[SkeepReborn] Error calling PlayFromMenu: " + ex.ToString());
                }

                // Clean up by destroying the launcher.
                Destroy(gameObject);
            }
        }
    }
}
