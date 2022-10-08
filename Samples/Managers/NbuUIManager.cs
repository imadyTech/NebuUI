using UnityEngine;
using UnityEngine.Events;
using System;
using imady.NebuEvent;
using UnityEngine.EventSystems;


namespace imady.NebuUI.Samples
{
    public class NbuUIManager : NebuEventUnityObjectBase,
        INebuProvider<NebuUnityUIMessage<NebuUnityButtonInput>>,
        INebuProvider<NebuUnityUIMessage<MouseDragMsg>>,
        IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler//鼠标拖拽、滚动
    {
        private NbuUIViewPool viewPool;
        public TopButtonGroupView topButtonGroupView;
        public MainCanvasView mainView;

        public GameObject mainCanvasContainer => this.gameObject;



        /// <summary>
        /// FLAG: 有时打开一个View需要卫星等数据服务进入后台并暂停
        /// </summary>
        public bool m_IsDataServicePausedFlag = false;
        /// <summary>
        /// FLAG：指示场景的3D/2D模式状态
        /// </summary>
        public string m_earthDisplayMode = "2D";


        protected void Update()
        {
            // 关闭、开启UI信息面板
            if (Input.GetKeyUp(KeyCode.F11))
            {
                mainView.ToggleOnOff();
            }
        }

        /// <summary>
        /// 初始化一个ViewPool(MdyUIManager在App中刚刚被加载时还找不到AppInstance实例，所以要置后调用)
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public NbuUIManager AddPool(Transform parent)
        {
            viewPool = new NbuUIViewPool(AppConfiguration.defaultUITemplatePrefabPath);
            return this;
        }

        public NbuUIManager Initialize(NebuEventManager eventmanager)
        {
            if (viewPool == null)
            {
                Debug.LogException(new Exception("MdyViewPool视窗池子还没完成初始化。"));
                return this;
            }
            topButtonGroupView = GetComponentInChildren<TopButtonGroupView>()
                .AddEventManager(eventmanager) as TopButtonGroupView;
            mainView = GetComponentInChildren<MainCanvasView>()
                .Init(eventmanager)
                .AddEventManager(eventmanager) as MainCanvasView;
            mainView.SetParent(mainCanvasContainer.transform);

            ShowMessageBox(
                        "应用启动完成！",
                        "应用已经完成启动初始化，请关闭提示面板并点击‘SAR覆盖模拟’按钮进行操作。",
                        this.transform,
                        new UnityEngine.Events.UnityAction(() =>
                        {
                            //Close message box
                            HideMessageBox();
                            topButtonGroupView.Toggle_Start_Button.interactable = true;
                        }));
            return this;
        }

        public INbuUIView WakeView(Type viewType, GameObject requester, bool isNewInstance)
        {
            var view = viewPool
                .WakeView(viewType, requester.transform, isNewInstance)
                .GetComponent<INbuUIView>();
            return view;
        }

        [Obsolete("请尽量使用WakeView(Type, GameObject)方法，可支持从attribute读取资源存储路径。")]
        public INbuUIView WakeView(string viewFullName, GameObject requester, bool isNewInstance)
        {
            var view = viewPool.WakeView(viewFullName, requester.transform, isNewInstance);
            //view.transform.parent = requester.transform;
            return view.GetComponent<INbuUIView>();

        }
        public void HibernateView(GameObject view)
        {
            //由ViewPool管理view对象的休眠
            viewPool.HibernateView();
        }


        #region imadyEventSystem INTERFACE IMPLEMENTATION
        #endregion


        #region GLOBAL INPUTS -- MOUSE DRAGGING, KEYBOARD 鼠标、键盘 全局用户操作处理
        // #####--------------------------------------------------#####
        // 在主界面Main_Canvas下挂载了一个透明隐形image面板用于拦截鼠标的操作。
        // 因此界面上的用户操作均被转移到UIManager下，再进行消息分发。
        // #####--------------------------------------------------#####

        Vector3 m_PrevPos = Vector3.zero;//缓存前一段时间偏离屏幕中心点的位置
        Vector3 m_PosDelta = Vector3.zero;//delta
        Vector3 m_PosCenter = Vector3.zero;//delta

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_PrevPos = eventData.position - AppConfiguration.defaultScreenSize;//转换为屏幕中心坐标系
            //Debug.Log($"Mouse BeginDrag: {eventData.position}");
        }

