using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using imady.NebuEvent;

namespace imady.NebuUI.Samples
{
    [NbuResourcePath("UIViews/")]
    public class TopButtonGroupView : NbuUIViewBase,
        INebuProvider<NebuUnityUIMessage<NebulogServerInitiateMsg>>,
        INebuProvider<NebuUnityUIMessage<Mdy2DViewMsg>>, INebuProvider<NebuUnityUIMessage<Mdy3DViewMsg>>,
        INebuProvider<NebuUnityUIMessage<NebuUnityButtonInput>>

    {
        public new void Awake()
        {
            BindButtonActions();
            base.Awake();
        }

        private string displayMode = "2D";


        #region ---------- UIView包含的Button对象的引用声明 -- 请在unity editor中进行预赋值 ------------
        //---------- PlayMode Setting Button Group 场景模式---------//
        public Button Toggle_Start_Button;//Start nebulogServer process

        //---------- 3D/2D Button Group ---------//
        public Button Toggle_3D2D_Button;//3D-2D视图切换

        //---------- Data Play Button Group 数据控制---------//
        public Button Toggle_SpeedDown_Button;
        public Button Toggle_Play_Button;
        public Button Toggle_Pause_Button;
        public Button Toggle_SpeedUp_Button;

        //---------- Setting Button Group 设置---------//
        public Button Toggle_Setting_Button;//打开设置面板
        #endregion


        /// <summary>
        /// 绑定按钮对象实例与对应的动作
        /// </summary>
        protected override void BindButtonActions()
        {
            Toggle_Start_Button.onClick.AddListener(() =>
            {
                base.NotifyObservers(new NebuUnityUIMessage<NebulogServerInitiateMsg>());
            });

            //---------- Data Play Button Group---------//
            Toggle_SpeedDown_Button.onClick.AddListener(this.OnToggle_SpeedDown_Clicked);//减速
            Toggle_Play_Button.onClick.AddListener(this.OnToggle_Play_Clicked);//播放
            Toggle_Pause_Button.onClick.AddListener(this.OnToggle_Pause_Clicked);//暂停
            Toggle_SpeedUp_Button.onClick.AddListener(this.OnToggle_SpeedUp_Clicked);//加速
            Toggle_3D2D_Button.onClick.AddListener(this.OnToggle_3D_2D_Button_Clicked);
            //----------Setting Button Group 设置---------//
            Toggle_Setting_Button.onClick.AddListener(this.OnToggle_Setting_Button_Clicked);
        }


        #region ----------///3D_2D///---------
        public void OnToggle_3D_2D_Button_Clicked()
        {
            switch (displayMode)
            {
                case "3D":
                    {
                        displayMode = "2D";
                        NotifyObservers(new NebuUnityUIMessage<Mdy2DViewMsg>());
                        break;
                    }
                case "2D":
                    {
                        displayMode = "3D";
                        NotifyObservers(new NebuUnityUIMessage<Mdy3DViewMsg>());
                        break;
                    }
            }
            Debug.Log("OnToggle_3D_2D_Button_Clicked: " + displayMode);
        }

        #endregion


        #region ---------- Play Button Group ---------
        public Text promptLabel;

        private void OnToggle_SpeedUp_Clicked()
        {

            var message = new NebuUnityUIMessage<NebuUnityButtonInput>()
            {
                timeSend = DateTime.Now,
                messageBody = new NebuUnityButtonInput() { msg = "UIButton_SpeedUp", actionItem = promptLabel }
            };
            base.NotifyObservers(message);
            Debug.Log("OnToggle_SpeedUp_Clicked");
        }

        private void OnToggle_Pause_Clicked()
        {

            var message = new NebuUnityUIMessage<NebuUnityButtonInput>()
            {
                timeSend = DateTime.Now,
                messageBody = new NebuUnityButtonInput() { msg = "UIButton_Pause", actionItem = promptLabel }
            };
            base.NotifyObservers(message);
            Debug.Log("OnToggle_Pause_Clicked");
        }

        private void OnToggle_Play_Clicked()
        {
            var message = new NebuUnityUIMessage<NebuUnityButtonInput>()
            {
                timeSend = DateTime.Now,
                messageBody = new NebuUnityButtonInput() { msg = "UIButton_Play", actionItem = promptLabel }
            };
            base.NotifyObservers(message);
            Debug.Log("OnToggle_Play_Clicked");
        }

        private void OnToggle_SpeedDown_Clicked()
        {
            var message = new NebuUnityUIMessage<NebuUnityButtonInput>()
            {
                timeSend = DateTime.Now,
                messageBody = new NebuUnityButtonInput() { msg = "UIButton_SpeedDown", actionItem = promptLabel }
            };
            base.NotifyObservers(message);
            Debug.Log("OnToggle_SpeedDown_Clicked");
        }
        #endregion


        #region ----------///Setting Button Group 设置///---------
        /// <summary>
        /// Setting 设置
        /// </summary>
        public void OnToggle_Setting_Button_Clicked()
        {
            var message = new NebuUnityUIMessage<NebuUnityButtonInput>()
            {
                timeSend = DateTime.Now,
                messageBody = new NebuUnityButtonInput() { msg = "UIButton_Settings", actionItem = promptLabel }
            };
            base.NotifyObservers(message);
            Debug.Log("OnToggle_Setting_Button_Clicked");
        }
        #endregion
    }
}
