using Cysharp.Threading.Tasks;
using DG.Tweening;
using ngov3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace NeedyMintsOverdose
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimationClip))]
    public class Viewer : MonoBehaviour
    {
        Animator animator;
        Image behindMask;
        public void Awake()
        {
            animator = GetComponent<Animator>();
            behindMask = transform.GetComponent<Image>();
        }

        public void Start()
        {
            Animate();
            SingletonMonoBehaviour<NeedyMintsModManager>.Instance.viewColor.ObserveEveryValueChanged((ReactiveProperty<Color> c) => c.Value, FrameCountType.Update, false).Subscribe(delegate (Color col)
            {
                behindMask.color = col;
            }).AddTo(this);
        }

        public async UniTask Animate()
        {
            Color oldColor = behindMask.color;
            behindMask.color = new Color(1, 1, 1, 0);

            AnimateAlpha(0, 1, 0.33333f);
            await UniTask.Delay(2000 / 6 * closedLoops);
            animator.Play("viewer_opening");
            await UniTask.Delay(100 / 6 * 22);
            await UniTask.Delay(2000 / 6 * openLoops);
            animator.Play("viewer_closing");
            AnimateAlpha(1, 0, 0.66666f);
            await UniTask.Delay(2000 / 3);
            transform.parent.gameObject.SetActive(false);
        }

        public async UniTask AnimateAlpha(float startVal = 0f, float endVal = 1f, float seconds = 1f)
        {
            float value = startVal;
            Color oldColor = behindMask.color;
            DOTween.To(() => startVal, x => {
                behindMask.color = new Color(oldColor.r, oldColor.g, oldColor.b, x);
            }, endVal, seconds).Play();
        }

        int closedLoops = UnityEngine.Random.Range(1, 7);
        int openLoops = UnityEngine.Random.Range(4, 12);


        static AnimationClip closingAnim = Addressables.LoadAssetAsync<AnimationClip>("viewer_closing.anim").WaitForCompletion();
        static AnimationClip openingAnim = Addressables.LoadAssetAsync<AnimationClip>("viewer_opening.anim").WaitForCompletion();
        static AnimationClip closedAnim = Addressables.LoadAssetAsync<AnimationClip>("viewer_closed.anim").WaitForCompletion();
        static AnimationClip openAnim = Addressables.LoadAssetAsync<AnimationClip>("viewer_open.anim").WaitForCompletion();
    }
}
