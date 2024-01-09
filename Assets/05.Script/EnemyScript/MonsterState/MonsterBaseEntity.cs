using UnityEngine;

public abstract class MonsterBaseEntity : MonoBehaviour
{
    //정적 변수 , 몬스터 ID이다.
    private static int m_iNextMonsterID = 0;
    //이 클래스를 통해 상속받는 몬스터들은 ID가 1씩 증가하여 고유번호로 존재함.

    private int id;
    public int ID
    {
        set {
            id = value;
            m_iNextMonsterID++;
        }
        get => id;
    }

    private string entityName;      //엔티티 이름

    //파생 클래스에서 base.Setup()으로 호출.
    public virtual void SetUp(string name)
    {
        //몬스터 번호 설정
        ID = m_iNextMonsterID;
        //이름 설정
        entityName = name;  
    }

    //GameController 클래스에서 모든 에이전트의 Updated()를 호출해 에이전트를 구동한다.
    public abstract void Updated();

    public void PrintText(string text)
    {
        Debug.Log($"{entityName} : {text}");
    }
}
