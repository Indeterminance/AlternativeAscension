using Cysharp.Threading.Tasks.CompilerServices;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using ngov3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using NeedyEnums;
using NGO;
using UnityEngine.AddressableAssets;
using System.Threading;
using static AlternativeAscension.AAPatches;

namespace AlternativeAscension
{
    public class ChangePassword : MonoBehaviour
    {
        private void Awake()
        {
            _base = transform.GetChild(0);
            _confirm = _base.Find("ActionButton").GetComponent<Button>();
            _entry1 = _base.Find("InputField (TMP)").GetComponent<TMP_InputField>();
            _entry2 = _base.Find("ConfirmInputField (TMP)").GetComponent<TMP_InputField>();
            _badge = _confirm.transform.Find("badge").gameObject;
        }

        
        private void Start()
        {
            _badge.SetActive(false);
            _confirm.interactable = false;
            _entry1.ObserveEveryValueChanged((TMP_InputField input) => input.text, FrameCountType.Update, false).Subscribe(delegate (string text)
            {
                CheckValidPassword(text);
            }).AddTo(gameObject);
            _entry2.ObserveEveryValueChanged((TMP_InputField input) => input.text, FrameCountType.Update, false).Subscribe(delegate (string text)
            {
                CheckValidPassword(text);
            }).AddTo(gameObject);
            _confirm.OnClickAsObservable().Subscribe(delegate (Unit _)
            {
                SingletonMonoBehaviour<AltAscModManager>.Instance.password = _entry1.text;
                SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(ModdedAppType.ChangePassword.Swap());
            }).AddTo(gameObject);

        }

        private void CheckValidPassword(string password)
        {
            if (password.Length >= 6
                && password != "angelkawaii2" && password != "angelikawaii2"
                && _entry1.text == _entry2.text)
            {
                _badge.SetActive(true);
                _confirm.interactable = true;
            }
            else
            {
                _badge.SetActive(false);
                _confirm.interactable = false;
            }
        }

        Transform _base;
        GameObject _badge;
        TMP_InputField _entry1;
        TMP_InputField _entry2;
        Button _confirm;
    }
}
