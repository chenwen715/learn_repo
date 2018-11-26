using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACSsocket
{
    public enum HeightEnum
    {
        Low = 1,
        Middle = 2,
        High = 3,
    };

    public enum AgvState
    {
        //10：自动、无码（报警）；11:自动、空闲；12:自动、歪码（报警、占位）；
        //13：忙碌；14:充电中；20:手动、无码；21:手动、有码（占位）
        D1 = 1,
        D2 = 2,
        D3 = 3,
        D4 = 4,
        D5 = 5,
        D6 = 6,
        D7 = 7,
        D8 = 8,
        D9 = 9,
        D10 = 10,
        D11 = 11,
        D12 = 12,
        D13 = 13,
        D14 = 14,
        D15 = 15,
        D17 = 17,
        D18 = 18,
        D19 = 19,
        D20 = 20,
        D21 = 21,
    };

    public enum TaskState
    {
        Init = 0,
        Down = 1,
        PathFail = 2,
        PathSuccess = 3,
    };

    public enum STaskType
    {
        //  1	行走
        D1 = 1,

        //2	顶升
        D2 = 2,

        //3	下降
        D3 = 3,

        //4	直接顶升
        D4 = 4,

        //5	直接下降
        D5 = 5,

        //6	充电
        D6 = 6,

        //7	取消充电
        D7 = 7,

        //8	原地旋转
        D8 = 8,

        //9	左弧

        D9 = 9,

        //10	右弧

        D10 = 10,

        //11	旋转左弧出

        D11 = 11,

        //12	旋转右弧出

        D12 = 12,


    };

    public enum TypeEnum
    {
        Heart = 0,          //心跳
        NewAction = 1,      //新动作
        Repeat = 2,         //重发
        Finish = 3,         //任务完成
    };

    public enum rTypeEnum
    {      
        Repeat = 1,         //重发
        Finish = 2,         //任务完成
        NewAction = 3,      //新动作
    };
}
