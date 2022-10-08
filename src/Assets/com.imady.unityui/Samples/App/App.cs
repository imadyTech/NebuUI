#region Copyrights imady
/*
 *Copyright(C) 2020 by imady Technology (Suzhou); All rights reserved.
 *Author:       Frank Shen
 *Date:         2020-07-31
 *Description:   
 */
#endregion

using System;
using UnityEngine;
using imady.NebuEvent;

namespace imady.NebuUI.Samples
{
    public class App : NebuSingleton<App>
    {
        #region GameObjects & Managers对象定义
        //Nebu辅助开发工具，用于监控运行时进程
        public GameObject NebuEventMgrGO;
        public GameObject NebuSceneMgrGO;
        public GameObject NebuUiMgrGO;
        public GameObject MainCanvasObject;
        public GameObject NebuCameraMgrGO;

        public NbuUIManager uiManager;
        public NbuTheatreManager theatreManager;
        public NebuEventManager eventManager;
        public NebulogManager nebulogManager;
        #endregion

        #region MonoBehaviour Methods
        protected override void Awake()
        {
            base.Awake();

            //========================================
            //App对象及App.cs脚本必须自始至终都在场景中存在。
            //DontDestroyOnLoad(this);
            //========================================


            InitializeAppConfiguration();
            InitializeManagers(null, null);
        }

        void Update()
        {
            // exit
            if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("[NebulogAPP应用程序]: 应用程序已经退出。");
                Application.Quit();
            }

        }

        void OnApplicationQuit()
        {
            //停止 TODO: remove all listeners registered
        }
        #endregion



        private void InitializeAppConfiguration()
        {
        }


        /// <summary>
        /// Add the manager objects and components after the logger is ready
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitializeManagers(object sender, EventArgs e)
        {
            //初始化所有Manager。。。
            //eventManager必须在最先加载
            eventManager = new NebuEventManager();

            /// 剧场管理器，负责管理被操作对象（NebuInteractable）的生成、销毁、入场、离场
            if (NebuSceneMgrGO != null) theatreManager = ((NbuTheatreManager)NebuSceneMgrGO
                .AddComponent<NbuTheatreManager>()
                .AddEventManager(this.eventManager))//这是NebuTheatreManager自己注册到eventsystem
                .AddPool(this.NebuSceneMgrGO.transform);
            //.AddDataService(satelliteService);
            Debug.Log("[Nebu剧场对象管理器]：NebuTheatreManager 初始化完成。");

            //添加用户界面管理器
            if (NebuUiMgrGO != null) uiManager = ((NbuUIManager)NebuUiMgrGO
                .AddComponent<NbuUIManager>()
                .AddEventManager(this.eventManager))
                .AddPool(MainCanvasObject.transform)
                .Initialize(this.eventManager);
            uiManager.mainView.AddSystemLog("UI启动完成", this.name);
            Debug.Log("[Nebu用户界面管理器]：NebuUIMananger UI管理器初始化完成。");

            //重要：EventSystem进行匹配subscribe。
            theatreManager.Initialize(this.eventManager);//这是为自己管理的对象注册到eventsystem
            uiManager.mainView.AddSystemLog("对象加载完成！", this.name);



            nebulogManager = NebuEventMgrGO
                .AddComponent<NebulogManager>()
                .AddEventManager(this.eventManager) as NebulogManager;


            //eventManager.MappingEventObjects();//Deprecated 2022-10-01
            eventManager.MappingEventObjectsByInterfaces();

            Debug.Log("[Nebu消息系统]：iNebuEventManager初始化完成。");
            //Debug.Log(NebuStateObjectBase.SubscribeLog);

            //丢个加载完成的消息出去
            this.AfterNebuManagersInitialized(this, new EventArgs());
            Debug.Log("[Nebu应用管理器]: 应用程序加载完成！");
            uiManager.mainView.AddSystemLog("应用程序启动完成。", this.name);
        }


        /// <summary>
        /// 完成管理器加载后要处理的事务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AfterNebuManagersInitialized(object sender, EventArgs e)
        {

        }
    }
}