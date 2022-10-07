using System;
using UnityEngine;
using UnityEngine.UI;

namespace imady.NebuUI.Samples
{
    [NbuResourcePath("UIViews/")]
    public class SettingView : NbuUIViewBase
    {
        #region Inherited functions from ViewBase


        //public LeeSettingView SetCurrentUser(LeeUser user)
        //{
        //    LeeMainCanvasViewModel.currentUser = user;
        //    RenderView();
        //    return this;
        //}
        #endregion

        public new void Awake()
        {
            //Source = new LeeSettingViewModel();

            BindButtonActions();
            base.Awake();
        }
        public override void Hide()
        {
            //在view被关闭的时候把当前活动的子层级view关闭
            if (currentActivePanel != null)
            {
                currentActivePanel.Hide();
                currentActivePanel = null;
            }

            base.Hide();
        }


        protected override void BindButtonActions()
        {

            base.BindButtonActions();

        }

        #region 开放给UIView内部button控件的接口
        private INbuUIView currentActivePanel; //当前被打开的对象属性面板


        #endregion
    }
}
