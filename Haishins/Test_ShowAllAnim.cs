using Cysharp.Threading.Tasks;
using DG.Tweening;
using NeedyEnums;
using NGO;
using ngov3;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static AlternativeAscension.AAPatches;

namespace AlternativeAscension
{
    public class Test_ShowAllAnim : LiveScenario
    {
        bool dark;
        // Token: 0x06000FD5 RID: 4053 RVA: 0x00049280 File Offset: 0x00047480
        public override void Awake()
        {
            base.Awake();
            this.title = "Animations";
            _Live.Speed = 0;

            List<string> ids = new List<string>();

            foreach (var loc in Addressables.ResourceLocators)
            {
                foreach (var key in loc.Keys)
                {
                    if (!loc.Locate(key, typeof(object), out var resourceLocations) || key.ToString().Contains("bundle"))
                        continue;

                    Regex choPattern = new Regex("stream_cho.*?\\.anim");
                    Regex amePattern = new Regex("stream_ame.*?\\.anim");
                    string id = string.Empty;
                    if (choPattern.IsMatch(key.ToString()))
                    {
                        //id = choPattern.Match(key.ToString()).Value.Replace(".anim", "");
                    }
                    else if (amePattern.IsMatch(key.ToString()))
                    {
                        id = amePattern.Match(key.ToString()).Value.Replace(".anim", "");
                    }
                    if (id.IsNotEmpty())
                    {
                        ids.Add(id);
                        //AltAscMod.log.LogMessage($"Stream ID : {id} > {key}");
                    }
                }
            }

            ids = ids.Distinct().ToList();
            playing.AddRange(ids.Select(id => new Playing(true, id, animation: id)));
            //AltAscMod.log.LogMessage($"Playing {playing.Count} anims...");
        }

        // Token: 0x06000FD6 RID: 4054 RVA: 0x0004A57C File Offset: 0x0004877C
        public override async UniTask StartScenario()
        {
            await base.StartScenario();
        }
    }
}
