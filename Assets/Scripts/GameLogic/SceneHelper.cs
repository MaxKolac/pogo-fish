using System;
using UnityEngine;
using UnityEngine.SceneManagement;

internal static class SceneHelper
{
    //Code copied from here:
    //https://gist.github.com/kurtdekker/862da3bc22ee13aff61a7606ece6fdd3
    /// <summary>
    /// Loads a new Scene and waits 1 frame to let it load.
    /// </summary>
    /// <param name="s">The name of the Scene for SceneManager to look for.</param>
    /// <param name="additive">The scene loading mode, the default is Singular.</param>
    /// <param name="setActive">Whether or not to set the newly loaded scene as Active.</param>
    public static void LoadScene(string s, bool additive = false, bool setActive = false)
    {
        s ??= SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(
            s, additive ? LoadSceneMode.Additive : 0);

        if (setActive)
        {
            CallAfterDelay.Create(0, () =>
            {
                SceneManager.SetActiveScene(
                SceneManager.GetSceneByName(s));
            });
        }
    }

    private class CallAfterDelay : MonoBehaviour
    {
        float delay;
        float age;
        Action action;

        // Will never call this frame, always the next frame at the earliest
        public static CallAfterDelay Create(float delay, Action action)
        {
            CallAfterDelay cad = new GameObject("CallAfterDelay").AddComponent<CallAfterDelay>();
            cad.delay = delay;
            cad.action = action;
            return cad;
        }

        void Update()
        {
            if (age > delay)
            {
                action();
                Destroy(gameObject);
            }
        }
        void LateUpdate()
        {
            age += Time.deltaTime;
        }
    }
}