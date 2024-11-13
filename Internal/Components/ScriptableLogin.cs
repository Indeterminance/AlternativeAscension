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
    public class ScriptableLogin : MonoBehaviour
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
            // Set login button positions
            initLoginButtonPos = _login.transform.position;
            invalidLoginButtonPos = initLoginButtonPos + new Vector3(0, -0.25f, 0);

            // Set initial values
            _input.interactable = false;
            _passText.gameObject.SetActive(false);
            _placeholderText.text = NgoEx.SystemTextFromType((SystemTextType)(int)ModdedSystemTextType.Login_BadPassword, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
            _placeholderText.gameObject.SetActive(false);
            SteamInput();

            // Set observables
            isBadLogin.Where((bool b) => b).Subscribe(delegate (bool bad)
            {
                AltAscMod.log.LogMessage("Observed change to isBadLogin");
                SetLoginState(bad, isInvalidLogin.Value);
            }).AddTo(gameObject);
            isInvalidLogin.Where((bool b) => b).Subscribe(delegate (bool invalid)
            {
                AltAscMod.log.LogMessage("Observed change to isInvalidLogin");
                SetLoginState(isBadLogin.Value, invalid);
            }).AddTo(gameObject);


            // Initialize login state
            SetLoginState(isBadLogin.Value, isInvalidLogin.Value);
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
                await ProcessLoginAttempt();
                attempts++;
                //AltAscMod.log.LogMessage($"Attempted to log in {attempts} times");
            }).AddTo(gameObject);
        }

        public async UniTask ProcessLoginAttempt()
        {
            _input.text = string.Empty;
            _input.interactable = false;

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

        private void SetLoginState(bool isBad, bool isInvalid)
        {
            if (isInvalid && isBad) _imageContainer.sprite = invalidPasswordImageBad;
            else if (isInvalid && !isBad) _imageContainer.sprite = invalidPasswordImage;
            else if (!isInvalid && isBad) _imageContainer.sprite = baseImageBad;
            else if (!isInvalid && !isBad) _imageContainer.sprite = baseImage;

            _placeholderText.gameObject.SetActive(isInvalid);

            if (isInvalid)
            {
                _login.transform.position = invalidLoginButtonPos;
            }
            else _login.transform.position = initLoginButtonPos;
        }

        [SerializeField]
        private int attempts = 0;

        [SerializeField]
        public Button _login;

        [SerializeField]
        public GameObject _badge;

        [SerializeField]
        public TMP_InputField _input;

        [SerializeField]
        public TMP_Text _passText;

        [SerializeField]
        public TMP_Text _placeholderText;

        [SerializeField]
        public Image _imageContainer;

        [SerializeField]
        public Sprite baseImage;

        [SerializeField]
        public Sprite baseImageBad;

        [SerializeField]
        public Sprite invalidPasswordImage;

        [SerializeField]
        public Sprite invalidPasswordImageBad;

        [SerializeField]
        public ReactiveProperty<bool> isBadLogin = new ReactiveProperty<bool>(false);

        [SerializeField]
        public ReactiveProperty<bool> isInvalidLogin = new ReactiveProperty<bool>(false);

        [SerializeField]
        public Queue<Func<UniTask>> loginActions = new Queue<Func<UniTask>>();

        public Vector3 initLoginButtonPos;

        public Vector3 invalidLoginButtonPos;
    }
}
