using Microsoft.Xna.Framework;

namespace Poloknightse
{
    class HealthPickup : GameObject
    {
        public HealthPickup(Point gridPosition = new Point()) : base(gridPosition, "GameObjects/HealthPickup")
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
