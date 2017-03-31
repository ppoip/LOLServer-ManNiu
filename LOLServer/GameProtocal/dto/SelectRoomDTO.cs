using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto
{
    [Serializable]
    public class SelectRoomDTO
    {
        public SelectModel[] teamOne;
        public SelectModel[] teamTwo;

        public SelectModel GetModelByUserId(int userId)
        {
            foreach(var item in teamOne)
            {
                if (item.userId == userId)
                    return item;
            }

            foreach (var item in teamTwo)
            {
                if (item.userId == userId)
                    return item;
            }

            return null;
        }

        public SelectModel SetModel(SelectModel model)
        {
            foreach (var item in teamOne)
            {
                if (item.userId == model.userId)
                {
                    item.name = model.name;
                    item.isReady = model.isReady;
                    item.isEnter = model.isEnter;
                    item.hero = model.hero;
                    return item;
                }
            }

            foreach (var item in teamTwo)
            {
                if (item.userId == model.userId)
                {
                    item.name = model.name;
                    item.isReady = model.isReady;
                    item.isEnter = model.isEnter;
                    item.hero = model.hero;
                    return item;
                }
            }

            return null;
        }

    }
}
