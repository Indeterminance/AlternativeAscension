using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace NeedyMintsOverdose
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Animator))]
    public class Viewer : MonoBehaviour
    {
        Animator animator;
        public void Awake()
        {
        }

        public async UniTask idleClosed()
        {

        }



        int closedLoops = UnityEngine.Random.Range(1, 7);
        int openLoops = UnityEngine.Random.Range(4, 12);
        int closedFadeLoops = UnityEngine.Random.Range(1, 4);


        static AnimationClip closingAnim = Addressables.LoadAssetAsync<AnimationClip>("viewer_closing.anim").WaitForCompletion();
        static AnimationClip openingAnim = Addressables.LoadAssetAsync<AnimationClip>("viewer_opening.anim").WaitForCompletion();
        static AnimationClip closedAnim = Addressables.LoadAssetAsync<AnimationClip>("viewer_closed.anim").WaitForCompletion();
        static AnimationClip openAnim = Addressables.LoadAssetAsync<AnimationClip>("viewer_open.anim").WaitForCompletion();
    }
}
