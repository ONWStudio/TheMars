namespace TMCard.Runtime
{
    public interface ITMCardMoveBegin
    {
        void OnMoveBegin();
    }

    public interface ITMCardMoveEnd
    {
        void OnMoveEnd();
    }

    public interface ITMCardMove
    {
        void OnMove();
    }
}