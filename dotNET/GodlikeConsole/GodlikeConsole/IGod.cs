using GodlikeConsole.Humans;

namespace GodlikeConsole
{
    internal interface IGod
    {
        Human CreateHuman();
        Human CreateHuman(Sex gender);
        Human CreatePair(Human human);
    }
}
