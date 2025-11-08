using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Menu
{
    /// <summary>
    ///     A simple scene loader that transitions between screens.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [Header("References")] public Animator animator;

        [Header("Parameters")] public bool showCursor;

        [SerializeField] private List<string> _sceneNames;

        private int _inputSceneIndex;

        [SerializeField] private List<string> _inputScenes;

#if UNITY_EDITOR
        private void RefreshScenes()
        {
            string[] searchPaths = { "Assets/Scenes/Menu", "Assets/Scenes/Game" };
            _sceneNames = AssetDatabase.FindAssets($"t:scene", searchPaths)
                .Select(guid => Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guid)))
                .ToList();
            
            _inputScenes = AssetDatabase.FindAssets($"t:scene", new[] { "Assets/Scenes/Menu/Input" })
                .Select(guid => Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guid)))
                .ToList();
            
            _inputSceneIndex = _inputScenes.IndexOf(SceneManager.GetActiveScene().name);
            
            EditorUtility.SetDirty(this);
        }

        private void OnValidate()
        {
            RefreshScenes();
        }
#endif

        private void OnEnable()
        {
            if (animator)
            {
                animator.gameObject.SetActive(true);
                animator.SetBool("Active", false);
            }

            if (showCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void QuitApplication()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void NextScene()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            SwitchScene(_sceneNames[(_sceneNames.IndexOf(currentScene) + 1) % _sceneNames.Count]);
        }

        public void PreviousScene()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            SwitchScene(_sceneNames[(_sceneNames.IndexOf(currentScene) - 1 + _sceneNames.Count) % _sceneNames.Count]);
        }

        public void SwitchScene(string sceneName)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            StartCoroutine(C_SwitchScene(sceneName));
        }

        private IEnumerator C_SwitchScene(string sceneName)
        {
            if (animator)
            {
                animator.gameObject.SetActive(true);
                animator.SetBool("Active", true);
                yield return new WaitForSeconds(0.5f);
            }

#if UNITY_EDITOR
            string path = sceneName;
            if (path.IndexOf('/') == -1)
            {
                string guid = AssetDatabase.FindAssets($"{sceneName} t:scene")[0];
                path = AssetDatabase.GUIDToAssetPath(guid);
            }

            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(path,
                new LoadSceneParameters(LoadSceneMode.Single));
#else
            // make sure the scene is in the build settings for runtime version
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
#endif
        }

        // Switch between scenes describing inputs, for demo purposes
        public void NextSwitchInputScene()
        {
            if (_inputScenes == null || _inputScenes.Count == 0)
            {
                Debug.LogWarning("No input scenes available to switch to.");
                return;
            }
            
            _inputSceneIndex = (_inputSceneIndex + 1) % _inputScenes.Count;
            SwitchScene(_inputScenes[_inputSceneIndex]);
        }

        public void PreviousSwitchInputScene()
        {
            if (_inputScenes == null || _inputScenes.Count == 0)
            {
                Debug.LogWarning("No input scenes available to switch to.");
                return;
            }
            
            _inputSceneIndex = (_inputSceneIndex - 1 + _inputScenes.Count) % _inputScenes.Count;
            SwitchScene(_inputScenes[_inputSceneIndex]);
        }
    }
}
