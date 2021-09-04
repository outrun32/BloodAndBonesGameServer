using Controllers;
using Controllers.Character;
using UnityEngine;

namespace Delegates
{
    public delegate void ReturnVoid();
    public delegate void ReturnInt(int value);
    public delegate void ReturnFloat(float value);
    public delegate void ReturnPlayer(Player value);
    public delegate void ReturnTwoPlayer(Player player1, Player player2);
    public delegate void ReturnCharacter(Character player);
    public delegate void ReturnClient(Client player);
    public delegate void ReturnTwoCharacter(Character player1, Character player2);
    public delegate void ReturnDamage(float value, DamageType damageType);
    public delegate void ReturnDamageCharacter(float value, DamageType damageType, Character character);
    
}

