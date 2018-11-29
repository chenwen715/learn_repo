﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACSsocket
{
    public class Agv
    {
        /// <summary>
        /// 小车编号
        /// </summary>
        public string agvNo;

        /// <summary>
        /// 小车当前的码值
        /// </summary>
        public string barcode;

        /// <summary>
        /// 小车需要重建路径的需求度，值越大需求越高
        /// </summary>
        public int RePathLevel;
        /// <summary>
        /// 小车需要重建路径的时刻
        /// </summary>
        public DateTime drepath;
        /// <summary>
        /// 小车状态
        /// </summary>
        public AgvState state;

        /// <summary>
        /// 小车是否可用
        /// </summary>
        public bool isEnable;

        /// <summary>
        /// 小车当前电量
        /// </summary>
        public float currentCharge;

        /// <summary>
        /// 小车当前的顶升状态
        /// </summary>
        public HeightEnum height;

        /// <summary>
        /// 小车当前子任务列表
        /// </summary>
        public List<STask> sTaskList;

        public int errorMsg;

        public string areaNo;
    }
}