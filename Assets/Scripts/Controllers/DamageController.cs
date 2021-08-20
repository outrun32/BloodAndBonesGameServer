namespace Controllers
{
    public class DamageController : HealthController
    {
        public ReturnVoid Damage;

        private bool _isNotDamage = false;
        //Invoked in AttackControllers on SendMassage()
        public void AddDamage(float value)
        {
            Damage?.Invoke();
            if (!_isNotDamage) SubHealth(value);
        }

        public void SetDamageState(bool value)
        {
            _isNotDamage = value;
        }
    }
}
