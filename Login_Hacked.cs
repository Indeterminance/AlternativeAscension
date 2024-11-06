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

namespace NeedyMintsOverdose
{
    public class LoginHacked : MonoBehaviour
    {
        public GameObject LoginButtonObj
        {
            get
            {
                return this._login.gameObject;
            }
        }

        private void Start()
        {
            //NeedyMintsMod.log.LogMessage("LoginHacked!");
            _input.interactable = false;
            //NeedyMintsMod.log.LogMessage("LoginHacked2!");
            _passText.gameObject.SetActive(false);
            //NeedyMintsMod.log.LogMessage("LoginHacked!3");
            _placeholderText.text = NgoEx.SystemTextFromType((SystemTextType)(int)ModdedSystemTextType.Login_BadPassword, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
            //NeedyMintsMod.log.LogMessage("LoginHacked4!");
            _placeholderText.gameObject.SetActive(false);
            //NeedyMintsMod.log.LogMessage("LoginHacked5!");
            SteamInput();

            Transform trans = this.transform.parent;
            //for (int i = 0; i < trans.childCount; i++)
            //{
            //    NeedyMintsMod.log.LogMessage($"LoginChild: {trans.GetChild(i)}");
            //}

            _imageContainer.sprite = baseImage;
        }

        private void SteamInput()
        {
            _input.ObserveEveryValueChanged((TMP_InputField input) => input.text, FrameCountType.Update, false).Subscribe(delegate (string text)
            {
                couldLogin(text);
            }).AddTo(gameObject);
            SingletonMonoBehaviour<TooltipManager>.Instance.Hide(false);
            _badge.SetActive(false);
            _login.interactable = false;
            _login.OnClickAsObservable().Subscribe(async delegate (Unit _)
            {
                await SetInvalid();
                attempts++;
                NeedyMintsMod.log.LogMessage($"Attempted to log in {attempts} times");
            }).AddTo(gameObject);
        }

        public async UniTask SetInvalid()
        {
            if (attempts == 0)
            {
                _placeholderText.gameObject.SetActive(true);
                _login.transform.position += new Vector3(0, -0.25f, 0);
            }
            _input.text = string.Empty;
            _imageContainer.sprite = invalidPasswordImage;
            _input.interactable = false;
            LanguageType lang = SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value;

            Func<UniTask> action = loginActions.Dequeue();
            await action();

            if (_input != null) _input.interactable = true;
        }

        private void couldLogin(string text)
        {
            if ((text == "angelkawaii2" || text == "angelikawaii2"))
            {
                this._badge.SetActive(true);
                this._login.interactable = true;
                return;
            }
            this._badge.SetActive(false);
            this._login.interactable = false;
        }

        [SerializeField]
        private int attempts = 0;

        [SerializeField]
        public Button _login;

        // Token: 0x0400232D RID: 9005
        [SerializeField]
        public GameObject _badge;

        // Token: 0x0400232E RID: 9006
        [SerializeField]
        public TMP_InputField _input;

        // Token: 0x0400232F RID: 9007
        [SerializeField]
        public TMP_Text _passText;

        // Token: 0x04002330 RID: 9008
        [SerializeField]
        public TMP_Text _placeholderText;

        [SerializeField]
        public Image _imageContainer;

        [SerializeField]
        public Sprite baseImage;

        [SerializeField]
        public Sprite invalidPasswordImage;

        [SerializeField]
        public Queue<Func<UniTask>> loginActions = new Queue<Func<UniTask>>();
    }
}
