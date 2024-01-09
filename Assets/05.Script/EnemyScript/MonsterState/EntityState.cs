using UnityEngine;

public abstract class EntityState<T>
{
    //최초 호출//
    public virtual void SetUp(T entity)
    {

    }
    public abstract void Enter(T entity);
    //업데이트//
    public abstract void Execute(T entity);

    //해당 상태 종료시 1회 호출//
    public abstract void Exit(T entity);
}
