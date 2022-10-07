using UnityEngine;
using System.Collections;
using System;

namespace imady.NebuUI.Samples
{
    /// <summary>
    /// 顶部消息提示（当前场景）
    /// </summary>
    [NbuResourcePath("MessageBox/")]
    public class PromptTitleBox : NbuUIViewBase, INbuUIView
    {
        public override void SetViewModel(object source)
        {
            base.SetViewModel(source);
            Timing(3);//默认3秒关闭自己
        }


        public PromptTitleBox Timing(float seconds)
        {
            StartCoroutine(SetTimer(seconds));
            return this;
            
            IEnumerator SetTimer(float seconds)
            {
                yield return new WaitForSeconds(seconds);
                this.Hide();
            }
        }

    }
}