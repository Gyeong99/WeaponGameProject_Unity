public abstract class MonsterState {
    
    //���� ȣ��//
    public abstract void Enter(Monster entity);
    //������Ʈ//
    public abstract void Execute(Monster entity);
    //�ش� ���� ����� 1ȸ ȣ��//
    public abstract void Exit(Monster entity);
}
