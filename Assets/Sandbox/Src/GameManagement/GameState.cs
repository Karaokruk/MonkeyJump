public enum GameState
{
    IDLE, /* No level is yet loaded, or in-between-levels transition is playing */
    PREGAME, /* Monkey does NOT move by itself, only Player can drag it */
    INGAME /* Monkey does move by itself, Player can interact with the rest of the environment */
}
