using Delegates;

namespace Controllers
{
    public class DamageController : HealthController
    {
        public ReturnVoid Damage;

        private bool _isNotDamage = false;
        //Invoked in AttackControllers on SendMassage()
        public void AddDamage(float value)
        {
            if (!_isNotDamage) SubHealth(value);
            Damage?.Invoke();
        }

        public void SetDamageState(bool value)
        {
            _isNotDamage = value;
        }
    }
}
