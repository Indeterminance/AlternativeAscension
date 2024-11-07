using Cysharp.Threading.Tasks;
using NGO;
using ngov3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NeedyXML;
using NeedyEnums;
using System.Reflection;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using HarmonyLib;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using UnityEngine.AddressableAssets;
using System.Threading;

namespace AlternativeAscension
{
    public class AltAscModManager : SingletonMonoBehaviour<AltAscModManager>
    {
        public override void Awake()
        {
            base.Awake();
            AltAscMod.log.LogMessage("Awoken manager!");
            InitViewers();
            InitEffects();
            isFollowerBG.Where((bool v) => v).Subscribe(delegate (bool _)
            {
                this.ChangeBG();
            }).AddTo(base.gameObject);
            viewing.Where((bool v) => v).Subscribe(delegate (bool _)
            {
                this.SpawnLoop();
            }).AddTo(base.gameObject);
        }

        public void Start()
        {
            _windowParent = new Traverse(SingletonMonoBehaviour<WindowManager>.Instance).Field(nameof(WindowManager._windowparent)).GetValue<Transform>();
        }

        public void InitViewers()
        {
            Viewer = Addressables.LoadAssetAsync<GameObject>("ViewerContainer.prefab").WaitForCompletion();
            Viewer.transform.GetChild(0).gameObject.AddComponent<Viewer>();
        }

        public void InitEffects()
        {
            loveEffect = UnityEngine.Object.Instantiate(Addressables.LoadAssetAsync<GameObject>("LoveEffect.prefab").WaitForCompletion(),transform);

        }

        // AMA stuff
        public int QUESTIONS = 20;
        public int QUESTIONS_ASKED = 0;
        public int PlannedAMALength = 120000;
        public List<Playing> playedQuestions = new List<Playing>();
        public bool deleteTime = false;
        public LiveComment deleteComment = null;
        public bool isAMA = false;
        public bool wasUncontrollable = false;
        public int oldSpeed = 1;
        public int delta;
        public int StressDelta = 0;
        public int overnightStreamStartDay = 0;
        public bool lockDayCount = false;

        public GameObject Viewer;
        public GameObject loveEffect;
        protected Transform _windowParent;

        public ReactiveProperty<bool> isAmeDelete = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> isFollowerBG = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> viewing = new ReactiveProperty<bool>(false);
        public ReactiveProperty<int> viewInterval = new ReactiveProperty<int>(160);
        public ReactiveProperty<Color> viewColor = new ReactiveProperty<Color>(Color.red);

        public bool isLove;
        public bool isLoveLoop;
        public int sleepCount;

        public Sprite defaultBG;
        public Sprite followerBG = Addressables.LoadAssetAsync<Sprite>("eyes_bg.png").WaitForCompletion();

        public Sprite followerEndBG = Addressables.LoadAssetAsync<Sprite>("stream_ame_follower_end_bg.png").WaitForCompletion();
        public Sprite followerDarkEndBG = Addressables.LoadAssetAsync<Sprite>("stream_ame_follower_end_dark_bg.png").WaitForCompletion();

        public TweenerCore<float, float, FloatOptions> anim;
        public List<IWindow> pills = new List<IWindow>();

        public int GetAMAStressDelta(string question)
        {
            //NeedyMintsMod.log.LogMessage($"Getting delta for question \"{question}\"");
            int delta = 0;
            switch (question)
            {
                case "AMA_Q001": delta = 3;
                    break;
                case "AMA_Q006": delta = 10;
                    break;
                case "AMA_Q007": delta = 1;
                    break;
                case "AMA_Q014": delta = 8;
                    break;
                case "AMA_Q019": delta = 4;
                    break;
                case "AMA_Q020":
                    delta = -99;
                    goto default;
                case "AMA_Q023": delta = 1;
                    break;
                case "AMA_Q027": delta = 10;
                    break;
                case "AMA_Q031": delta = 8;
                    break;
                default: break;
            }
            return delta;
        }

        public List<Playing> GetAMAEnd()
        {
            string prefix = ModdedAlphaType.FollowerAlpha.ToString() + "2_AMAFINISH_";
            string mid;
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.AMAStress.Swap(), StressDelta);
            if (StressDelta >= 15)
            {
                mid = "BAD";
            }
            else if (playedQuestions.Count < 6)
            {
                mid = "IGNORE";
            }
            else
            {
                mid = "GOOD";
            }

