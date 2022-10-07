using System;

namespace imady.NebuUI.Samples
{
    /// <summary>
    /// 一条系统消息的VM
    /// </summary>
    public class MainSystemLogViewModel
    {
        [NbuViewProperty]
        public DateTime LeeVM_Time { get; set; }

        [NbuViewProperty]
        public string LeeVM_Loglevel { get; set; }

        [NbuViewProperty]
        public string LeeVM_Project { get; set; }

        [NbuViewProperty]
        public string LeeVM_Sender { get; set; }

        [NbuViewProperty]
        public string LeeVM_Message { get; set; }

        public MainSystemLogViewModel()
        {

        }
    }

    [NbuResourcePath("MainCanvasCombo/")]
    public class MainSystemLogViewItem : NbuUIViewBase, INbuUIView
    {
    }
}