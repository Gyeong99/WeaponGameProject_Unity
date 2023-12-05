public abstract class MonsterState {
    
    //최초 호출//
    public abstract void Enter(Monster entity);
    //업데이트//
    public abstract void Execute(Monster entity);
    //해당 상태 종료시 1회 호출//
    public abstract void Exit(Monster entity);
}
