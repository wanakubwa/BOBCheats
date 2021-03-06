﻿using BOBCheats.Collections;
using BOBCheats.GUI;
using System.Collections.Generic;
using UnityEngine;

namespace BOBCheats
{
    public class BOBCheatsManager : MonoBehaviour
    {
        #region Fields

        private static BOBCheatsManager instance;

        [Space]
        [SerializeField]
        private CheatsMenuController cheatMenuGUIPrefab;

        #endregion

        #region Propeties

        public static BOBCheatsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (BOBCheatsManager)FindObjectOfType(typeof(BOBCheatsManager));
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public CheatsMenuController CheatMenuGUIPrefab { get => cheatMenuGUIPrefab; set => cheatMenuGUIPrefab = value; }

        public List<CheatCategory> CategoriesCollection {
            get;
            private set;
        } = new List<CheatCategory>();

        private CheatsMenuController CurrentCheatGUI
        {
            get;
            set;
        }

        private KeyCode TriggerKey
        {
            get;
            set;
        }

        private bool OneClick { get; set; } = false;
        private float TimerForDoubleClick { get; set; }
        private float DoubleClickDelay { get; set; } = 0.2f;

        #endregion


        #region Methods

        /// <summary>
        /// Use it for initialize BOB cheats, if auto initialize is enabled.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInitialize()
        {
            // BOB is already initiaized on scene.
            if(Instance != null)
            {
                return;
            }

            if(BOBCheatsSettings.IsAutoInitialize == true)
            {
                GameObject go = new GameObject("BOBCheatManager");
                BOBCheatsManager bob = go.AddComponent<BOBCheatsManager>();

                GameObject cheatsMenuObj = Resources.Load("GUI/BOBCheatsGUI") as GameObject;
                bob.CheatMenuGUIPrefab = cheatsMenuObj.GetComponent<CheatsMenuController>();

                GameObject.DontDestroyOnLoad(go);

                Debug.Log("[BOBCheats] Auto initialized!");
            }
        }

        public void ToggleCheatMenuGUI()
        {
            if (CurrentCheatGUI != null)
            {
                HideGUI();
            }
            else
            {
                ShowGUI();
            }
        }

        public void ShowGUI()
        {
            CheatsMenuController cheatMenuGUI = Instantiate(CheatMenuGUIPrefab);
            cheatMenuGUI.transform.localScale = Vector3.one;
            CurrentCheatGUI = cheatMenuGUI;
        }

        public void HideGUI()
        {
            if (CurrentCheatGUI != null)
            {
                CurrentCheatGUI.DestroyGUIWindow();
                CurrentCheatGUI = null;
            }
        }

        public void UseCheat(CheatInfo cheat, object[] parameters)
        {
            if (cheat == null)
            {
                Debug.Log("Cheat was null! Can't use it!");
                return;
            }

            Debug.LogFormat("[BOBCheat] Activate cheat name: {0}", cheat.CheatName);
            cheat.CachedInfo.Invoke(null, parameters);
        }

        private void Awake()
        {
            DontDestroyCheck();

            BOBCheatsSettings cheatsSettings = BOBCheatsSettings.Instance;
            if (cheatsSettings == null)
            {
                Debug.Log("[BOBCheat] Scriptable object BOBCheatsSettings not instanced! Can't load cheats!");
                return;
            }

            cheatsSettings.RefreshCheatsCollection();

            TriggerKey = cheatsSettings.TriggerKey;
            CategoriesCollection = cheatsSettings.CheatsCategories;
        }

        private void DontDestroyCheck()
        {
            BOBCheatsManager[] objs = FindObjectsOfType<BOBCheatsManager>();

            if (objs.Length > 1)
            {
                gameObject.SetActive(false);
                Destroy(this.gameObject);
                return;
            }

            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(TriggerKey) == true)
            {
                ToggleCheatMenuGUI();
            }

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR

            if (Input.GetMouseButtonDown(0))
            {
                if (OneClick == false)
                {
                    OneClick = true;

                    TimerForDoubleClick = Time.time;
                }
                else
                {
                    OneClick = false;
                    ToggleCheatMenuGUI();
                }
            }
            if (OneClick)
            {
                if ((Time.time - TimerForDoubleClick) > DoubleClickDelay)
                {
                    OneClick = false;
                }
            }
#endif

#endregion

#region Enums



#endregion
        }
    }
}
