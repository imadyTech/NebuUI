using System;
using imady.NebuEvent;

namespace imady.NebuUI.Samples
{
    [NbuResourcePath("UIViews/")]
    public class MainCanvasView : NbuUIViewBase
    {
        /// <summary>
        /// 指向MainSystemLogPanel系统消息面板
        /// </summary>
        public MainSystemLogPanel mainSystemLogPanel;

        protected override void Awake()
        {
            base.Awake();
        }
        public MainCanvasView Init(NebuEventManager eventmanager)
        {
            mainSystemLogPanel.AddEventManager(eventmanager);

            mainSystemLogPanel.Init();
            return this;
        }

        public void AddSystemLog(string content, string sender)
        {
            mainSystemLogPanel.AddLog(DateTime.Now, App.Instance.name, sender, "Trace", content);
        }

        public override void ToggleOnOff()
        {
            //MainCanvasView不通过viewPool管理，因此需要覆盖基类方法
            base.isOnOff = !base.isOnOff;
            if (base.isOnOff)
                this.gameObject.SetActive(true);
            else
                this.gameObject.SetActive(false);

        }

    }
}
