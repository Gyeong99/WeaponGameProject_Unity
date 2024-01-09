using UnityEngine;

public abstract class EntityState<T>
{
    //���� ȣ��//
    public virtual void SetUp(T entity)
    {

    }
    public abstract void Enter(T entity);
    //������Ʈ//
    public abstract void Execute(T entity);

    //�ش� ���� ����� 1ȸ ȣ��//
    public abstract void Exit(T entity);
}
