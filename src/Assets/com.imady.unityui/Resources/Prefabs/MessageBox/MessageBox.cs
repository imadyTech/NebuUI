using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;

namespace imady.NebuUI.Samples
{
    /// <summary>
    /// 简单的消息提示MessageBox
    /// </summary>
    [NbuResourcePath("MessageBox/")]
    public class MessageBox : NbuUIViewBase, INbuUIView
    {
        public Button OKButton;

        private MdyMessageBoxViewModel _source;
        public override object ViewModel
        {
            get => _source;
            set
            {
                _source = (MdyMessageBoxViewModel)value;
                RenderView();
            }
        }

        protected override void Awake()
        {
            //OK按钮的事件由激活消息盒的程序添加
            //OKButton.onClick.AddListener(base.Hide);
            base.Awake();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override void SetViewModel(object source)
        {
            ViewModel = source as MdyMessageBoxViewModel;
        }
    }



    /// <summary>
    /// 对应于LeeMessageBox的视图模型
    /// </summary>
    [NbuViewModelType(typeof(MessageBox))]
    public class MdyMessageBoxViewModel
    {
        [JsonProperty]
        public string promptTitle { get; set; }

        [JsonProperty]
        public string promptMsg { get; set; }


        public MdyMessageBoxViewModel()
        {
            promptTitle = "title";
            promptMsg = "message";
        }
        public MdyMessageBoxViewModel(string title, string msg)
        {
            promptTitle = title;
            promptMsg = msg;
        }
    }

}