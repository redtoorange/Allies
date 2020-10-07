namespace character
{
    /// <summary>
    /// Player controlled GameCharacter.  When I contact an Innocent, they are converted into an Ally, when I contact a
    /// zombie, I lose health.
    /// </summary>
    public class Player : GameCharacter
    {
        private void Start()
        {
            base.Start();
        }
    }
}