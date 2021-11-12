using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject[] shootPos;
    public GameObject bulletPrefab;
    public Transform aimUI;
    public GameObject[] missilePos;
    public GameObject missilePrefab;
    public float[] fireTimes;
    private float activeFireTime;
    private float fireTimer;
    private float weaponHeat;
    public float weaponHeatMax;
    public Image weaponHeatUI;
    public GameObject redLock;
    public GameObject canvas;
    private float lockTimer = 0f;
    public float lockTime;
    public GameObject greenLock;
    private GameObject lockedObj;
    public LayerMask lockable;
    public float missileRechargeTime;
    private float missileRechargeTimer;
    public Image missileRechargeUI;

    // Start is called before the first frame update
    void Start()
    {
        GlobalStateMgr.initalize();
        fireTimer = 0f;
        weaponHeat = 0f;
        activeFireTime = fireTimes[0];
        fireTimer = activeFireTime;
        missileRechargeTimer = missileRechargeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalStateMgr.canMove())
        {
            return;
        }
        if (Input.GetButton("Fire1") && fireTimer >= activeFireTime && weaponHeat < weaponHeatMax)
        {
            for (int i =0; i < shootPos.Length; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.position = shootPos[i].transform.position;
                bullet.transform.forward = shootPos[i].transform.position;
                float end_x = this.transform.position.x + 100 * this.transform.forward.x;
                float end_y = this.transform.position.y + 100 * this.transform.forward.y;
                float end_z = this.transform.position.z + 100 * this.transform.forward.z;
                Vector3 lookPosition = new Vector3(end_x, end_y, end_z);
                bullet.GetComponent<BulletMovement>().setStraight(this.transform.forward, lookPosition);
                bullet.transform.LookAt(lookPosition);
                fireTimer = 0;
                weaponHeat++;
                float weaponHeatPercent = ((weaponHeat / weaponHeatMax) * 100);
                if (weaponHeatPercent < 70)
                {
                    activeFireTime = fireTimes[0];
                    
                } else if (weaponHeatPercent > 70 && weaponHeatPercent < 90)
                {
                    activeFireTime = fireTimes[1];
                } else
                {
                    activeFireTime = fireTimes[2];
                }
            }
        }
        Ray ray = Camera.main.ScreenPointToRay(aimUI.position);
        RaycastHit hit;
        //if (Physics.Raycast(ray.origin, ray.direction, out hit))

        if (missileRechargeTimer >= missileRechargeTime && recursiveSphereCast(ray, out hit, 1f, 4f))
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                if (hit.transform.gameObject != lockedObj && lockedObj != null)
                {
                    lockedObj = null;
                    redLock.SetActive(false);
                    greenLock.SetActive(false);
                    lockTimer = 0f;
                }
                if (lockTimer >= lockTime)
                {
                    redLock.SetActive(false);
                    greenLock.SetActive(true);
                    Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, hit.transform.position);
                    greenLock.GetComponent<RectTransform>().anchoredPosition = screenPoint - (canvas.GetComponent<RectTransform>().sizeDelta * canvas.GetComponent<RectTransform>().localScale) / 2f;
                } else
                {
                    lockedObj = hit.transform.gameObject;
                    lockTimer += Time.deltaTime;
                    redLock.SetActive(true);
                    Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, hit.transform.position);
                    redLock.GetComponent<RectTransform>().anchoredPosition = screenPoint - (canvas.GetComponent<RectTransform>().sizeDelta * canvas.GetComponent<RectTransform>().localScale) / 2f;
                }
            } else
            {
                lockedObj = null;
                redLock.SetActive(false);
                greenLock.SetActive(false);
                lockTimer = 0f;
            }
        } else
        {
            lockedObj = null;
            redLock.SetActive(false);
            greenLock.SetActive(false);
            lockTimer = 0f;
        }
        if (Input.GetButtonDown("Fire2") && lockTimer >= lockTime)
        {
            for (int i = 0; i < missilePos.Length; i++)
            {
                GameObject bullet = Instantiate(missilePrefab);
                bullet.transform.position = missilePos[i].transform.position;
                bullet.transform.forward = missilePos[i].transform.forward;
                bullet.GetComponent<Missile>().setTarget(lockedObj);
                bullet.transform.LookAt(lockedObj.transform.position);
            }
            lockedObj = null;
            redLock.SetActive(false);
            greenLock.SetActive(false);
            lockTimer = 0f;
            missileRechargeTimer = 0f;

        }
        fireTimer += Time.deltaTime;
        weaponHeat -= Time.deltaTime * 2f;
        missileRechargeTimer+= Time.deltaTime;
        updateUI();
    }

    void updateUI()
    {
        weaponHeat = Mathf.Clamp(weaponHeat, 0f, weaponHeatMax);
        weaponHeatUI.fillAmount = (weaponHeat / weaponHeatMax);

        missileRechargeTimer = Mathf.Clamp(missileRechargeTimer, 0f, missileRechargeTime);
        missileRechargeUI.fillAmount = (missileRechargeTimer / missileRechargeTime);
    }

    bool recursiveSphereCast(Ray ray, out RaycastHit hit, float i, float final)
    {
        if (Physics.SphereCast(ray.origin, i, ray.direction, out hit, Mathf.Infinity, lockable.value))
        {
            return true;
        } else
        {
            if (i <= final)
            {
                return recursiveSphereCast(ray, out hit, i + 1f, final);
            } else
            {
                return false;
            }
        }
    }
}

