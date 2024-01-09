using UnityEngine;

public abstract class MonsterBaseEntity : MonoBehaviour
{
    //���� ���� , ���� ID�̴�.
    private static int m_iNextMonsterID = 0;
    //�� Ŭ������ ���� ��ӹ޴� ���͵��� ID�� 1�� �����Ͽ� ������ȣ�� ������.

    private int id;
    public int ID
    {
        set {
            id = value;
            m_iNextMonsterID++;
        }
        get => id;
    }

    private string entityName;      //��ƼƼ �̸�

    //�Ļ� Ŭ�������� base.Setup()���� ȣ��.
    public virtual void SetUp(string name)
    {
        //���� ��ȣ ����
        ID = m_iNextMonsterID;
        //�̸� ����
        entityName = name;  
    }

    //GameController Ŭ�������� ��� ������Ʈ�� Updated()�� ȣ���� ������Ʈ�� �����Ѵ�.
    public abstract void Updated();

    public void PrintText(string text)
    {
        Debug.Log($"{entityName} : {text}");
    }
}
