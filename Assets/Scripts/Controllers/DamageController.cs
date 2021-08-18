namespace Controllers
{
    public class DamageController : HealthController
    {
        public void AddDamage(float value)
        {
            SubHealth(value);
        }
    }
}
