using UnityEngine;
using UniRx;

namespace SankusaLib
{
    [ExecuteAlways]
    public class ScaleOverWriter : MonoBehaviour
    {
        [SerializeField] private Vector3 fixedScale = Vector3.one;
        private Subject<Vector3> onFixedScaleChanged = new Subject<Vector3>();
        public Vector3 FixedScale {
            get => fixedScale;
            set {
                fixedScale = value;
                onFixedScaleChanged.OnNext(fixedScale);
            }
        }
        [SerializeField] private Vector3 adjustScale = Vector3.one;
        [SerializeField] private bool fixOnUpdate;
        [SerializeField] private bool fixOnEditor;

        void Awake()
        {
            onFixedScaleChanged.Subscribe(_ => FixScale()).AddTo(this);
        }

        void Update()
        {
            if(fixOnUpdate)
            {
#if UNITY_EDITOR
                if(Application.isPlaying == false)
                {
                    if(fixOnEditor)
                    {
                        FixScale();
                    }
                }
                else
                {
                    if(fixOnUpdate)
                    {
                        FixScale();
                    }
                }
#else
                if(fixOnUpdate)
                {
                    FixScale();
                }
#endif
            }
        }

        public void FixScale()
        {
            Vector3 localScale = transform.localScale;
            Vector3 lossyScale = transform.lossyScale;
            transform.localScale =  new Vector3(lossyScale.x == 0 ? 0 : localScale.x / lossyScale.x * fixedScale.x * adjustScale.x,
                                                lossyScale.y == 0 ? 0 : localScale.y / lossyScale.y * fixedScale.y * adjustScale.y,
                                                lossyScale.z == 0 ? 0 : localScale.z / lossyScale.z * fixedScale.z * adjustScale.z);
        }
    }
}