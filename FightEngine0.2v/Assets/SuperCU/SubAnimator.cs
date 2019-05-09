using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;


namespace SuperCU.Generic
{
    public static class SubAnimator
    {
        /// <summary>
        /// Animatorが所持しているAnimationClipをすべて取得する
        /// </summary>
        /// <param name="animator">取得するAnimator</param>
        /// <returns>AnimationClipのリスト</returns>
        public static List<AnimationClip> GetAnimatorAndClips(Animator animator)
        {
            List<AnimationClip> clipList = new List<AnimationClip>();
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                clipList.Add(clip);
            }
            return clipList;
        }

        /// <summary>
        /// AnimatorControllerを作成する
        /// 以降のメソッドを使用する際に必須なので、必ず呼び出してください
        /// </summary>
        /// <param name="path">操作するオブジェクトの名前の.controllerファイルのパス</param>
        /// <returns>作成したAnimatorController</returns>
        public static AnimatorController createAnimatorController(string path)
        {
            return AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
        }

        //以下animatorをいじる系のメソッド

        /// <summary>
        /// AnimationClipを取得し、stateに変化させAnimatorに入れる。transitionも同時に設定する。
        /// </summary>
        /// <param name="animatorController">AnimationClipを入れる先のAnimatorController</param>
        /// <param name="clip">Animatorに入れるAnimationClip</param>
        /// <param name="position">AnimationClipを置くAnimatorの座標</param>
        /// <param name="layer">Animatorを入れるレイヤー</param>
        public static void AddClip(AnimatorController animatorController, AnimationClip clip, Vector2 position,int layer=0,AnimatorStateTransition[] animatorStateTransition=null)
        {
            if(animatorController==null)
            {
                Debug.Log("animatorController is null");
                return;
            }
            //animationClipをstateMachineに渡して、state作成
            var stateMachine = animatorController.layers[layer].stateMachine;
            var newState = stateMachine.AddState(clip.name,position);
            newState.motion = clip;
            if(animatorStateTransition!=null)
            {
                foreach(AnimatorStateTransition transition in animatorStateTransition)
                {
                    newState.AddTransition(transition);
                }
            }
        }

        //AnimatorContollerParameterを追加する
        public static void AddParameter(AnimatorController animatorController,AnimatorControllerParameter[] parameter)
        {
            foreach(AnimatorControllerParameter p in parameter)
            {
                animatorController.AddParameter(p);
            }
        }
    }
}
