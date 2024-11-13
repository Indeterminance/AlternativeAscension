using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;
using NGO;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AlternativeAscension
{
    public class Event_Mistweet : NgoEvent
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            base.startEvent(cancellationToken);
            await UniTask.Delay(9000);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine, true);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Jine);
            PostEffectManager.Instance.SetShader(EffectType.GoCrazy);
            PostEffectManager.Instance.SetShaderWeight(2);
            SingletonMonoBehaviour<WebCamManager>.Instance.SetBaseAnim("stream_ame_craziness");
            await UniTask.Delay(Constants.MIDDLE, false, PlayerLoopTiming.Update, default, false);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE003.Swap());
            await UniTask.Delay(Constants.MIDDLE);
            DeleteTweet();
            SingletonMonoBehaviour<WebCamManager>.Instance.SetBaseAnim("stream_ame_talk_c");
            await UniTask.Delay(Constants.MIDDLE, false, PlayerLoopTiming.Update, default, false);

            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE009.Swap());
            base.endEvent();
        }

        public void DeleteTweet()
        {
            PoketterManager pm = SingletonMonoBehaviour<PoketterManager>.Instance;
            pm.history.RemoveAt(pm.history.Count - 1);

            //NeedyMintsMod.log.LogMessage($"Waku: {SingletonMonoBehaviour<PoketterView>.Instance._Waku}");

            //Transform trans = rt.transform.GetChild(1);
            //NeedyMintsMod.log.LogMessage(trans.name);
            //GameObject.Destroy(trans);
            return;
        }
    }
}
