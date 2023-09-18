using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SankusaLib.SceneManagementLib {
    public interface ISceneArg {}

    public class SceneArgStore
    {
        private readonly Dictionary<Type, ISceneArg> argDic = new Dictionary<Type, ISceneArg>();

        public void Set<T>(T arg) where T : ISceneArg {
            argDic[typeof(T)] = arg;
        }

        public T Get<T>() where T : ISceneArg {
            Type t = typeof(T);

            if(!argDic.ContainsKey(t)) {
                return default(T);
            }

            return (T)argDic[t];
        }

        public T Pop<T>() where T : ISceneArg {
            Type t = typeof(T);

            if(!argDic.ContainsKey(t)) {
                return default(T);
            }

            T ret = (T)argDic[t];
            argDic.Remove(t);
            return ret;
        }

        public bool Exists<T>() where T : ISceneArg{
            return argDic.ContainsKey(typeof(T));
        }
    }
}