        public void OnScroll(PointerEventData eventData)
        {
            base.NotifyObservers(new NebuUnityUIMessage<MouseDragMsg>()
            {
                messageBody = new MouseDragMsg(eventData.scrollDelta.y)
            });
            //Debug.Log($"[Scroll] {eventData.scrollDelta}");
        }

        public void OnDrag(PointerEventData eventData)
        {
            m_PosDelta = (eventData.position - AppConfiguration.defaultScreenSize) - (Vector2)m_PrevPos;
            m_PosCenter = eventData.position - AppConfiguration.defaultScreenSize;
            m_PrevPos = eventData.position - AppConfiguration.defaultScreenSize;

            //Debug.Log($"Mouse OnDrag: {eventData.position}, [ROTATE] : {mPosDelta}");
            base.NotifyObservers(new NebuUnityUIMessage<MouseDragMsg>()
            {
                messageBody = new MouseDragMsg(
                    m_PosDelta.x / AppConfiguration.defaultScreenSize.x,
                    m_PosDelta.y / AppConfiguration.defaultScreenSize.y,
                    m_PosCenter.x / AppConfiguration.defaultScreenSize.x,
                    m_PosCenter.y / AppConfiguration.defaultScreenSize.y)
            });
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_PrevPos = Vector2.zero;
            //Debug.Log($"Mouse OnEndDrag: {eventData.position}");
        }

        public void OnMouseDrag()
        {
            Debug.Log($"Mouse OnMouseDrag:");

        }
        #endregion




        #region MESSAGEBOX - SPECIAL TYPE OF VIEW 消息弹窗
        private INbuUIView messageBox;
        internal MessageBox WakeMessageBox(MdyMessageBoxViewModel sourceData, Transform parent)
        {
            //var temp = currentView;

            messageBox = (WakeView(typeof(MessageBox), parent.gameObject, false)
                as MessageBox);
            messageBox.SetViewModel(sourceData);

            //Don't let the messageBox affect the CurrentView cache.
            //currentObject = temp.gameObject;
            return messageBox as MessageBox;
        }
        internal MessageBox ShowMessageBox(string title, string msg, UnityAction OKCallback)
        {
            var msgVM = new MdyMessageBoxViewModel(title, msg);
            var box = WakeMessageBox(msgVM, mainCanvasContainer.transform);
            if (OKCallback != null)
                box.OKButton.onClick.AddListener(OKCallback);
            return box;
        }
        internal MessageBox ShowMessageBox(string title, string msg)
        {
            return ShowMessageBox(title, msg, this.HideMessageBox);
        }
        internal MessageBox ShowMessageBox(string title, string msg, Transform parent)
        {
            var msgVM = new MdyMessageBoxViewModel(title, msg);
            var box = WakeMessageBox(msgVM, parent);
            //box.SetParent(parent);
            return box;
        }
        /// <summary>
        /// 显示一个消息盒子
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息主体</param>
        /// <param name="parent">希望跟随的父transform</param>
        /// <param name="OKCallback">用户点击OK按钮后的回调</param>
        /// <returns></returns>
        internal MessageBox ShowMessageBox(string title, string msg, Transform parent, UnityAction OKCallback)
        {
            var box = ShowMessageBox(title, msg, parent);
            if (OKCallback != null)
                box.OKButton.onClick.AddListener(OKCallback);
            return box;
        }
        internal void HideMessageBox()
        {
            (messageBox as MessageBox).OKButton.onClick.RemoveAllListeners();
            HibernateView(messageBox.gameObject);
        }
        #endregion

    }
}