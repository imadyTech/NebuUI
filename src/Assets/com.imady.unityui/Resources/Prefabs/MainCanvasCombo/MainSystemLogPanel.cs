using imady.NebuEvent;
using imady.NebuLog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace imady.NebuUI.Samples
{
    [NbuResourcePath("MainCanvasCombo/")]
    public class MainSystemLogPanel : NbuUIViewBase, INbuUIView,
        INebuObserver<NebuLogMsg>
    {
        public MainSystemLogViewItem itemTemplate; //��һ��¼����ͼģ�壬��unity editor��Ԥ��ֵ
        public VerticalLayoutGroup systemLogContainer;//��������unity editor��Ԥ��ֵ
        public int maxLogNumber = 100; //���������־�������������Ƴ��������еļ�¼��
        private int logCountIndex = 0;

        Queue<MainSystemLogViewModel> m_logVMs;//��־VM����
        Queue<INbuUIView> m_logView;//ÿ��Log��Ϣ��ItemView���󼯺�
        private Vector3 logContainerHeight => new Vector3(0, systemLogContainer.preferredHeight, 0f);
        private Vector3 logItemHeight = new Vector3(0, 50, 0f);//һ����¼viewItem�ĵ�λ�и߶�
        private Vector3 logViewCurrentPos;//���ڹ���ListView

        protected override void Awake()
        {
            base.Awake();
        }
        public void Init()
        {
            
            m_logVMs = new Queue<MainSystemLogViewModel>();
            m_logView = new Queue<INbuUIView>();
            logViewCurrentPos = systemLogContainer.GetComponent<RectTransform>().anchoredPosition3D;
        }

        /// <summary>
        /// �������ݣ�VM������ͼ��ʽ��Ⱦ
        /// </summary>
        /// <param name="logVMs"></param>
        /// <returns></returns>
        public MainSystemLogPanel SetViewModel(Queue<MainSystemLogViewModel> logVMs)
        {
            m_logVMs = logVMs;//��������
            this.RenderListview<MainSystemLogViewModel>(m_logVMs, systemLogContainer);
            return this;
        }

        /// <summary>
        /// �����������base�ķ��������������
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public new MainSystemLogPanel SetViewModel(object source)
        {
            try
            {
                this.SetViewModel(source as Queue<MainSystemLogViewModel>);
            }
            catch (Exception e)
            {
                Debug.LogError($"{this.name} is not able to cast object to List<MainSystemLogViewModel>.\n{e}");
            }
            return this;
        }

        /// <summary>
        /// ʵ����������¼��viewItem
        /// </summary>
        /// <param name="viewmodel"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        protected virtual INbuUIView RenderItem(MainSystemLogViewModel viewmodel, LayoutGroup container)
        {
            var itemView = GameObject.Instantiate(itemTemplate, container.transform, false)
                    .GetComponent<INbuUIView>();
            itemView.SetViewModel(viewmodel);
            return itemView;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceViewModel">����VM</param>
        /// <param name="container">����item������</param>
        /// <returns></returns>
        protected Queue<INbuUIView> RenderListview<T>(Queue<T> sourceViewModel, LayoutGroup container)
        {
            foreach (MainSystemLogViewModel viewmodel in sourceViewModel as Queue<MainSystemLogViewModel>)
            {
                m_logVMs.Enqueue(viewmodel);
                m_logView.Enqueue(RenderItem(viewmodel, container));
            }
            return m_logView;
        }

        /// <summary>
        /// �������viewItem
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sender"></param>
        public void AddLog(DateTime time, string projectname, string sendername, string loglevel, string message)
        {
            if (m_logVMs == null)
                throw new InvalidOperationException("MainSystemLogPanel is not initialized yet!");
            var itemVM = new MainSystemLogViewModel()
            {
                LeeVM_Time = time,
                LeeVM_Loglevel = loglevel,
                LeeVM_Project = projectname,
                LeeVM_Sender = sendername,
                LeeVM_Message = message
            };
            m_logVMs.Enqueue(itemVM);
            m_logView.Enqueue(RenderItem(itemVM, this.systemLogContainer));
            logCountIndex++;
            if (m_logVMs.Count > maxLogNumber)//��������������Ƴ��������еĶ���
            {
                m_logVMs.Dequeue();
                var earliestItem = m_logView.Dequeue();
                Destroy(earliestItem.gameObject);
            }

            systemLogContainer.GetComponent<RectTransform>().anchoredPosition3D = logViewCurrentPos += logItemHeight;//�Զ�����һ��
            //Debug.Log($"logViewCurrentPos {content} - Pos:{logViewCurrentPos}");
        }

        public void OnNext(NebuLogMsg message)
        {
            AddLog(message.TimeOfLog, message.ProjectName, message.SenderName, message.LogLevel, message.LoggingMessage);
        }
    }

}