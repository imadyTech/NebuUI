using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using imady.NebuEvent;
using imady.NebuLog;
using imady.NebuLog.DataModel;
using imady.NebuLog.Loggers;
using Microsoft.Extensions.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace imady.NebuUI.Samples
{
    public class NebulogManager : NebuEventUnityObjectBase,
        INebuObserver<NebuUnityUIMessage<NebulogServerInitiateMsg>>,
        INebuProvider<NebuLogMsg>,
        INebuProvider<NebuUnityUIMessage<NebulogServerConnectedMsg>>
    {
        public static INebuLogger logger;


        private List<NebuLogMsg> _messageList;
        public List<NebuLogMsg> messageList
        {
            get { if (_messageList == null) _messageList = new List<NebuLogMsg>(); return _messageList; }
            set => _messageList = value;
        }

        private List<NebuLogStatMsg> _statList;
        private long _messageCount;
        public List<NebuLogStatMsg> statList
        {
            get { if (_statList == null) _statList = new List<NebuLogStatMsg>(); return _statList; }
            set => _statList = value;
        }


        #region 响应来自NebuLogHub的事件，进行前端视图的处理
        int tempNebulogMsgLocker = 0;
        ConcurrentQueue<NebuLogMsg> messagesCache = new ConcurrentQueue<NebuLogMsg>();
        public void ReceiveOnILogging(DateTime time, string projectname, string sourcename, string loglevel, string message)
        {
            if (0 == Interlocked.Exchange(ref tempNebulogMsgLocker, 1))
            {
                messagesCache.Enqueue(new NebuLogMsg()
                {
                    TimeOfLog = time,
                    ProjectName = projectname,
                    SenderName = sourcename,
                    LogLevel = loglevel,
                    LoggingMessage = message
                });
                Interlocked.Exchange(ref tempNebulogMsgLocker, 0);
            }
            else //... TODO: 这里可能出现接受数据不成功状况，应回复发送者，要求重新发送
                Debug.LogError($"[ReceiveOnILogging Thread Conflict] {time}/{projectname}/{sourcename}/{loglevel} not received.");
        }

        #endregion

        NebuLogMsg tempMsg = null;
        public void Update()
        {
            if (messagesCache.IsEmpty) return;

            if (0 == Interlocked.Exchange(ref tempNebulogMsgLocker, 1))
            {
                messagesCache.TryDequeue(out tempMsg);
                messageList.Add(tempMsg);
                Debug.Log($"[Nebulog ReceiveOnILogging] {messageList.Count}");
                base.NotifyObservers(tempMsg);
                Interlocked.Exchange(ref tempNebulogMsgLocker, 0);
            }
            else
                Debug.LogWarning($"[Render Thread Conflict]...");
        }


        #region imady.NebuyEvent System INTERFACE IMPLEMENTATION
        public void OnNext(NebuUnityUIMessage<NebulogServerInitiateMsg> message)
        {
            //System.Diagnostics.Process.Start(Application.streamingAssetsPath + "\\" + AppConfiguration.multiSateProductName);

            #region 加载 Nebulogger
            var option = new NebuLogOption()
            {
                NebuLogHubUrl = "http://localhost/NebuLogHub",
                ProjectName = Application.productName,
                LogLevel = LogLevel.Trace
            };

            logger = new imady.NebuLog.Loggers.NebuLogger(option, SceneManager.GetActiveScene().name);
            //logger.NebulogHubConnection.On<DateTime, string, string, string, string>("OnILogging", this.ReceiveOnILogging);
            logger.NebulogConnected += ((o,e) => { 
                Debug.Log("NebuLogger Connected.");
                base.NotifyObservers(new NebuUnityUIMessage<NebulogServerConnectedMsg>()
                {
                    messageBody = new NebulogServerConnectedMsg()
                });
            });


            // 注册Debug.Log响应委托
#if UNITY_4
                Application.RegisterLogCallback(HandleUnityLogs);
#else
            Application.logMessageReceived += logger.HandleUnityLogs;
#endif

            #endregion
        }
        #endregion
    }
}
