using UnityEngine;

namespace Models
{
    public struct PlayerSendingDataModel
    {
        public int ID;
        public Transform Transform;
        public AnimationModel AnimationModel;
        public float Health;
        public float Mana;

        public PlayerSendingDataModel(int id, Transform transform, AnimationModel animationModel, float health, float mana)
        {
            ID = id;
            Transform = transform;
            AnimationModel = animationModel;
            Health = health;
            Mana = mana;
        }
    }
}
