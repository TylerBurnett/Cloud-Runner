using System;
using UnityEngine;


namespace Game.Gameplay
{
    [Serializable]
    public class GameServiceSettings
    {
        public float DeathWallInitialSpeed = 10.0f;

        public float DeathWallSpeedIncrement = 0.01f;

        public float DeathWallMaxSpeed = 58;

        public float StartDistance = 100.0f;

        [Tooltip("Controls the distance a player can be from the death wall before they die")]
        public float DeathWallMinDistance = 10.0f;

        [Tooltip("Controls the minimum Y axis a player can be before dying")]
        public float GameFloor = -10.0f;
    }
}
