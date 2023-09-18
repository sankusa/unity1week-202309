using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SankusaLib.LeanLauncher
{
    [Serializable]
    public class AssetData
    {
        [SerializeField] private UnityEngine.Object asset;
        public UnityEngine.Object Asset => asset;
        [SerializeField] private string label = "";
        public string Label => label;

        public AssetData(UnityEngine.Object asset)
        {
            this.asset = asset;
        }
    }

    [CreateAssetMenu(menuName = nameof(LeanLauncherData), fileName = nameof(LeanLauncherData))]
    public class LeanLauncherData : ScriptableObject
    {
        [SerializeField] private List<AssetData> assetDataList;
        public List<AssetData> AssetDataList => assetDataList;

        public void AddAssets(IEnumerable<UnityEngine.Object> assets)
        {
            foreach(UnityEngine.Object asset in assets)
            {
                if(assetDataList.Find(x => x.Asset == asset) != null)
                {
                    Debug.Log(asset.name + " already exists.");
                    continue;
                }
                assetDataList.Add(new AssetData(asset));
            }
        }
    }
}
