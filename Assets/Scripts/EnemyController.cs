using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 3f;
    [SerializeField] private Transform visualRoot;
    [SerializeField] private bool spriteFacingRightByDefault = true;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 2f; // كل قد إيه يعمل انيميشن ضرب

    private Transform _dragon;
    private int _moveDirection = -1;
    private float _lastAttackTime;
    private Animator _anim;

    private void Start()
    {
        GameObject dragonGO = GameObject.FindGameObjectWithTag("Player");
        if (dragonGO != null)
            _dragon = dragonGO.transform;
        else
            Debug.LogWarning("EnemyController: No object with tag 'Player' found!");

        _anim = GetComponentInChildren<Animator>();
    }

    public void Init(int direction)
    {
        _moveDirection = direction;
        UpdateVisualFacing(direction);
    }

    private void Update()
    {
        if (_dragon == null) return;

        float dx = _dragon.position.x - transform.position.x;
        float distanceX = Mathf.Abs(dx);

        int desiredDir = dx > 0 ? 1 : -1;
        if (desiredDir != _moveDirection)
        {
            _moveDirection = desiredDir;
            UpdateVisualFacing(desiredDir);
        }

        if (distanceX > stopDistance)
        {
            MoveTowardsDragon();
        }
        else
        {
            // يقف ويعمل انيميشن ضرب بس
            TryAttack();
        }
    }

    private void MoveTowardsDragon()
    {
        Vector3 pos = transform.position;
        pos.x += _moveDirection * moveSpeed * Time.deltaTime;
        transform.position = pos;
    }

    private void TryAttack()
    {
        if (_anim == null) return;

        if (Time.time - _lastAttackTime < attackCooldown)
            return;

        _lastAttackTime = Time.time;

        // نشغّل الانيميشن بتاعة الضرب بس
        _anim.SetTrigger("Attack");
    }

    private void UpdateVisualFacing(int dir)
    {
        if (visualRoot == null) return;

        int visualDir = dir;
        if (!spriteFacingRightByDefault)
            visualDir *= -1;

        Vector3 scale = visualRoot.localScale;
        scale.x = Mathf.Abs(scale.x) * (visualDir > 0 ? 1 : -1);
        visualRoot.localScale = scale;
    }
}
