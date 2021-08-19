namespace Controllers
{
    public class DamageController : HealthController
    {
        public ReturnVoid Damage;
        //Invoked in AttackControllers on SendMassage()
        public void AddDamage(float value)
        {
            Damage?.Invoke();
            SubHealth(value);
        }
    }
}
