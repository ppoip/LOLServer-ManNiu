using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto
{
    [Serializable]
    public class SelectModel
    {
        public int userId;       //角色id
        public string name;      //角色名称
        public int hero;         //角色所选的英雄的id
        public bool isEnter;     //角色是否已经进入房间
        public bool isReady;     //角色是否已经准备
    }
}
