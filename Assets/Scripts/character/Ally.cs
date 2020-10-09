namespace character
{
    public enum AllyState
    {
        Neutral,
        Follow,
        Combat
    }

    /// <summary>
    /// Innocents that are touched by the player are converted into Allies.  Allies patrol randomly and shoot the
    /// closest zombie.
    /// </summary>
    public class Ally : GameCharacter
    {
        private void Start()
        {
            base.Start();
        }
    }
}