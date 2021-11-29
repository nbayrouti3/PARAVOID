using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DataManagement
{
    [System.Serializable]
    public class PlayerData
    {
        #region FileData (for storing file specific data)

        public bool empty;
        public DateTime dateAndTime;
        public string playerFileName;

        #endregion

        #region PlayerData (for storing all player game data, vector pos and other stuff)

        public string currentScene; //Stores what scene level player will load back to
        public float[] currentPosition = new float[3]; //Stores player postion in scene level
        public float[] currentRotation = new float[4];

        public byte keys;
        public byte memories;

        #endregion

        public PlayerData(Player player)
        {
            if (player != null)
            {
                empty = false;

                //Stores File specifc data
                dateAndTime = player.dateAndTime;
                playerFileName = player.playerFileName;

                keys = player.keys;
                memories = player.Memories;

                //Stores player current position in currentlevel
                currentPosition[0] = player.currentPosition.x;
                currentPosition[1] = player.currentPosition.y;
                currentPosition[2] = player.currentPosition.z;

                //Stores player current rotation in currentlevel
                currentRotation[0] = player.currentRotation.x;
                currentRotation[1] = player.currentRotation.y;
                currentRotation[2] = player.currentRotation.z;
                currentRotation[3] = player.currentRotation.w;

                //Stores the last scene the player loaded to
                currentScene = player.currentScene;
            }
            else
            {
                empty = true;
            }
        }
    }
}
