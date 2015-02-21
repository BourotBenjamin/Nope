using UnityEngine;
using System.Collections;

public class CharactersAttributes : MonoBehaviour {

    [SerializeField]
    private int _hp = 0;
    public int hp {
        get{return _hp;}
        set{_hp = value;}
    }

    [SerializeField]
    private int _currentHP = 0;
    public int currentHP
    {
        get { return _currentHP; }
        set { _currentHP = value; }
    }

    [SerializeField]
    private int _mp = 0;
    public int mp{
        get{return _mp;}
        set{_mp = value;}
    }

    [SerializeField]
    private int _strengh = 0;
    public int strengh
    {
        get { return _strengh; }
        set { _strengh = value; }
    }

    [SerializeField]
    private int _armor = 0;
    public int armor
    {
        get { return _armor; }
        set { _armor = value; }
    }

    [SerializeField]
    private int _attackRange = 0;
    public int attackRange
    {
        get { return _attackRange; }
        set { _attackRange = value; }
    }

    [SerializeField]
    private string _weaponType = "";
    public string weaponType {
        get{return _weaponType;}
        set{_weaponType = value;}
    }

    [SerializeField]
    private string _specifications = "";
    public string specifications
    {
        get { return _specifications; }
        set { _specifications = value; }
    }
    
    [SerializeField]
    private int _mobilityRange = 0;
    public int mobilityRange
    {
        get { return _mobilityRange; }
        set { _mobilityRange = value; }
    }

    [SerializeField]
    private int _moveSpeed = 0;
    public int moveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }
    public bool hurt(int hp)
    {
        _currentHP -= hp;
        if (_currentHP <= 0)
        {
            _currentHP = 0;
            return true;
        }
        return false;
    }
}

