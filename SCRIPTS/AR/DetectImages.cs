using System;
using System.Collections.Generic;
using MENU;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR
{
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class DetectImages : MonoBehaviour
    {
        public event Action<int> NewSignDetected;
        public event Action<string> SignDetected;
        int sovpalo = 0;
        public int _debugSignsDetected = 0; // планировалось, что тут знаки все подряд

        // Reference to AR tracked image manager component
        private ARTrackedImageManager _trackedImagesManager;

        // List of prefabs to instantiate - these should be named the same
        // as their corresponding 2D images in the reference image library 
        public GameObject[] ArPrefabs;

        // Keep dictionary array of created prefabs
        private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();

        //======= мое: СЛОВАРЬ НАЙДЕННЫХ ЗНАКОВ === !!! НАДО СОХРАНЯТЬ !!!! =======================================
        public List<string> MyDetectedSignsList;


        void Awake()
        {
            MyDetectedSignsList = new List<string>();
            //============ TODO подгружаем из сохранения!!! ====================

            // Cache a reference to the Tracked Image Manager component
            _trackedImagesManager = GetComponent<ARTrackedImageManager>();
            GameManager.Instance.MoveMenuButtonToStandard();
        }

        void OnEnable()
        {
            // Attach event handler when tracked images change
            _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        void OnDisable()
        {
            // Remove event handler
            _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }

        // Event Handler
        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            // Loop through all new tracked images that have been detected
            foreach (var trackedImage in eventArgs.added)
            {
                // Get the name of the reference image
                var imageName = trackedImage.referenceImage.name;
                // Now loop over the array of prefabs
                foreach (var curPrefab in ArPrefabs)
                {
                    // Check whether this prefab matches the tracked image name, and that
                    // the prefab hasn't already been created
                    if (string.Compare(curPrefab.name, imageName, StringComparison.OrdinalIgnoreCase) == 0
                        && !_instantiatedPrefabs.ContainsKey(imageName))
                    {
                        // Instantiate the prefab, parenting it to the ARTrackedImage
                        var newPrefab = Instantiate(curPrefab, trackedImage.transform);
                        // Add the created prefab to our array
                        _instantiatedPrefabs[imageName] = newPrefab;

                        // ################# мое ##################################
                        MyFunctionOnDetect(imageName);
                        // ########################################################
                    }
                }
            }

            // For all prefabs that have been created so far, set them active or not depending
            // on whether their corresponding image is currently being tracked
            foreach (var trackedImage in eventArgs.updated)
            {
                _instantiatedPrefabs[trackedImage.referenceImage.name]
                    .SetActive(trackedImage.trackingState == TrackingState.Tracking);


                //---- тут не катит
            }

            // If the AR subsystem has given up looking for a tracked image
            foreach (var trackedImage in eventArgs.removed)
            {
                // Destroy its prefab
                Destroy(_instantiatedPrefabs[trackedImage.referenceImage.name]);
                // Also remove the instance from our array
                _instantiatedPrefabs.Remove(trackedImage.referenceImage.name);
                // Or, simply set the prefab instance to inactive
                //_instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(false);
            }
        }


        
        private void MyFunctionOnDetect(string imageName)
        {
            //------ для дебага-------------------------------------------------
            SignDetected?.Invoke(imageName); // в меню AR
            _debugSignsDetected++;
            GameManager.Instance.PlusSignsDetected(); 

            //====== мое событие + проверка что задетекчен НОВЫЙ знак ===========
            foreach (var signInList in MyDetectedSignsList)
            {
                // ------ если совпало с одним из уже найденных ?
                if (imageName == signInList)
                {
                    sovpalo++;
                }
            }

            if (sovpalo == 0) // ---- не совпало, такой знак еще не определяли ---
            {
                MyDetectedSignsList.Add(imageName);
                int howmuch = MyDetectedSignsList.Count;
                NewSignDetected?.Invoke(howmuch); // идет в GameManager!
                
                GameManager.Instance.GetComponent<AudioManager>().PlaySound("YES");
            }
            //====================================================================

        }
    }
}