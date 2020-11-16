namespace FutureGames.GamePhysics
{
    public interface ISphersCollidable
    {
        void BallFiredListener();
        void FindSpheres();
        void SomeOneHasGone(string obj);
        void CleanSphersList();
    }
}