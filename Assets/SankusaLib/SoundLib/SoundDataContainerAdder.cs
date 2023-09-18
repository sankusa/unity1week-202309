using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.SoundLib {
    public class SoundDataContainerAdder : MonoBehaviour
    {
        [SerializeField] private List<SoundDataContainer> containers;
        void Awake() {
            SoundDataMaster.Instance.Add(containers);
        }

        void OnDestroy() {
            SoundDataMaster.Instance.Remove(containers);
        }
    }
}