using UnityEngine;

public class Dongle : MonoBehaviour
{
    public int Level { get; private set; }
    public void SetLevel(int lv) { Level = lv; }
    public bool Dropped { get; private set; }

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    bool _stop;
    void Update()
    {
        if (_stop == Managers.Game.Paused || Dropped == false)
            return;

        _stop = Managers.Game.Paused;
        rigid.simulated = !(_stop);
    }

    public void Drop()
    {
        rigid.simulated = true;
    }

    #region �浹 ����
    void OnCollisionEnter2D(Collision2D coll)
    {
        // �� �ܿ� ó�� �浹 �� ��� ó��
        if(coll.gameObject.CompareTag(ConstVal.WALL) == false && Dropped == false)
        {
            Dropped = true;
            Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Attach);
        }

        // ���� ������ ���۳��� �浹 �� ��ü
        if (coll.gameObject.CompareTag(ConstVal.DONGLE) == false)
            return;
        if (coll.gameObject.GetComponent<Dongle>().Level != Level)
            return;
        Combine(coll.gameObject);
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        // ��� ó�� ������ �н�
        if (coll.CompareTag(ConstVal.OUTLINE) == false)
            return;
        if (Dropped == false)
            return;

        // �̹� ����� ������ �ƿ����ο� ������ ���ӿ���
        Managers.Game.GameOver();
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag(ConstVal.OUTLINE) == false)
            return;

        Dropped = true;
    }

    void Combine(GameObject other)
    {
        if (Level == ConstVal.MAX_LEVEL)
            return;

        // y�� ��ǥ�� ���� �ʿ��� ȣ��
        if (transform.position.y > other.transform.position.y)
            Managers.Game.UpagrageDongle(gameObject, other, Level);
        // y�� ��ǥ�� �����ϴٸ� ���ʿ��� ȣ��
        else if(transform.position.y == other.transform.position.y)
        {
            if(transform.position.x < other.transform.position.x)
                Managers.Game.UpagrageDongle(gameObject, other, Level);
        }
    }
    #endregion

    #region OnEnable / OnDisable
    Vector3 _outOfSight = Vector3.one * -100;
    void OnEnable()
    {
        rigid.simulated = false;
        Dropped = false;
        transform.position = _outOfSight;
        transform.rotation = Quaternion.identity;
    }
    #endregion
}