            LanguageType lang = SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value;
            List<Playing> list = new List<Playing>()
            {
                new Playing(true,NgoEx.TenTalk(prefix + mid + "001",lang),StatusType.Tension,1,0,"","","",true,SuperchatType.White,false,""),
                new Playing(true,NgoEx.TenTalk(prefix + mid + "002",lang),StatusType.Tension,1,0,"","","",true,SuperchatType.White,false,""),
                new Playing(true,NgoEx.TenTalk(prefix + mid + "003",lang),StatusType.Tension,1,0,"","","",true,SuperchatType.White,false,""),
            };

            if (mid == "BAD" || mid == "IGNORE")
            {
                list.Insert(1, new Playing(false, NgoEx.Kome(prefix + mid, lang)));
            }
            return list;
        }

        public void StartAMA(ref LiveScenario scenario)
        {
            isAMA = true;
            playedQuestions.Clear();

            Live live = new Traverse(scenario).Field(nameof(LiveScenario._Live)).GetValue() as Live;

            //Live live = UnityEngine.Object.FindObjectOfType<Live>();
            wasUncontrollable = live.isUncontrollable;
            live.isUncontrollable = true;
            //NeedyMintsMod.log.LogMessage($"Live was {live}");

            // Change comment label
            LanguageType lang = SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value;
            SystemTextType type = (SystemTextType)(int)ModdedSystemTextType.System_AMARead;
            string text = NgoEx.SystemTextFromType(type, lang);
            //NeedyMintsMod.log.LogMessage($"AMA systext {text}!");
            live.CommentLabel.text = text;
        }

        public void FinishAMA(ref LiveScenario scenario)
        {
            if (!isAMA) return;
            isAMA = false;
            Live live = new Traverse(scenario).Field(nameof(LiveScenario._Live)).GetValue() as Live;
            live.isUncontrollable = wasUncontrollable;
            //NeedyMintsMod.log.LogMessage($"AMA ended with {playedQuestions.Count} questions asked and {StressDelta} AMA stress accumulated!");

            List<Playing> amaEndText = GetAMAEnd();
            scenario.playing.InsertRange(1, amaEndText);


            //NeedyMintsMod.log.LogMessage($"FinishAMA finishing with {live}...");

            // Change comment label
            LanguageType lang = SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value;
            SystemTextType type = SystemTextType.Sysyem_NotReadComment;
            string text = NgoEx.SystemTextFromType(type, lang);
            //NeedyMintsMod.log.LogMessage($"AMA systext {text}!");
            live.CommentLabel.text = text;

            AltAscMod.log.LogMessage("FinishAMA finished");
            
        }

        public void ChangeBG()
        {
            GameObject mainPanel = GameObject.Find("MainPanel");
            Sprite sprite = mainPanel.GetComponent<Image>().sprite;
            if (sprite != followerBG) defaultBG = sprite;
            mainPanel.GetComponent<Image>().sprite = followerBG;
        }

        public void CreateViewer()
        {
            RectTransform mainPanel = GameObject.Find("MainPanel").transform as RectTransform;
            float maxX = mainPanel.rect.width / mainPanel.localScale.x;
            float maxY = mainPanel.rect.height / mainPanel.localScale.y;
            float num = UnityEngine.Random.Range(0, maxX);
            float num2 = UnityEngine.Random.Range(0, maxY);

            GameObject obj = null;
            if (transform.childCount < 300)
            {
                obj = UnityEngine.Object.Instantiate(Viewer, new Vector2(num, num2), Quaternion.identity, transform);
            }
            else
            {
                bool foundObject = false;
                for (int i = 0; i < transform.childCount; i++)
                {
                    obj = transform.GetChild(i).gameObject;
                    if (!obj.activeSelf)
                    {
                        foundObject = true;
                        break;
                    }
                }
                if (!foundObject) return;
            }
            obj.transform.GetChild(0).position = new Vector2(num, num2);

        }

        public async UniTask SetViewersInactive()
        {
            viewing.Value = false;
            await UniTask.Delay(viewInterval.Value);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }


        public async UniTask SpawnLoop()
        {
            while (viewing.Value)
            {
                await UniTask.Delay(viewInterval.Value);
                CreateViewer();
            }
        }
    }
}
